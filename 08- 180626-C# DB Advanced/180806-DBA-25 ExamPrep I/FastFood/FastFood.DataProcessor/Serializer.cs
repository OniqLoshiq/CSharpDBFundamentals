using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var userOrder = context.Employees.ToArray()
                .Where(e => e.Name == employeeName)
                .Select(e => new
                {
                    Name = e.Name,
                    Orders = e.Orders
                              .Where(o => o.Type.ToString() == orderType)
                              .Select(o => new
                              {
                                  Customer = o.Customer,
                                  Items = o.OrderItems.Select(oi => new
                                  {
                                      Name = oi.Item.Name,
                                      Price = oi.Item.Price,
                                      Quantity = oi.Quantity
                                  }).ToArray(),
                                  TotalPrice = o.TotalPrice
                              })
                              .OrderByDescending(o => o.TotalPrice)
                              .ThenByDescending(o => o.Items.Count())
                              .ToArray(),
                    TotalMade = e.Orders.Where(t => t.Type.ToString() == orderType).Sum(p => p.TotalPrice)
                })
                .SingleOrDefault();

            var jsonOrders = JsonConvert.SerializeObject(userOrder, new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            });

            return jsonOrders;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            var categoryRequested = categoriesString.Split(',');

            var categories = context.Categories.Where(c => categoryRequested.Any(x => x == c.Name))
                                                .Select(s => new CategoryDto()
                                                {
                                                    Name = s.Name,
                                                    Item = s.Items.Select(i => new ItemDto()
                                                    {
                                                        Name = i.Name,
                                                        TotalMade = i.OrderItems.Sum(p => p.Item.Price * p.Quantity),
                                                        TimesSold = i.OrderItems.Sum(p => p.Quantity)
                                                    })
                                                    .OrderByDescending(x => x.TotalMade)
                                                    .ThenByDescending(x => x.TimesSold)
                                                    .FirstOrDefault()
                                                })
                                                .OrderByDescending(x => x.Item.TotalMade)
                                                .ThenByDescending(x => x.Item.TimesSold)
                                                .ToArray();

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), categories, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            return sb.ToString().Trim();
        }
    }
}