using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilder.Models
{
    public class EventTeam
    {
        [ForeignKey("Event")]
        [Range(0, int.MaxValue)]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        [ForeignKey("Team")]
        [Range(0, int.MaxValue)]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
