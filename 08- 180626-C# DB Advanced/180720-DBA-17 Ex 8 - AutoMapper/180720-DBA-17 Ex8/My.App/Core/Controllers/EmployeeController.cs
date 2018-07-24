using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using My.App.Core.Dtos;
using My.App.Interfaces;
using My.Data;
using My.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace My.App.Core.Controllers
{
    public class EmployeeController : IEmployeeController
    {
        private readonly MyDbContext db;
        private readonly IMapper mapper;

        public EmployeeController(MyDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public void AddEmployee(EmployeeDto employeeDto)
        {
            var employee = mapper.Map<Employee>(employeeDto);
            this.db.Employees.Add(employee);

            this.db.SaveChanges();
        }

        public EmployeeDto GetEmployeeInfo(int employeeId)
        {
            var employee = this.db.Employees.Where(e => e.EmployeeId == employeeId)
                .Select(this.mapper.Map<EmployeeDto>)
                .SingleOrDefault();

            CheckEmployee(employee);

            return employee;
        }

        public EmployeePersonalInfoDto GetEmployeePersonalInfo(int employeeId)
        {
           //var employee = this.db.Employees.Where(e => e.EmployeeId == employeeId)
           //   .Select(this.mapper.Map<EmployeePersonalInfoDto>)
           //   .SingleOrDefault();

            var employee = this.db.Employees.Where(e => e.EmployeeId == employeeId)
              .ProjectTo<EmployeePersonalInfoDto>(this.mapper.ConfigurationProvider)
              .SingleOrDefault();

            CheckEmployee(employee);

            return employee;
        }

        public List<EmployeeOlderThanDto> GetEmployeesOlderThan(int age)
        {
            //var employees = this.db.Employees.Include(e => e.Manager).Select(this.mapper.Map<EmployeeOlderThanDto>)
            //                                                         .Where(e => e.Age > age).OrderByDescending(e => e.Salary).ToList();
            
            if (age <= 0)
            {
                throw new ArgumentException("Value must be bigger than 0!");
            }

            var employees = this.db.Employees.Include(e => e.Manager).ProjectTo<EmployeeOlderThanDto>(this.mapper.ConfigurationProvider)
                                             .Where(e => e.Age > age)
                                             .OrderByDescending(e => e.Salary).ToList();

            return employees;
        }

        public void SetAddress(int employeeId, string address)
        {
            var employee = this.db.Employees.Find(employeeId);

            CheckEmployee(employee);

            employee.Address = address;

            this.db.SaveChanges();
        }

        public void SetBirthday(int employeeId, DateTime date)
        {
            var employee = this.db.Employees.Find(employeeId);

            CheckEmployee(employee);

            employee.Birthday = date;

            this.db.SaveChanges();
        }

        private void CheckEmployee(object employee)
        {
            if (employee == null)
            {
                throw new ArgumentException("Invalid employee id!");
            }
        }
    }
}
