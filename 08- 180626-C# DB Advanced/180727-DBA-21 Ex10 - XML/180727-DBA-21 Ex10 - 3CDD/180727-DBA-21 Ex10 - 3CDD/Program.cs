using _180727_DBA_21_Ex10___3CDD.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10___3CDD
{
    class Program
    {
        static void Main(string[] args)
        {
            //01-SetupDatabase
            //using (var ctx = new CarDealerDbContext())
            //{
            //    ctx.Database.Migrate();
            //}

            //02-ImportData
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var mapper = config.CreateMapper();

            //ImportSuppliers(mapper);
            //ImportParts(mapper);
            //ImportCars(mapper);
            //ImportPartCars();
            //var customers = ImportCustomers(mapper);
            //ImportSales(customers);

            var ctx = new CarDealerDbContext();


            //Query 1. Cars with Distance
            //GetCarsWithDistance(ctx);

            //Query 2. Cars from make Ferrari
            //GetCarsFromFerrari(ctx);

            //Query 3. Local Suppliers
            //GetLocalSuppliers(ctx);

            //Query 4. Cars with Their List of Parts
            //GetCarsWithListOfParts(ctx);

            //Query 5. Total Sales by Customer
            //GetTotalSalesByCustomer(ctx, mapper);

            //Query 6. Sales with Applied Discount
            //GetSalesWithAppliedDiscount(ctx, mapper);
        }

        private static void GetSalesWithAppliedDiscount(CarDealerDbContext ctx, IMapper mapper)
        {
            var sales = ctx.Sales.ProjectTo<Q6_SaleDto>(mapper.ConfigurationProvider).ToArray();

            var serializer = new XmlSerializer(typeof(Q6_SaleDto[]), new XmlRootAttribute("sales"));

            using (var writer = new StreamWriter("../../../XML/Outer/sales-discount.xml"))
            {
                serializer.Serialize(writer, sales, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetTotalSalesByCustomer(CarDealerDbContext ctx, IMapper mapper)
        {
            var customers = new Q5_CustomerRootDto()
            {
                Customers = ctx.Customers
                                .Where(c => c.Sales.Count >= 1)
                                .ProjectTo<Q5_CustomerDto>(mapper.ConfigurationProvider)
                                .OrderByDescending(c => c.TotalSpentMoney)
                                .OrderByDescending(c => c.BoughtCars)
                                .ToArray()
            };

            var serializer = new XmlSerializer(typeof(Q5_CustomerRootDto), new XmlRootAttribute("customers"));

            using (var writer = new StreamWriter("../../../XML/Outer/customers-total-sales.xml"))
            {
                serializer.Serialize(writer, customers, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetCarsWithListOfParts(CarDealerDbContext ctx)
        {
            var cars = ctx.Cars
                              .Select(c => new Q4_CarDto()
                              {
                                  Make = c.Make,
                                  Model = c.Model,
                                  TravelledDistance = c.TravelledDistance,
                                  Parts = c.PartCars.Select(p => new Q4_PartDto()
                                  {
                                      Name = p.Part.Name,
                                      Price = p.Part.Price
                                  }).ToArray()
                              }).ToArray();


            var serializer = new XmlSerializer(typeof(Q4_CarDto[]), new XmlRootAttribute("cars"));

            using (var writer = new StreamWriter("../../../XML/Outer/cars-and-parts.xml"))
            {
                serializer.Serialize(writer, cars, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetLocalSuppliers(CarDealerDbContext ctx)
        {
            var suppliers = ctx.Suppliers
                              .Where(s => !s.IsImporter)
                              .Select(s => new Q3_SupplierDto()
                              {
                                  Id = s.Id,
                                  Name = s.Name,
                                  PartsCount = s.Parts.Count
                              }).ToArray();


            var serializer = new XmlSerializer(typeof(Q3_SupplierDto[]), new XmlRootAttribute("suppliers"));

            using (var writer = new StreamWriter("../../../XML/Outer/local-suppliers.xml"))
            {
                serializer.Serialize(writer, suppliers, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetCarsFromFerrari(CarDealerDbContext ctx)
        {
            var cars = ctx.Cars
                              .Where(c => c.Make == "Ferrari")
                              .OrderBy(c => c.Model)
                              .ThenByDescending(c => c.TravelledDistance)
                              .Select(c => new Q2_CarDto()
                              {
                                  Id = c.Id,
                                  Model = c.Model,
                                  TravelledDistance = c.TravelledDistance
                              }).ToArray();


            var serializer = new XmlSerializer(typeof(Q2_CarDto[]), new XmlRootAttribute("cars"));

            using (var writer = new StreamWriter("../../../XML/Outer/ferrari-cars.xml"))
            {
                serializer.Serialize(writer, cars, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetCarsWithDistance(CarDealerDbContext ctx)
        {
            var cars = ctx.Cars
                              .Where(c => c.TravelledDistance > 2_000_000)
                              .OrderBy(c => c.Make)
                              .ThenBy(c => c.Model)
                              .Select(c => new Q1_CarDto()
                              {
                                  Make = c.Make,
                                  Model = c.Model,
                                  TravelledDistance = c.TravelledDistance
                              }).ToArray();


            var serializer = new XmlSerializer(typeof(Q1_CarDto[]), new XmlRootAttribute("cars"));

            using (var writer = new StreamWriter("../../../XML/Outer/cars.xml"))
            {
                serializer.Serialize(writer, cars, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void ImportSales(List<Customer> customers)
        {
            List<Sale> sales = new List<Sale>();

            double[] discountValues = new[] { 0, 0.05, 0.1, 0.15, 0.2, 0.3, 0.4, 0.5 };
            var carIds = Enumerable.Range(1, 358).OrderBy(x => new Random().Next()).Take(80).ToArray();

            for (int i = 0; i < 80; i++)
            {
                int customerId = new Random().Next(1, 31);
                int carId = carIds[i];               
                double discount = discountValues[new Random().Next(discountValues.Length)];

                if(customers.Single(c => c.Id == customerId).IsYoungDriver)
                {
                    discount += 0.05;
                }

                var sale = new Sale()
                {
                    CustomerId = customerId,
                    CarId = carId,
                    Discount = discount
                };

                sales.Add(sale);
            }

            var ctx = new CarDealerDbContext();
            ctx.Sales.AddRange(sales);
            ctx.SaveChanges();

        }

        private static List<Customer> ImportCustomers(IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XML/Inner/customers.xml");

            var serializer = new XmlSerializer(typeof(CustomerDto[]), new XmlRootAttribute("customers"));
            var deserializedCustomers = (CustomerDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Customer> customers = new List<Customer>();

            foreach (var customerDto in deserializedCustomers)
            {
                var customer = mapper.Map<Customer>(customerDto);

                customers.Add(customer);
            }

            var ctx = new CarDealerDbContext();
            ctx.Customers.AddRange(customers);
            ctx.SaveChanges();

            return customers;
        }

        private static void ImportPartCars()
        {
            List<PartCar> partCars = new List<PartCar>();

            for (int i = 1; i <= 358; i++)
            {
                var parts = Enumerable.Range(1, 131).OrderBy(x => new Random().Next()).Take(new Random().Next(10,21)).ToArray();

                for (int n = 0; n < parts.Length; n++)
                {
                    int partId = parts[n];

                    var partCar = new PartCar()
                    {
                        CarId = i,
                        PartId = partId
                    };

                    partCars.Add(partCar);
                }
            }

            var ctx = new CarDealerDbContext();
            ctx.PartCars.AddRange(partCars);
            ctx.SaveChanges();
        }

        private static void ImportCars(IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XML/Inner/cars.xml");

            var serializer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("cars"));
            var deserializedCars = (CarDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Car> cars = new List<Car>();

            foreach (var carDto in deserializedCars)
            {
                var car = mapper.Map<Car>(carDto);

                cars.Add(car);
            }
            
            var ctx = new CarDealerDbContext();
            ctx.Cars.AddRange(cars);
            ctx.SaveChanges();
        }

        private static void ImportParts(IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XML/Inner/parts.xml");

            var serializer = new XmlSerializer(typeof(PartDto[]), new XmlRootAttribute("parts"));
            var deserializedParts = (PartDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Part> parts = new List<Part>();

            foreach (var partDto in deserializedParts)
            {
                var part = mapper.Map<Part>(partDto);

                int supplierId = new Random().Next(1, 32);

                part.SupplierId = supplierId;

                parts.Add(part);
            }

            var ctx = new CarDealerDbContext();
            ctx.Parts.AddRange(parts);
            ctx.SaveChanges();
        }

        private static void ImportSuppliers(IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XML/Inner/suppliers.xml");

            var serializer = new XmlSerializer(typeof(SupplierDto[]), new XmlRootAttribute("suppliers"));
            var deserializedSuppliers = (SupplierDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var supplierDto in deserializedSuppliers)
            {
                var supplier = mapper.Map<Supplier>(supplierDto);

                suppliers.Add(supplier);
            }

            var ctx = new CarDealerDbContext();
            ctx.Suppliers.AddRange(suppliers);
            ctx.SaveChanges();
        }
    }
}
