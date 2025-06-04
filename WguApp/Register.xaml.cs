

namespace WguApp;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

    public async void Button_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(usernameEntry.Text) || string.IsNullOrWhiteSpace(passwordEntry.Text))
        {
            DisplayAlert("Error", "Please enter both username and password.", "OK");
            return;
        }
        SqlData.SqlDatahelper sqlData = new SqlData.SqlDatahelper();
		WguClasses.Login newUser = new WguClasses.Login { Password = passwordEntry.Text, Username = usernameEntry.Text, Role = "User" };
        int result = await sqlData.CreateUser(newUser);
        if (result == -1)
        {
            DisplayAlert("Error", "User already exists", "OK");
        }
        else
        {
            DisplayAlert("Success", "User created successfully", "OK");
            // Optionally, navigate to the login page or clear the fields
            usernameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
            Navigation.PopAsync(); // Assuming you want to go back to the login page
        }
    }

    private async void AdminAdd_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(usernameEntry.Text) || string.IsNullOrWhiteSpace(passwordEntry.Text))
        {
            DisplayAlert("Error", "Please enter both username and password.", "OK");
            return;
        }
        SqlData.SqlDatahelper sqlData = new SqlData.SqlDatahelper();
        WguClasses.Login newUser = new WguClasses.Login{ Password = passwordEntry.Text, Username = usernameEntry.Text, Role = "Admin" };
        int result = await sqlData.CreateUser(newUser);
        if (result == -1)
        {
            DisplayAlert("Error", "Admin already exists", "OK");
        }
        else
        {
            DisplayAlert("Success", "Admin user created successfully", "OK");
            // Optionally, navigate to the login page or clear the fields
            usernameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
            Navigation.PopAsync(); // Assuming you want to go back to the login page
        }

    }
}