using System;
using TeamBuilder.App.Utilities;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core
{
    public static class AuthenticationManager
    {
        private static User currentUser;
        

        public static void Login(User user)
        {
            if (currentUser != null)
                throw new InvalidCastException(Constants.ErrorMessages.LogoutFirst);

            currentUser = user;
        }

        public static void Logout()
        {
            currentUser = null;
        }

        public static void Authorize()
        {
            if (currentUser == null)
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
        }

        public static bool IsAuthenticated()
        {
            return currentUser == null ? false : true;
        }

        public static User GetGurrentUser()
        {
            Authorize();

            return currentUser;
        }
    }
}
