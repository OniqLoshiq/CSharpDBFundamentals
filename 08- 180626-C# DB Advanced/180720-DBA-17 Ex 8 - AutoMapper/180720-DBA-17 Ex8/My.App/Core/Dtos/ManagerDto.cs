using System.Collections.Generic;

namespace My.App.Core.Dtos
{
    public class ManagerDto
    {
        public ManagerDto()
        {
            this.SubordinateDtos = new HashSet<EmployeeDto>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<EmployeeDto> SubordinateDtos { get; set; }

        public int SubordinatesCount => this.SubordinateDtos.Count;
    }
}
