using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;

namespace P01_StudentSystem.Data.EntityConfig
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(x => x.Name)
                  .HasMaxLength(80)
                  .IsUnicode(true)
                  .IsRequired();

            builder.Property(x => x.Description)
                 .IsUnicode(true)
                 .IsRequired(false);

            builder.HasMany(x => x.StudentsEnrolled)
                   .WithOne(x => x.Course)
                   .HasForeignKey(x => x.CourseId);

            builder.HasMany(x => x.HomeworkSubmissions)
                   .WithOne(x => x.Course)
                   .HasForeignKey(x => x.CourseId);

            builder.HasMany(x => x.Resources)
                   .WithOne(x => x.Course)
                   .HasForeignKey(x => x.CourseId);
        }
    }
}
