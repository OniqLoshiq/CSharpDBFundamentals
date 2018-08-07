using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamBuilder.Models;

namespace TeamBuilder.Data.Configuration
{
    public class UserTeamConfig : IEntityTypeConfiguration<UserTeam>
    {
        public void Configure(EntityTypeBuilder<UserTeam> builder)
        {
            builder.HasKey(ut => new { ut.TeamId, ut.UserId });

            builder.HasOne(ut => ut.Team)
                .WithMany(t => t.Members);

            builder.HasOne(ut => ut.User)
                .WithMany(u => u.MemberOf);
        }
    }
}
