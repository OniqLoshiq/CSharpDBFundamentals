﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.EntityConfig
{
    public class DiagnostConfig : IEntityTypeConfiguration<Diagnose>
    {
        public void Configure(EntityTypeBuilder<Diagnose> builder)
        {
            builder.HasKey(x => x.DiagnoseId);

            builder.Property(x => x.Name)
                   .HasMaxLength(50)
                   .IsUnicode(true);

            builder.Property(x => x.Comments)
                   .HasMaxLength(250)
                   .IsUnicode(true);
        }
    }
}
