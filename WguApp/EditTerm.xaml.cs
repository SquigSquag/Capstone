using System.Threading.Tasks;
using WguApp.WguClasses;

namespace WguApp;

public partial class EditTerm : ContentPage
{
    private readonly SqlData.SqlDatahelper _databaseHelper;
    private readonly WguClasses.Term selectedterm;

    public EditTerm(WguClasses.Term term)
    {

        InitializeComponent();
        _databaseHelper = new SqlData.SqlDatahelper();
        this.selectedterm = term;
        termTitleEntry.Text = selectedterm.TermTitle;
        startDatePicker.Date = selectedterm.StartDate;
        endDatePicker.Date = selectedterm.EndDate;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("Save button clicked!");

        if (selectedterm == null)
        {
            Console.WriteLine("ERROR: _term is null!");
            await DisplayAlert("Error", "Term data is missing!", "OK");
            return;
        }

        if (termTitleEntry == null || startDatePicker == null || endDatePicker == null)
        {
            Console.WriteLine("ERROR: One or more UI elements are null!");
            await DisplayAlert("Error", "UI elements are not initialized properly!", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(termTitleEntry.Text))
        {
            await DisplayAlert("Error", "Term title cannot be empty.", "OK");
            return;
        }

        DateTime newStartDate = startDatePicker.Date;
        DateTime newEndDate = endDatePicker.Date;

        
        if (newStartDate >= newEndDate)
        {
            await DisplayAlert("Invalid Dates", "Start date must be before the end date.", "OK");
            return;
        }

        
        if (WguClasses.SessionInfo.currentUser.Role != "Admin" && await _databaseHelper.IsTermOverlapping(newStartDate, newEndDate, selectedterm.TermID, selectedterm.UserId))
        {
            await DisplayAlert("Conflict", "The selected dates overlap with another term.", "OK");
            return;
        }

        
        selectedterm.TermTitle = termTitleEntry.Text;
        selectedterm.StartDate = newStartDate;
        selectedterm.EndDate = newEndDate;

        await _databaseHelper.UpdateTerm(selectedterm);
        await Navigation.PopAsync(); // Return to main page
    }

}