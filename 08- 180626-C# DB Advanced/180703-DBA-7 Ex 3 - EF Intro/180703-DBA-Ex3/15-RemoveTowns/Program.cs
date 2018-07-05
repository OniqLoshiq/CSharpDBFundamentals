using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace _15_RemoveTowns
{
    class Program
    {
        static void Main(string[] args)
        {
            string townToDelete = Console.ReadLine();
            int removedAddresses = 0;

            using (var ctx = new SoftUniContext())
            {
                var addresses = ctx.Addresses.Where(a => a.Town.Name == townToDelete);
                removedAddresses = addresses.Count();

                var addressesIds = addresses.Select(a => a.AddressId);
                var employees = ctx.Employees.Where(e => addressesIds.Any(a => a == e.AddressId)).ToList();
                employees.ForEach(e => e.AddressId = null);

                ctx.Addresses.RemoveRange(addresses);
                ctx.Towns.Remove(ctx.Towns.SingleOrDefault(t => t.Name == townToDelete));
                ctx.SaveChanges();
            }

            Console.WriteLine($"{removedAddresses} addresses in {townToDelete} were deleted");
        }
    }
}
