using My.App.Core.Dtos;
using System;
using System.Collections.Generic;

namespace My.App.Interfaces
{
    public interface IEmployeeController
    {
        void AddEmployee(EmployeeDto employeeDto);

        void SetBirthday(int employeeId, DateTime date);

        void SetAddress(int employeeId, string address);

        EmployeeDto GetEmployeeInfo(int employeeId);

        EmployeePersonalInfoDto GetEmployeePersonalInfo(int employeeId);

        List<EmployeeOlderThanDto> GetEmployeesOlderThan(int age);
    }
}
