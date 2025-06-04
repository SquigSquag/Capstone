namespace WguApp;

public partial class EditAssessment : ContentPage
{
    private readonly SqlData.SqlDatahelper _databaseHelper;
    private readonly WguClasses.Assessment selectedAssessment;

    public EditAssessment(WguClasses.Assessment assessment)

	{
        _databaseHelper = new SqlData.SqlDatahelper();
        this.selectedAssessment = assessment;
        BindingContext = selectedAssessment;
       
        InitializeComponent();
        IsTurnedIn.IsToggled = selectedAssessment.Turnedin;
    }

    private async void Save_Assessment(object sender, EventArgs e)
    {
        if(string.IsNullOrWhiteSpace(selectedAssessment.Name) ||
            selectedAssessment.StartDate == default ||
            selectedAssessment.EndDate == default ||
            string.IsNullOrWhiteSpace(selectedAssessment.Type))
        {
            await DisplayAlert("Validation Error", "All fields must be filled out.", "OK");
            return;
        }
        await _databaseHelper.UpdateAssessment(selectedAssessment);
        await Navigation.PopAsync();
    }

    private void IsTurnedIn_Toggled(object sender, ToggledEventArgs e)
    {
        if (selectedAssessment != null)
        {
            selectedAssessment.Turnedin = e.Value;
            IsTurnedInLabel.Text = $"Turned In: {(e.Value ? "Yes" : "No")}";

        }
    }
}