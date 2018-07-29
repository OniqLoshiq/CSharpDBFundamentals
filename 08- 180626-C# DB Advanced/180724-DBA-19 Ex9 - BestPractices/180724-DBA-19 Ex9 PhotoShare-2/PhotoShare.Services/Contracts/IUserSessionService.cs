using PhotoShare.Models;

namespace PhotoShare.Services.Contracts
{
    public interface IUserSessionService
    {
        User User { get; }
        bool LoggedIn { get; }

        void Login(string username, string password);
        void Logout();
    }
}
