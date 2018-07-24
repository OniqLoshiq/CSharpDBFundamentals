using My.App.Interfaces;
using System;

namespace My.App.Commands
{
    public class ExitCommand : IExecutable
    {
        public string Execute(string[] args)
        {
            Environment.Exit(0);

            return null;
        }
    }
}
