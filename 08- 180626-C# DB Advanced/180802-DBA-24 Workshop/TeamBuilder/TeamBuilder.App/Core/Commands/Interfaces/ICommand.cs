using System;
using System.Collections.Generic;
using System.Text;

namespace TeamBuilder.App.Core.Commands.Interfaces
{
    public interface ICommand
    {
        string Execute(string[] args);
    }
}
