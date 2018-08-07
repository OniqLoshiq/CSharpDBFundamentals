using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TeamBuilder.Models;

namespace TeamBuilder.Data.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Username).IsUnique();

            builder.HasMany(u => u.CreatedUserTeams)
                   .WithOne(t => t.Creator)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.CreatedEvents)
                   .WithOne(e => e.Creator)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ReceivedInvitations)
                .WithOne(i => i.InvitedUser)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
