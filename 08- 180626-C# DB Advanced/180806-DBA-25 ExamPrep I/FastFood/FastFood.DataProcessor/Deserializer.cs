using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            var deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            List<Employee> employees = new List<Employee>();

            var sb = new StringBuilder();

            foreach (var e in deserializedEmployees)
            {
                if(!IsValid(e))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = GetOrCreatePosition(context, e.Position);

                var employee = new Employee()
                {
                    Name = e.Name,
                    Age = e.Age,
                    Position = position
                };

                employees.Add(employee);
                sb.AppendLine(string.Format(SuccessMessage, e.Name));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().Trim();
		}

        public static string ImportItems(FastFoodDbContext context, string jsonString)
		{
            var deserializedItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            List<Item> items = new List<Item>();

            var sb = new StringBuilder();

            foreach (var item in deserializedItems)
            {
                if(!IsValid(item))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if(items.Any(i => i.Name == item.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Category category = GetOrCreateCategory(context, item.Category);

                var newItem = new Item()
                {
                    Name = item.Name,
                    Price = item.Price,
                    Category = category
                };

                items.Add(newItem);

                sb.AppendLine(string.Format(SuccessMessage, newItem.Name));
            }

            context.AddRange(items);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var deserializedOrders = (OrderDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Order> orders = new List<Order>();
            List<OrderItem> orderItems = new List<OrderItem>();

            var sb = new StringBuilder();

            foreach (var orderDto in deserializedOrders)
            {
                bool isValidItem = true;

                if(!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                }

                foreach (var io in orderDto.Items)
                {
                    if(!IsValid(io) || !context.Items.Any(i => i.Name == io.Name))
                    {
                        isValidItem = false;
                        break;
                    }
                }

                if(!isValidItem)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var employee = context.Employees.SingleOrDefault(e => e.Name == orderDto.EmployeeName);

                if(employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var date = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var orderType = (OrderType)Enum.Parse(typeof(OrderType), orderDto.Type);

                var order = new Order()
                {
                    Customer = orderDto.Customer,
                    DateTime = date,
                    Employee = employee,
                    Type = orderType
                };

                orders.Add(order);

                foreach (var itemDto in orderDto.Items)
                {
                    var item = context.Items.First(i => i.Name == itemDto.Name);

                    var orderItem = new OrderItem()
                    {
                        Order = order,
                        Quantity = itemDto.Quantity,
                        Item = item
                    };

                    orderItems.Add(orderItem);
                }

                sb.AppendLine($"Order for {order.Customer} on {date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InstalledUICulture)} added");
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static Category GetOrCreateCategory(FastFoodDbContext context, string categoryName)
        {
            Category category = context.Categories.SingleOrDefault(c => c.Name == categoryName);

            if(category == null)
            {
                category = new Category() { Name = categoryName };

                context.Categories.Add(category);
                context.SaveChanges();
            }

            return category;
        }

        private static Position GetOrCreatePosition(FastFoodDbContext context, string positionName)
        {
            Position position = context.Positions.SingleOrDefault(p => p.Name == positionName);

            if(position == null)
            {
                position = new Position()
                {
                    Name = positionName
                };

                context.Positions.Add(position);
                context.SaveChanges();
            }

            return position;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return result;
        }
	}
}