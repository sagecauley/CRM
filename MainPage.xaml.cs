namespace CRM
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnLogoutClicked(object? sender, EventArgs e)
        {
            FirebaseAuthService authService = new FirebaseAuthService();
            bool success = await authService.LogoutAsync();

            if (success)
            {
                Application.Current.MainPage = new LoginPage();
            }
            else
            {
                await DisplayAlert("Error", "Logout failed. Please try again.", "OK");
            }
        }
    }
}
