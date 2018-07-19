using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;
using P01_BillsPaymentSystem.Data.Models.Enums;
using System;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    public class PaymentMethodConfig : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.Property(x => x.Type)
                   .HasConversion(v => v.ToString(),
                                  v => (PaymentType)Enum.Parse(typeof(PaymentType), v));
        }
    }
}
