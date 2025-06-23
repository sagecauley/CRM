namespace CRM
{
    public partial class App : Application
    {
        public App(LoginPage loginPage)
        {
            InitializeComponent();
            MainPage = loginPage;
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            var authService = new Firebase.FirebaseAuthService();
            var token = await authService.GetValidIdTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                MainPage = new AppShell(); // Shell starts and navigates to MainPage
            }
        }
    }
}