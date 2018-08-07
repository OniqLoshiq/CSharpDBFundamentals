using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamBuilder.Models
{
    public class Team
    {
        public Team()
        {
            this.Invitations = new HashSet<Invitation>();
            this.Members = new HashSet<UserTeam>();
            this.EventTeams = new HashSet<EventTeam>();
        }

        [Key]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [MaxLength(25)]
        [Required]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Description { get; set; }

        [StringLength(3,MinimumLength = 3)]
        [Required]
        public string Acronym { get; set; }

        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<UserTeam> Members { get; set; }
        public virtual ICollection<EventTeam> EventTeams { get; set; }

    }
}
