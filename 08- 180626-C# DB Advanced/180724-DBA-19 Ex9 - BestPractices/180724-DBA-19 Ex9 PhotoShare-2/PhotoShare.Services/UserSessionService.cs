using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;
using System.Linq;

namespace PhotoShare.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IUserService userService;
        private bool hasLoggedInUser = false;
        public User User { get; private set; }

        public UserSessionService(IUserService userService)
        {
            this.userService = userService;
            this.User = null;
        }

        public bool LoggedIn => this.hasLoggedInUser;

        public void Login(string username, string password)
        {
            this.User = this.userService.ByUsernameAndPassword(username, password);
            if (this.User != null)
            {
                this.hasLoggedInUser = true;
            }
        }

        public void Logout()
        {
            this.User = null;
            this.hasLoggedInUser = false;
        }
    }
}
