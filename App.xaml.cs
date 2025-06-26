using CRM.Models;

namespace CRM
{
    public partial class App : Application
    {
        public App(LoginPage loginPage, CustomersModel cm, Controller c)
        {
            InitializeComponent();
            MainPage = loginPage;
            CheckLoginStatus(cm, c);
            
        }

        private async void CheckLoginStatus(CustomersModel cm, Controller c)
        {
            var authService = new Firebase.FirebaseAuthService();
            var token = await authService.GetValidIdTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                bool success = await c.Initalize(); // Initialize the controller with the auth service and model
                MainPage = new AppShell(); // Shell starts and navigates to MainPage

            }
        }
    }
}