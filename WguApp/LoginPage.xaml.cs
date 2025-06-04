using System.Threading.Tasks;

namespace WguApp;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "WguApp.db");

        if (File.Exists(dbPath))
            File.Delete(dbPath);
    }

    

    private void registerUser_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Register());

    }

    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(usernameEntry.Text) || string.IsNullOrWhiteSpace(passwordEntry.Text))
        {
            DisplayAlert("Error", "Please enter both username and password.", "OK");
            return;
        }

        SqlData.SqlDatahelper sqlData = new SqlData.SqlDatahelper();
        WguClasses.Login user = await sqlData.AttemptLogin(usernameEntry.Text, passwordEntry.Text);
        if (user != null) {
            WguClasses.SessionInfo.currentUser = user; // Store the user in session info
            await Navigation.PushAsync(new MainPage(user));
        }
        else {
            await DisplayAlert("Login Failed", "Invalid Username or Password", "OK");
        }
    }

   

   
}