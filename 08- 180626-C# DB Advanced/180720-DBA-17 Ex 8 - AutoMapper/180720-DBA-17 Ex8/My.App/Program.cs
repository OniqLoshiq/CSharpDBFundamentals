using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using My.App.Core;
using My.App.Core.Controllers;
using My.App.Interfaces;
using My.Data;
using My.Services;
using My.Services.Interfaces;
using System;
using AutoMapper;
using My.App.Core.MyProfile;

namespace My.App
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider sp = ConfigureServices();

            IEngine engine = new Enigne(sp);

            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddAutoMapper(cfg => cfg.AddProfile<MyProfile>());

            services.AddDbContext<MyDbContext>(opt => opt.UseSqlServer(Config.ConfigurationString));
            services.AddTransient<IDbInitializerService, DbInitializerService>();
            services.AddTransient<ICommandInterpreter, CommandInterpreter>();
            services.AddTransient<IEmployeeController, EmployeeController>();
            services.AddTransient<IManagerController, ManagerController>();

            IServiceProvider sp = services.BuildServiceProvider();

            return sp;
        }
    }
}
