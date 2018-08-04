using CarDealer.Data;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarDealer.App
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
            //ImportSuppliers();
            //ImportParts();
            //ImportCars();
            //ImportPartCars();
            //List<Customer> customers = ImportCustomers();
            //ImportSales(customers);

            var ctx = new CarDealerDbContext();

            //Query 1. Ordered Customers
            //GetOrderedCustomers(ctx);

            //Query 2. Cars from make Toyota
            //GetCarsFromToyota(ctx);

            //Query 3. Local Suppliers
            //GetLocalSuppliers(ctx);

            //Query 4. Cars with Their List of Parts
            //GetCarsWithListOfParts(ctx);

            //Query 5. Total Sales by Customer
            //GetTotalSalesByCustomer(ctx);

            //Query 6. Sales with Applied Discount
            //GetSalesWithAppliedDiscount(ctx);

        }

        private static void GetSalesWithAppliedDiscount(CarDealerDbContext ctx)
        {
            var sales = ctx.Sales.Select(s => new
            {
                car =  new
                {
                    s.Car.Make,
                    s.Car.Model,
                    s.Car.TravelledDistance
                },
                customerName = s.Customer.Name,
                s.Discount,
                price = s.Car.PartCars.Sum(p => p.Part.Price),
                priceWithDiscount = (s.Car.PartCars.Sum(p => p.Part.Price)) * (Convert.ToDecimal(1 - s.Discount))
            }).ToArray();

            var jsonSales = JsonConvert.SerializeObject(sales, Formatting.Indented);

            File.WriteAllText("../../../JSON/Outer/sales-discounts.json", jsonSales);
        }

        private static void GetTotalSalesByCustomer(CarDealerDbContext ctx)
        {
            var customers = ctx.Customers.Where(c => c.Sales.Count > 0)
                .Include(c => c.Sales)
                .ThenInclude(s => s.Car)
                .ThenInclude(s => s.PartCars)
                .ThenInclude(p => p.Part)
                .ToArray()
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Select(s => s.Car.PartCars.Select(p => p.Part.Price).Sum() * (decimal)(1 - s.Discount)).Sum()
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            var jsonCustomers = JsonConvert.SerializeObject(customers, Formatting.Indented);

            File.WriteAllText("../../../JSON/Outer/customers-total-sales.json", jsonCustomers);
        }

        private static void GetCarsWithListOfParts(CarDealerDbContext ctx)
        {
            var cars = ctx.Cars.Select(c => new
            {
                car = new
                {
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                },
                parts = c.PartCars.Select(p => new
                {
                    p.Part.Name,
                    p.Part.Price
                }).ToArray()
            })
            .ToArray();

            var jsonCars = JsonConvert.SerializeObject(cars, Formatting.Indented);

            File.WriteAllText("../../../JSON/Outer/cars-and-parts.json", jsonCars);
        }

        private static void GetLocalSuppliers(CarDealerDbContext ctx)
        {
            var suppliers = ctx.Suppliers.Where(s => !s.IsImporter)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                }).ToArray();

            var jsonSuppliers = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            File.WriteAllText("../../../JSON/Outer/local-suppliers.json", jsonSuppliers);
        }

        private static void GetCarsFromToyota(CarDealerDbContext ctx)
        {
            var cars =ctx.Cars.Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                }).ToArray();

            var jsonCars = JsonConvert.SerializeObject(cars, Formatting.Indented);

            File.WriteAllText("../../../JSON/Outer/toyota-cars.json", jsonCars);
        }

        private static void GetOrderedCustomers(CarDealerDbContext ctx)
        {
            var customers = ctx.Customers
                .OrderBy(c => c.BirthDate)
                .OrderBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver,
                    Sales = new List<string>()
                })
                .ToArray();

            var jsonCustomers = JsonConvert.SerializeObject(customers, Formatting.Indented);

            File.WriteAllText("../../../JSON/Outer/ordered-customers.json", jsonCustomers);
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

                if (customers.Single(c => c.Id == customerId).IsYoungDriver)
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

        private static List<Customer> ImportCustomers()
        {
            var jsonString = File.ReadAllText("../../../JSON/Inner/customers.json");

            var deserializedCustomers = JsonConvert.DeserializeObject<Customer[]>(jsonString);

            List<Customer> customers = new List<Customer>();

            foreach (var customer in deserializedCustomers)
            {
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
                var parts = Enumerable.Range(1, 131).OrderBy(x => new Random().Next()).Take(new Random().Next(10, 21)).ToArray();

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

        private static void ImportCars()
        {
            var jsonString = File.ReadAllText("../../../JSON/Inner/cars.json");

            var deserializedCars = JsonConvert.DeserializeObject<Car[]>(jsonString);

            List<Car> cars = new List<Car>();

            foreach (var car in deserializedCars)
            {

                cars.Add(car);
            }

            var ctx = new CarDealerDbContext();
            ctx.Cars.AddRange(cars);
            ctx.SaveChanges();
        }

        private static void ImportParts()
        {
            var jsonString = File.ReadAllText("../../../JSON/Inner/parts.json");

            var deserializedParts = JsonConvert.DeserializeObject<Part[]>(jsonString);

            List<Part> parts = new List<Part>();

            foreach (var part in deserializedParts)
            {
                int supplierId = new Random().Next(1, 32);

                part.SupplierId = supplierId;

                parts.Add(part);
            }

            var ctx = new CarDealerDbContext();
            ctx.Parts.AddRange(parts);
            ctx.SaveChanges();
        }

        private static void ImportSuppliers()
        {
            var jsonString = File.ReadAllText("../../../JSON/Inner/suppliers.json");

            var deserializedSuppliers = JsonConvert.DeserializeObject<Supplier[]>(jsonString);

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var supplier in deserializedSuppliers)
            {
                suppliers.Add(supplier);
            }

            var ctx = new CarDealerDbContext();
            ctx.Suppliers.AddRange(suppliers);
            ctx.SaveChanges();
        }
    }
}
