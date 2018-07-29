namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] data)
        {
            string cmdName = this.GetType().Name;
            int cmdIndex = cmdName.IndexOf("Command");
            cmdName = cmdName.Substring(0, cmdIndex);

            if (data.Length != 0)
            {
                throw new InvalidOperationException($"Command {cmdName} not valid!");
            }

            Console.WriteLine("Good Bye!");
            Environment.Exit(0);
            return null;
        }
    }
}
