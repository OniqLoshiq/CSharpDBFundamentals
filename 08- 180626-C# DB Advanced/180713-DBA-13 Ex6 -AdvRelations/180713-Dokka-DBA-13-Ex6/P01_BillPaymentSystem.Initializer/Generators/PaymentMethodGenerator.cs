using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using P01_BillsPaymentSystem.Data.Models.Enums;
using System;

namespace P01_BillPaymentSystem.Initializer.Generators
{
    public class PaymentMethodGenerator
    {
        private static Random rnd = new Random();

        public static PaymentMethod NewPaymentMethod(BillsPaymentSystemContext ctx, int userId, int nextCreditCardId, int nextBankAccountId)
        {
            int typeEnum = rnd.Next(0, 2);

            var paymentMethod = new PaymentMethod();

            paymentMethod.Type = (PaymentType)typeEnum;
            paymentMethod.UserId = userId;
            
            if (typeEnum == 0)
            {
                paymentMethod.BankAccountId = nextBankAccountId;
            }
            else
            {
                paymentMethod.CreditCardId = nextCreditCardId;
            }

            return paymentMethod;
        }
    }
}
