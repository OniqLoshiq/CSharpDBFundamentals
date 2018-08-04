using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ProductShop.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //01-SetupDatabase
            //using(var ctx = new ProductShopContext())
            //{
            //    ctx.Database.Migrate();
            //}
           
            //02-ImportData
            //ImportUsers();
            //ImportProducts();
            //ImportCategories();
            //ImportCategoryProducts();

            var ctx = new ProductShopContext();

            //Query 1. Products In Range
            //GetProductsInRange(ctx);

            //Query 2. Sold Products
            //GetSoldProducts(ctx);

            //Query 3. Categories By Products Count
            //GetCategoriesByProductsCount(ctx);

            //Query 4. Users and Products
            //GetUsersAndProducts(ctx);
        }

        private static void GetUsersAndProducts(ProductShopContext ctx)
        {
            var users = new
            {
                usersCount = ctx.Users.Where(u => u.ProductsSold.Count > 0 && u.ProductsSold.Any(p => p.Buyer != null)).Count(),
                users = ctx.Users.Where(u => u.ProductsSold.Count > 0 && u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count)
                .ThenBy(u => u.LastName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count,
                        products = u.ProductsSold.Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        }).ToArray()
                    }
                }).ToArray()
            };

            var jsonUsers = JsonConvert.SerializeObject(users, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllText("../../../JSON/Outer/users-and-products.json", jsonUsers);
        }

        private static void GetCategoriesByProductsCount(ProductShopContext ctx)
        {
            var categories = ctx.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Sum(p => p.Product.Price) / c.CategoryProducts.Count,
                    totalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(c => c.productsCount)
                .ToArray();

            var jsonCategories = JsonConvert.SerializeObject(categories, Formatting.Indented);

            File.WriteAllText("../../../JSON/Outer/categories-by-products.json", jsonCategories);
        }

        private static void GetSoldProducts(ProductShopContext ctx)
        {
            var users = ctx.Users.Where(u => u.ProductsSold.Count >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold.Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    }).ToArray()
                })
                .ToArray();

            var jsonUsers = JsonConvert.SerializeObject(users, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllText("../../../JSON/Outer/users-sold-products.json", jsonUsers);
        }

        private static void GetProductsInRange(ProductShopContext ctx)
        {
            var products = ctx.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName ?? x.Seller.LastName
                })
                .ToArray();

            var jsonProducts = JsonConvert.SerializeObject(products, Formatting.Indented);

            File.WriteAllText("../../../JSON/Outer/products-in-range.json", jsonProducts);
        }

        private static void ImportCategoryProducts()
        {
            List<CategoryProduct> cps = new List<CategoryProduct>();

            for (int i = 1; i <= 200; i++)
            {
                var categoryId = new Random().Next(1, 12);

                var categoryProduct = new CategoryProduct()
                {
                    ProductId = i,
                    CategoryId = categoryId
                };

                cps.Add(categoryProduct);
            }

            var ctx = new ProductShopContext();
            ctx.CategoryProducts.AddRange(cps);
            ctx.SaveChanges();
        }

            private static void ImportCategories()
        {
            var jsonString = File.ReadAllText("../../../JSON/Inner/categories.json");

            var deserializedCategories = JsonConvert.DeserializeObject<Category[]>(jsonString);

            List<Category> categories = new List<Category>();

            foreach (var category in deserializedCategories)
            {
                if (!IsValid(category))
                {
                    continue;
                }

                categories.Add(category);
            }

            var ctx = new ProductShopContext();
            ctx.Categories.AddRange(categories);
            ctx.SaveChanges();
        }

        private static void ImportProducts()
        {
            var jsonString = File.ReadAllText("../../../JSON/Inner/products.json");

            var deserializedProducts = JsonConvert.DeserializeObject<Product[]>(jsonString);

            List<Product> products = new List<Product>();

            int counter = 1;

            foreach (var product in deserializedProducts)
            {
                if (!IsValid(product))
                {
                    continue;
                }

                var buyerId = new Random().Next(1, 30);
                var sellerId = new Random().Next(30, 57);

                product.BuyerId = buyerId;
                product.SellerId = sellerId;

                if (counter == 4)
                {
                    product.BuyerId = null;
                    counter = 0;
                }

                counter++;

                products.Add(product);
            }

            var ctx = new ProductShopContext();
            ctx.Products.AddRange(products);
            ctx.SaveChanges();
        }

        private static void ImportUsers()
        {
            var jsonString = File.ReadAllText("../../../JSON/Inner/users.json");

            var deserializedUsers = JsonConvert.DeserializeObject<User[]>(jsonString);

            List<User> users = new List<User>();

            foreach (var user in deserializedUsers)
            {
                if (IsValid(user))
                {
                    users.Add(user);
                }
            }

            var ctx = new ProductShopContext();
            ctx.Users.AddRange(users);
            ctx.SaveChanges();
        }
        
        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
