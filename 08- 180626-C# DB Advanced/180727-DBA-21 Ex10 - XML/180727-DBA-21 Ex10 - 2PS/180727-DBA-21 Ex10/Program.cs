using _180727_DBA_21_Ex10.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using DA = System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml;

namespace _180727_DBA_21_Ex10
{
    class Program
    {
        static void Main(string[] args)
        {
            //01-SetupDatabase
            //using(var ctx = new ProductShopDbContext())
            //{
            //    ctx.Database.Migrate();
            //}

            //02-ImportData
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            var mapper = config.CreateMapper();

            //ImportUsers(mapper);
            //ImportProducts(mapper);
            //ImportCategories(mapper);
            //ImportCategoryProducts();

            var ctx = new ProductShopDbContext();

            //Query 1. Products In Range
            //GetProductsInRange(ctx);

            //Query 2. Sold Products
            //GetSoldProducts(ctx);

            //Query 3. Categories By Products Count
            //GetCategoriesByProductsCount(ctx);

            //Query 4. Users and Products
            //GetUsersAndProducts(ctx);
        }

        private static void GetUsersAndProducts(ProductShopDbContext ctx)
        {
            var users = new Q4_UsersDto()
            {
                UsersCount = ctx.Users.Where(u => u.ProductsSold.Count >= 1).Count(),
                Users = ctx.Users.Where(u => u.ProductsSold.Count >= 1).Select(u => new Q4_UserDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age.ToString(),
                    SoldProducts = new Q4_SoldProductDto()
                    {
                        ProductsCount = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new Q4_ProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price
                        }).ToArray()   
                    }
                })
                .OrderByDescending(u => u.SoldProducts.ProductsCount)
                .ThenBy(u => u.LastName).ToArray()
            };

            var serializer = new XmlSerializer(typeof(Q4_UsersDto), new XmlRootAttribute("users"));

            using (var writer = new StreamWriter("../../../XML/Outer/users-and-products.xml"))
            {
                serializer.Serialize(writer, users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetCategoriesByProductsCount(ProductShopDbContext ctx)
        {
            var categories = ctx.Categories
                             .Select(c => new Q3_CategoriesDto
                             {
                                 Name = c.Name,
                                 ProductsCount = c.CategoryProducts.Count,
                                 AveragePrice = c.CategoryProducts.Select(p => p.Product.Price).DefaultIfEmpty(0).Average(),
                                 TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                             })
                             .OrderByDescending(c => c.ProductsCount)
                             .ToArray();

            var serializer = new XmlSerializer(typeof(Q3_CategoriesDto[]), new XmlRootAttribute("categories"));

            using (var writer = new StreamWriter("../../../XML/Outer/categories-by-products.xml"))
            {
                serializer.Serialize(writer, categories, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetSoldProducts(ProductShopDbContext ctx)
        {
            var users = ctx.Users
                              .Where(u => u.ProductsSold.Count >= 1)
                              .Select(u => new Q2_UserDto
                              {
                                  FirstName = u.FirstName,
                                  LastName = u.LastName,
                                  SoldProducts = u.ProductsSold.Select(p => new Q2_SoldProductDto()
                                  {
                                      Name = p.Name,
                                      Price = p.Price
                                  }).ToArray()
                              })
                              .OrderBy(u => u.LastName)
                              .ThenBy(u => u.FirstName)
                              .ToArray();  
            
            var serializer = new XmlSerializer(typeof(Q2_UserDto[]), new XmlRootAttribute("users"));

            using (var writer = new StreamWriter("../../../XML/Outer/users-sold-products.xml"))
            {
                serializer.Serialize(writer, users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        private static void GetProductsInRange(ProductShopDbContext ctx)
        {
            var products = ctx.Products
                              .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.BuyerId != null)
                              .Select(p => new Q1_ProductDto()
                              {
                                  Name = p.Name,
                                  Price = p.Price,
                                  Buyer = p.Buyer.FirstName == null ? p.Buyer.LastName : p.Buyer.FirstName + " " + p.Buyer.LastName
                              }).ToArray();


            var serializer = new XmlSerializer(typeof(Q1_ProductDto[]), new XmlRootAttribute("products"));

            using (var writer = new StreamWriter("../../../XML/Outer/products-in-range.xml"))
            {
                serializer.Serialize(writer, products, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
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

            var ctx = new ProductShopDbContext();
            ctx.CategoryProducts.AddRange(cps);
            ctx.SaveChanges();
        }

        private static void ImportCategories(IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XML/Inner/categories.xml");

            var serializer = new XmlSerializer(typeof(CategoryDto_02[]), new XmlRootAttribute("categories"));
            var deserializedCategories = (CategoryDto_02[])serializer.Deserialize(new StringReader(xmlString));

            List<Category> categories = new List<Category>();

            foreach (var categoryDto in deserializedCategories)
            {
                if (!IsValid(categoryDto))
                {
                    continue;
                }

                var category = mapper.Map<Category>(categoryDto);

                categories.Add(category);
            }

            var ctx = new ProductShopDbContext();
            ctx.Categories.AddRange(categories);
            ctx.SaveChanges();
        }

        private static void ImportProducts(IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XML/Inner/products.xml");

            var serializer = new XmlSerializer(typeof(ProductDto_02[]), new XmlRootAttribute("products"));
            var deserializedProducts = (ProductDto_02[])serializer.Deserialize(new StringReader(xmlString));

            List<Product> products = new List<Product>();

            int counter = 1;

            foreach (var productDto in deserializedProducts)
            {
                if (!IsValid(productDto))
                {
                    continue;
                }

                var product = mapper.Map<Product>(productDto);
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

            var ctx = new ProductShopDbContext();
            ctx.Products.AddRange(products);
            ctx.SaveChanges();
        }

        private static void ImportUsers(IMapper mapper)
        {
            var xmlString = File.ReadAllText("../../../XML/Inner/users.xml");

            var serializer = new XmlSerializer(typeof(UserDto_02[]), new XmlRootAttribute("users"));
            var deserializedUsers = (UserDto_02[])serializer.Deserialize(new StringReader(xmlString));

            List<User> users = new List<User>();

            foreach (var userDto in deserializedUsers)
            {
                if (!IsValid(userDto))
                {
                    continue;
                }

                var user = mapper.Map<User>(userDto);

                users.Add(user);
            }

            var ctx = new ProductShopDbContext();
            ctx.Users.AddRange(users);
            ctx.SaveChanges();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new DA.ValidationContext(obj);
            var validationResults = new List<DA.ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
