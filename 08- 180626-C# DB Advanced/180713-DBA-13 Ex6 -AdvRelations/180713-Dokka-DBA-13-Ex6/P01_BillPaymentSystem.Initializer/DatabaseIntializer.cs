using P01_BillPaymentSystem.Initializer.Generators;
using P01_BillsPaymentSystem.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace P01_BillPaymentSystem.Initializer
{
    public class DatabaseIntializer
    {
        private static Random rnd = new Random();
        private const int countUsers = 20;
        private const int countCreditCards = 40;
        private const int countBankAccounts = 40;

        public static void InitialSeed(BillsPaymentSystemContext ctx)
        {
            SeedUsers(ctx);
            SeedCreditCards(ctx);
            SeedBankAccounts(ctx);
            SeedPaymentMethods(ctx);
        }

        private static void SeedPaymentMethods(BillsPaymentSystemContext ctx)
        {
            var users = ctx.Users.Select(u => u.UserId).ToArray();
            var randomUsers = Enumerable.Range(0, 19).OrderBy(n => rnd.Next()).Take(10).ToArray();
            int creditCardId = 1;
            int bankAccountId = 1;

            for (int i = 0; i < users.Length; i++)
            {
                var paymentMethod = PaymentMethodGenerator.NewPaymentMethod(ctx, users[i], creditCardId, bankAccountId);

                if (IsValid(paymentMethod))
                {
                    ctx.PaymentMethods.Add(paymentMethod);

                    if (paymentMethod.BankAccountId == null)
                    {
                        creditCardId++;
                    }
                    else
                    {
                        bankAccountId++;
                    }
                }
            }

            for (int i = 0; i < randomUsers.Length; i++)
            {
                var paymentMethod = PaymentMethodGenerator.NewPaymentMethod(ctx, users[randomUsers[i]], creditCardId, bankAccountId);

                if (IsValid(paymentMethod))
                {
                    ctx.PaymentMethods.Add(paymentMethod);

                    if (paymentMethod.BankAccountId == null)
                    {
                        creditCardId++;
                    }
                    else
                    {
                        bankAccountId++;
                    }
                }
            }

            ctx.SaveChanges();
        }

        private static void SeedBankAccounts(BillsPaymentSystemContext ctx)
        {
            for (int i = 0; i < countCreditCards; i++)
            {
                var bankAccount = BankAccountGenerator.NewBankAccount(ctx);

                if (IsValid(bankAccount))
                {
                    ctx.BankAccounts.Add(bankAccount);
                }
            }

            ctx.SaveChanges();
        }

        private static void SeedCreditCards(BillsPaymentSystemContext ctx)
        {
            for (int i = 0; i < countCreditCards; i++)
            {
                var creditCard = CreditCardGenerator.NewCreditCard(ctx);

                if (IsValid(creditCard))
                {
                    ctx.CreditCards.Add(creditCard);
                }
            }

            ctx.SaveChanges();
        }

        private static void SeedUsers(BillsPaymentSystemContext ctx)
        {
            for (int i = 0; i < countUsers; i++)
            {
                var user = UserGenerator.NewUser(ctx);

                if (IsValid(user))
                {
                    ctx.Users.Add(user);
                }
            }

            ctx.SaveChanges();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var result = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, result, true);
        }
    }
}
