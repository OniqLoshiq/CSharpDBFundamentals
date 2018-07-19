using Microsoft.EntityFrameworkCore;
using P01_BillPaymentSystem.Initializer;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Linq;
using System.Text;

namespace P01_BillsPaymentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new BillsPaymentSystemContext())
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

                DatabaseIntializer.InitialSeed(ctx);

                User user = GetUser(ctx);
                Console.WriteLine(user.ToString());

                Console.Write("Enter the bills amount:");
                decimal bills = decimal.Parse(Console.ReadLine());

                PayBills(user, bills);
                ctx.SaveChanges();
            }
        }

        private static void PayBills(User user, decimal bills)
        {
            var bankAccountsTotalBalance = user.PaymentMethods.Where(x => x.BankAccount != null).Sum(x => x.BankAccount.Balance);
            var creditCardsTotalBalance = user.PaymentMethods.Where(x => x.CreditCard != null).Sum(x => x.CreditCard.LimitLeft);

            var totalBalance = bankAccountsTotalBalance + creditCardsTotalBalance;

            if (totalBalance >= bills)
            {
                var bankAccounts = user.PaymentMethods.Where(x => x.BankAccount != null).Select(x => x.BankAccount).OrderBy(x => x.BankAccountId);

                foreach (var ba in bankAccounts)
                {
                    if (ba.Balance >= bills)
                    {
                        ba.Withdraw(bills);
                        PrintNewInfo(user);
                        return;
                    }
                    else
                    {
                        bills -= ba.Balance;
                        ba.Withdraw(ba.Balance);
                    }
                }

                var creditCards = user.PaymentMethods.Where(x => x.CreditCard != null).Select(x => x.CreditCard).OrderBy(x => x.CreditCardId);

                foreach (var cc in creditCards)
                {
                    if (cc.LimitLeft >= bills)
                    {
                        cc.Withdraw(bills);
                        PrintNewInfo(user);

                        return;
                    }
                    else
                    {
                        bills -= cc.LimitLeft;
                        cc.Withdraw(cc.LimitLeft);
                    }
                }
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
            }
        }

        private static void PrintNewInfo(User user)
        {
            Console.WriteLine("The bills have been payed!");
            Console.WriteLine("Th updated user info is:");
            Console.WriteLine(user.ToString());
        }

        private static User GetUser(BillsPaymentSystemContext ctx)
        {
            int userId = int.Parse(Console.ReadLine());

            User user = null;

            while (true)
            {
                user = ctx.Users.Where(x => x.UserId == userId)
                                .Include(x => x.PaymentMethods)
                                .ThenInclude(x => x.BankAccount)
                                .Include(x => x.PaymentMethods)
                                .ThenInclude(x => x.CreditCard)
                                .FirstOrDefault();

                if (user == null)
                {
                    Console.WriteLine($"User with id {userId} not found! Please enter a valid id!");
                    userId = int.Parse(Console.ReadLine());
                    continue;
                }

                break;
            }

            return user;
        }
    }
}
