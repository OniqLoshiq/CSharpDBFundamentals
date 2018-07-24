using AutoMapper;
using My.App.Core.Dtos;
using My.App.Interfaces;
using My.Data;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace My.App.Core.Controllers
{
    public class ManagerController : IManagerController
    {
        private readonly MyDbContext db;
        private readonly IMapper mapper;

        public ManagerController(MyDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        
        public ManagerDto GetManagerInfo(int managerId)
        {
            var manager = this.db.Employees.Include(e => e.Subordinates)
                .Where(e => e.EmployeeId == managerId)
                .Select(this.mapper.Map<ManagerDto>)
                .SingleOrDefault();

            CheckEmployee(manager);

            return manager;
        }

        public void SetManager(int employeeId, int managerId)
        {
            var employee = this.db.Employees.Find(employeeId);
            CheckEmployee(employee);

            var manager = this.db.Employees.Find(managerId);
            CheckEmployee(manager);

            employee.Manager = manager;

            db.SaveChanges();
        }

        private void CheckEmployee(object employee)
        {
            if (employee == null)
            {
                throw new ArgumentException("Invalid id!");
            }
        }
    }
}
