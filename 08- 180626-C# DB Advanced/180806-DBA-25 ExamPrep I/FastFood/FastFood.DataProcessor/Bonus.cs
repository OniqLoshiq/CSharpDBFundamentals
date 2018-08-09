using System;
using System.Linq;
using FastFood.Data;

namespace FastFood.DataProcessor
{
    public static class Bonus
    {
	    public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
	    {
            var item = context.Items.SingleOrDefault(i => i.Name == itemName);

            if(item == null)
            {
                return $"Item {itemName} not found!";
            }

            decimal oldPrice = item.Price;

            item.Price = newPrice;
            context.SaveChanges();

            return $"{itemName} Price updated from ${oldPrice:f2} to ${newPrice:f2}";
	    }
    }
}
