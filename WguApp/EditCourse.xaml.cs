namespace WguApp;

public partial class EditCourse : ContentPage
{
    private readonly SqlData.SqlDatahelper _databaseHelper;
    private readonly WguClasses.Course selectedCourse;

    public EditCourse(WguClasses.Course course)
	{
        _databaseHelper = new SqlData.SqlDatahelper();
        this.selectedCourse = course;
        BindingContext = selectedCourse;
        InitializeComponent();
	}

    private bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) &&
               System.Text.RegularExpressions.Regex.IsMatch(email,
                   @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    private bool IsValidPhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        
        var phonePattern = @"^\+?[0-9\s\-\(\)]{7,}$";
        return System.Text.RegularExpressions.Regex.IsMatch(phone, phonePattern);
    }

    private async void Save_Course(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(selectedCourse.Title) ||
      selectedCourse.StartDate == default ||
      selectedCourse.EndDate == default ||
      string.IsNullOrWhiteSpace(selectedCourse.Status) ||
      string.IsNullOrWhiteSpace(selectedCourse.InstructorName) ||
      string.IsNullOrWhiteSpace(selectedCourse.InstructorPhone) ||
      string.IsNullOrWhiteSpace(selectedCourse.InstructorEmail))
        {
            await DisplayAlert("Validation Error", "All fields except notes are required.", "OK");
            return;
        }
        if (!IsValidPhoneNumber(selectedCourse.InstructorPhone)) {
            await DisplayAlert("Formatting Error", "Phone number must be a valid format.", "OK");
            return;
        }
        
        if (selectedCourse.StartDate >= selectedCourse.EndDate)
        {
            await DisplayAlert("Date Error", "Start date must be before end date.", "OK");
            return;
        }

        
        if (!IsValidEmail(selectedCourse.InstructorEmail))
        {
            await DisplayAlert("Invalid Email", "Please enter a valid instructor email.", "OK");
            return;
        }

        await _databaseHelper.UpdateCourse(selectedCourse);
        await Navigation.PopAsync();
    }
}