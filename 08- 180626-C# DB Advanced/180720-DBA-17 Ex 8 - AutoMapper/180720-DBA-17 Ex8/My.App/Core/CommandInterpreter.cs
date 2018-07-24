using My.App.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace My.App.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private readonly IServiceProvider sp;

        public CommandInterpreter(IServiceProvider sp)
        {
            this.sp = sp;
        }

        public IExecutable ProcessCommand(string cmdName)
        {
            Type cmdType = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(c => c.Name.ToLower() == cmdName.ToLower() + "command");

            if(cmdType == null)
            {
                throw new ArgumentException("Invalid command!");
            }
            if(!typeof(IExecutable).IsAssignableFrom(cmdType))
            {
                throw new ArgumentException($"{cmdName} is not a Command!");
            }

            var ctor = cmdType.GetConstructors().First();
            var ctrParams = ctor.GetParameters().Select(p => p.ParameterType).ToArray();

            var service = ctrParams.Select(sp.GetService).ToArray();

            var cmd = (IExecutable)ctor.Invoke(service);

            return cmd;
        }
    }
}
