namespace CRM
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly LoginPage _loginPage;
        private Controller controller;
        public MainPage(LoginPage loginPage, Controller c)
        {
            InitializeComponent();
            _loginPage = loginPage;
            controller = c;
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
            bool success = await controller.OnLogout();

            if (success)
            {
                Application.Current.MainPage = _loginPage;
            }
            else
            {
                await DisplayAlert("Error", "Logout failed. Please try again.", "OK");
            }
        }
    }
}
