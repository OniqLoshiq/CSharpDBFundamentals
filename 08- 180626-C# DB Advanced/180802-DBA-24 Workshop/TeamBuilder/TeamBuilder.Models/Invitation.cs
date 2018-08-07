using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilder.Models
{
    public class Invitation
    {
        [Key]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [ForeignKey("InvitedUser")]
        public int InvitedUserId { get; set; }
        public virtual User InvitedUser { get; set; }
        
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
