namespace CRM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            var authService = new FirebaseAuthService();
            var token = await authService.GetValidIdTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                MainPage = new AppShell(); // Shell starts and navigates to MainPage
            }
            else
            {
                MainPage = new LoginPage(); // Standalone login page
            }
        }
    }
}