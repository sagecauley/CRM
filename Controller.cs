using CRM.Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
    public class Controller
    {
        FirebaseAuthService auth;
        public Controller(FirebaseAuthService authService)
        {
            auth = authService;
        }

        public async Task<bool> OnLogin(string username, string password)
        {
            return await auth.LoginAsync(username, password);
        }

        public async Task<bool> OnCreateAccount(string username, string password)
        {
            return await auth.SignUpAsync(username, password);
        }

        public async Task<bool> OnLogout()
        {
            return await auth.LogoutAsync();
        }
    }
}
