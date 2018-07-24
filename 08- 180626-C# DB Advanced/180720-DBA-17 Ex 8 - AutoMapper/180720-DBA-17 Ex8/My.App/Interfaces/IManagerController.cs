using My.App.Core.Dtos;

namespace My.App.Interfaces
{
    public interface IManagerController
    {
        void SetManager(int employeeId, int managerId);

        ManagerDto GetManagerInfo(int managerId);
    }
}
