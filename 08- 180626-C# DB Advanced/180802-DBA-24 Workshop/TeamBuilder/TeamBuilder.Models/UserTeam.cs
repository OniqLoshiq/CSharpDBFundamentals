using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilder.Models
{
    public class UserTeam
    {
        [ForeignKey("User")]
        [Range(0, int.MaxValue)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Team")]
        [Range(0, int.MaxValue)]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
