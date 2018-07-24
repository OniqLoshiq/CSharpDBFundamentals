using System;

namespace My.App.Core.Dtos
{
    public class EmployeeOlderThanDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? Birthday { get; set; }

        public string ManagerFullName { get; set; }

        public int Age => this.Birthday == null ? -1 : new DateTime(DateTime.Now.Subtract(this.Birthday.Value).Ticks).Year - 1;
    }
}
