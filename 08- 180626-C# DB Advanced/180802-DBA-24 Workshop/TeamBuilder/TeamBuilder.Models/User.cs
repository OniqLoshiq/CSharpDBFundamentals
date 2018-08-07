using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeamBuilder.Models.Enums;

namespace TeamBuilder.Models
{
    public class User
    {
        public User()
        {
            this.CreatedEvents = new HashSet<Event>();
            this.CreatedUserTeams = new HashSet<Team>();
            this.ReceivedInvitations = new HashSet<Invitation>();
            this.MemberOf = new HashSet<UserTeam>();
        }

        [Key]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Username { get; set; }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        [StringLength(30, MinimumLength = 6)]
        [Required]
        public string Password { get; set; }

        public Gender Gender { get; set; }

        [Range(0, int.MaxValue)]
        public int Age { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Team> CreatedUserTeams { get; set; }
        public virtual ICollection<Event> CreatedEvents { get; set; }
        public virtual ICollection<Invitation> ReceivedInvitations { get; set; }
        public virtual ICollection<UserTeam> MemberOf { get; set; }
    }
}
