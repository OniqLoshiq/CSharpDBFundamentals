using System;

namespace My.App.Core.Dtos
{
    public class EmployeePersonalInfoDto
    {
        public int EmployeeId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? Birthday { get; set; }

        public string Address { get; set; }
    }
}
