namespace CRM;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private async void OnLoginClicked(object sender, EventArgs e)
	{
		errorLabel.IsVisible = false;
        if (ValidateCredentials(usernameEntry.Text, passwordEntry.Text))
        {
			errorLabel.IsVisible = true;
            errorLabel.Text = "Username or Password cannot be empty.";
        }
		else
		{
            string username = usernameEntry.Text.Trim();
            string password = passwordEntry.Text.Trim();

            var authService = new FirebaseAuthService();

            bool success = await authService.LoginAsync(username, password);

            if (success)
            {
                Application.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                errorLabel.IsVisible = true;
                errorLabel.Text = "Login failed. Please check your credentials.";
            }


        }


	}
    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        errorLabel.IsVisible = false;
        if (ValidateCredentials(usernameEntry.Text, passwordEntry.Text))
        {
            errorLabel.IsVisible = true;
            errorLabel.Text = "Username or Password cannot be empty.";
        }
        else
        {
            string username = usernameEntry.Text.Trim();
            string password = passwordEntry.Text.Trim();

            var authService = new FirebaseAuthService();
            bool created = await authService.SignUpAsync(username, password);

            if (created)
            {
                errorLabel.TextColor = Colors.Green;
                errorLabel.Text = "Account created successfully. Please log in.";
                errorLabel.IsVisible = true;
            }
            else
            {
                errorLabel.TextColor = Colors.Red;
                errorLabel.Text = "Account creation failed. Try another email.";
                errorLabel.IsVisible = true;
            }
        }


    }

    private bool ValidateCredentials(string username, string password)
	{
		return string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password);
    }
}