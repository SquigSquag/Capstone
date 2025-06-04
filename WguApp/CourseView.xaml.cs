using Plugin.LocalNotification;
namespace WguApp;


public partial class CourseView : ContentPage
{
    private readonly SqlData.SqlDatahelper _datahelper = new SqlData.SqlDatahelper();
    private readonly WguClasses.Term _term = new WguClasses.Term();

	public CourseView(WguClasses.Term term)
	{
		InitializeComponent();
        this._term = term;
        _datahelper = new SqlData.SqlDatahelper();

    }

    private async void LoadCourses()
    {
        if (_term == null) return;

        var courses = await _datahelper.GetCoursesForTerm(_term.TermID);
        if (courses == null || !courses.Any())
        {
            Console.WriteLine("⚠ No courses found for this term!");
        }

        CourseListView.ItemsSource = courses;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadCourses();  
    }

    private async void ViewselectedCourse(object sender, EventArgs e)
    {
        if (sender is ListView listView && listView.SelectedItem is WguClasses.Course selectedCourse)
        {
            // Navigate to CourseDetails page with the selected course
            await Navigation.PushAsync(new CourseDetails(selectedCourse));

            // Clear selection (optional, but good UX)
            listView.SelectedItem = null;
        }
        else if (sender is Button button && button.BindingContext is WguClasses.Course buttonCourse)
        {
            // If triggered by a button inside a ListView cell
            await Navigation.PushAsync(new CourseDetails(buttonCourse));
        }

    }
    private async void DeleteselectedCourse(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            WguClasses.Course course = (WguClasses.Course)button.BindingContext;
            await _datahelper.DeleteCourse(course);
        }
        LoadCourses();
    }
    private async void AddselectedCourse(object sender, EventArgs e)
    {
        var newCourse = new WguClasses.Course
        {
            TermID = _term.TermID,
            Title = "New Course",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(1),
            Status = "Planned",
            InstructorName = "Instructor",
            InstructorPhone = "000-000-0000",
            InstructorEmail = "email@example.com"
        };

        await _datahelper.AddCourse(newCourse);
        LoadCourses();
    }

    private void Open_Course(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            WguClasses.Course course = (WguClasses.Course)button.BindingContext;
            Navigation.PushAsync(new EditCourse(course));

        }
    }
}