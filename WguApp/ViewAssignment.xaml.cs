using Plugin.LocalNotification;

namespace WguApp;

public partial class ViewAssignment : ContentPage
{
	private readonly WguClasses.Assessment _assessment;	
    public ViewAssignment(WguClasses.Assessment assessment)
	{
		this._assessment = assessment;
        BindingContext = _assessment;
        InitializeComponent();
	}

    private async void Start_Notify(object sender, EventArgs e)
    {
        if(_assessment == null)
        {
            Console.WriteLine("⚠ No assessment found!");
            return;
        }
        var startNotification = new NotificationRequest
        {
            NotificationId = _assessment.Id * 10+3, // Unique ID
            Title = "Assessment Start Reminder",
            Description = $"{_assessment.Name} starts today!",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = _assessment.StartDate.AddSeconds(5),
                NotifyRepeatInterval = TimeSpan.Zero
            }
        };

        await LocalNotificationCenter.Current.Show(startNotification);
        await DisplayAlert("Notification Set", "Start date notification has been scheduled.", "OK");
    }

    private async void End_Notify(object sender, EventArgs e)
    {
        if (_assessment == null)
        {
            Console.WriteLine("⚠ No assessment found!");
            return;
        }
        var endNotification = new NotificationRequest
        {
            NotificationId = _assessment.Id * 10 + 4, // Unique ID
            Title = "Assessment Start Reminder",
            Description = $"{_assessment.Name} ends today!",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = _assessment.EndDate,
                NotifyRepeatInterval = TimeSpan.Zero
            }
        };

        await LocalNotificationCenter.Current.Show(endNotification);
        await DisplayAlert("Notification Set", "End date notification has been set.", "OK");
    }
}