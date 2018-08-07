using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamBuilder.Models;

namespace TeamBuilder.Data.Configuration
{
    public class EventTeamConfig : IEntityTypeConfiguration<EventTeam>
    {
        public void Configure(EntityTypeBuilder<EventTeam> builder)
        {
            builder.HasKey(et => new { et.EventId, et.TeamId });

            builder.HasOne(e => e.Event)
                .WithMany(e => e.ParticipatingEventTeams);

            builder.HasOne(t => t.Team)
                .WithMany(t => t.EventTeams);
        }
    }
}
