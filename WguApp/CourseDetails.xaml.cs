using System;
using System.Collections.Generic;
using Plugin.LocalNotification;

namespace WguApp;

public partial class CourseDetails : ContentPage
{
    private readonly SqlData.SqlDatahelper _datahelper;
    private readonly WguClasses.Course selectedCourse;
    private readonly List<WguClasses.Assessment> assessments;

	public CourseDetails(WguClasses.Course course)
	{
		this.selectedCourse = course;
		InitializeComponent();
        _datahelper = new SqlData.SqlDatahelper();
        BindingContext = selectedCourse;
        Notes_Editor.Text = selectedCourse.Notes;

    }
    private async Task LoadAssessments()
    {
        if (selectedCourse == null) return;
        AssessmentsListView.ItemsSource = await _datahelper.PullAssessments(selectedCourse.Id);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

       await LoadAssessments();
    }

    private async void Add_Assessment(object sender, EventArgs e)
    {
        var newAssessment = new WguClasses.Assessment
        {
            CourseId = selectedCourse.Id,
            Name = "New Assessment",
            Type = "Performance", // Default type
            Turnedin = false
        };

        await _datahelper.AddAssessment(newAssessment);
        LoadAssessments(); // Refresh list
    }

    private async void Delete_Assessment(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            WguClasses.Assessment assessment = (WguClasses.Assessment)button.BindingContext;
            await _datahelper.DeleteAssessment(assessment);
            await LoadAssessments(); // Refresh list
        }
    }

    private async void Share_Notes(object sender, EventArgs e)
    {
        if(Notes_Editor.Text == null || Notes_Editor.Text.Length == 0 )
        {
            await DisplayAlert("Error", "No notes to share.", "OK");
            return;
        }
        await Share.RequestAsync(new ShareTextRequest
        {
            Text = Notes_Editor.Text,
            Title = "Notes"

        });

    }
    private async void Set_StartselectedCourse_Notification(object sender, EventArgs e)
    {
        if (selectedCourse == null) return;

        DateTime baseDate = selectedCourse.StartDate.Date;
        DateTime timeFromNow = DateTime.Now.AddSeconds(5);

        var startNotification = new NotificationRequest
        {
            NotificationId = selectedCourse.Id * 10, // Unique ID
            Title = "Course Start Reminder",
            Description = $"{selectedCourse.Title} starts today!",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = baseDate
                .AddHours(timeFromNow.Hour)
                .AddMinutes(timeFromNow.Minute)
                .AddSeconds(timeFromNow.Second),

                NotifyRepeatInterval = TimeSpan.Zero
            }
        };

        await LocalNotificationCenter.Current.Show(startNotification);
        await DisplayAlert("Notification Set", "Start date notification has been scheduled.", "OK");


    }
    private async void Set_EndselectedCourse_Notification(object sender, EventArgs e)
    {
        if (selectedCourse == null) return;
        var endNotification = new NotificationRequest
        {
            NotificationId = selectedCourse.Id * 10 + 1, // Unique ID
            Title = "Course End Reminder",
            Description = $"{selectedCourse.Title} ends today!",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = selectedCourse.EndDate,
                NotifyRepeatInterval = TimeSpan.Zero
            }
        };

        await LocalNotificationCenter.Current.Show(endNotification);
        await DisplayAlert("Notification Set", "End date notification has been scheduled.", "OK");

    }

    private async void View_Assessment(object sender, EventArgs e)
    {
        if (sender is ListView listView && listView.SelectedItem is WguClasses.Assessment selectedAssessment)
        {
            // Navigate to ViewAssignment page with the selected assessment
            await Navigation.PushAsync(new ViewAssignment(selectedAssessment));
            // Clear selection (optional, but good UX)
            listView.SelectedItem = null;
        }
        else if (sender is Button button && button.BindingContext is WguClasses.Assessment buttonAssessment)
        {
            // If triggered by a button inside a ListView cell
            await Navigation.PushAsync(new ViewAssignment(buttonAssessment));
        }
    }

    private async void Edit_Assessment(object sender, EventArgs e)
    { 
        if (sender is Button button && button.BindingContext is WguClasses.Assessment buttonAssessment)
        {
           
            await Navigation.PushAsync(new EditAssessment(buttonAssessment));
        }
    }
}