using System.Linq;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Utilities
{
    public static class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (var ctx = new TeamBuilderContext())
            {
                return ctx.Teams.Any(t => t.Name == teamName);
            }
        }

        public static bool IsUserExisting(string username)
        {
            using (var ctx = new TeamBuilderContext())
            {
                return ctx.Users.Any(u => u.Username == username);
            }
        }

        public static bool IsInviteExisting(string teamName, User user)
        {
            using (var ctx = new TeamBuilderContext())
            {
                return ctx.Invitations.Any(i => i.Team.Name == teamName && i.InvitedUserId == user.Id && i.IsActive);
            }
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            using (var ctx = new TeamBuilderContext())
            {
                return ctx.Teams.Single(t => t.Name == teamName).CreatorId == user.Id;
            }
        }

        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            using (var ctx = new TeamBuilderContext())
            {
                return ctx.Events.Any(e => e.Name == eventName && e.CreatorId == user.Id);
            }
        }

        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (var ctx = new TeamBuilderContext())
            {
                return ctx.Teams.Single(t => t.Name == teamName).Members.Any(m => m.User.Username == username);
            }
        }

        public static bool IsEventExisting(string eventName)
        {
            using (var ctx = new TeamBuilderContext())
            {
                return ctx.Events.Any(e => e.Name == eventName);
            }
        }
    }
}
