using My.App.Interfaces;
using My.Services.Interfaces;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace My.App.Core
{
    public class Enigne : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Enigne(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            var dbInitializerService = this.serviceProvider.GetService<IDbInitializerService>();
            dbInitializerService.InitializeDatebase();

            var commandInterpreter = this.serviceProvider.GetService<ICommandInterpreter>();
            
            while (true)
            {
                try
                {
                    string[] data = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
                    string cmdName = data[0];
                    string[] args = data.Skip(1).ToArray();
                    IExecutable cmd = commandInterpreter.ProcessCommand(cmdName);

                    MethodInfo method = typeof(IExecutable).GetMethod("Execute");

                    try
                    {
                        string result = (string)method.Invoke(cmd, new[] { args });

                        Console.WriteLine(result);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
