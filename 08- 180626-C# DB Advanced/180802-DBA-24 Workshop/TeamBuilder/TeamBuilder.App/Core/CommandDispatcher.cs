using System;
using System.Linq;
using TeamBuilder.App.Core.Commands;

namespace TeamBuilder.App.Core
{
    public class CommandDispatcher
    {
        public string Dispatch(string input)
        {
            string result = String.Empty;

            string[] args = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            string cmdName = args.Length >= 0 ? args[0] : string.Empty;
            args = args.Skip(1).ToArray();

            switch (cmdName)
            {
                case "RegisterUser": result = new RegisterUserCommand().Execute(args); break;
                case "Login": result = new LoginCommand().Execute(args); break;
                case "Logout": result = new LogoutCommand().Execute(args); break;
                case "DeleteUser": result = new DeleteUserCommand().Execute(args); break;
                case "CreateEvent": result = new CreateEventCommand().Execute(args); break;
                case "CreateTeam": result = new CreateTeamCommand().Execute(args); break;
                case "InviteToTeam": result = new InviteToTeamCommand().Execute(args); break;
                case "AcceptInvite": result = new AcceptInviteCommand().Execute(args); break;
                case "DeclineInvite": result = new DeclineInviteCommand().Execute(args); break;
                case "KickMember": result = new KickMemberCommand().Execute(args); break;
                case "Disband": result = new DisbandCommand().Execute(args); break;
                case "AddTeamTo": result = new AddTeamToCommand().Execute(args); break;
                case "ShowEvent": result = new ShowEventCommand().Execute(args); break;
                case "ShowTeam": result = new ShowTeamCommand().Execute(args); break;
                case "Exit": result = new ExitCommand().Execute(args); break;
                default:
                    throw new NotSupportedException($"Command {cmdName} not supported!");
            }

            return result;
        }
    }
}
