using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WguApp.SqlData;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;


namespace WguApp;

public partial class MainPage : ContentPage
{
    private readonly SqlData.SqlDatahelper _database;
    private readonly WguClasses.Login _login;   
    private WguClasses.Term _term;

    private List<WguClasses.Term> _allTerms = new List<WguClasses.Term>();

    public MainPage(WguClasses.Login login)
	{
		InitializeComponent();
        _database = new SqlData.SqlDatahelper();
        _login = login;
        BindingContext = this; // Set the BindingContext to the current instance
        LoadTerms();
    }

   
   

    private async void LoadTerms()
    {
        _allTerms = await _login.GetVisibleTerms(_database); // Assuming PullTerms returns a List<WguClasses.Term>
        TermListView.ItemsSource = null;  // Clear existing data
        TermListView.ItemsSource = _allTerms; // Refresh list
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadTerms();
    }

    private async void AddTermClicked(object sender, EventArgs e)
    {
        WguClasses.Term blank = new WguClasses.Term
        {
            TermTitle = "Term Title (edit me)",
            StartDate = new DateTime(2025, 1, 10),
            EndDate = new DateTime(2025, 5, 20),
            UserId = _login.UserId
        };

        await _database.CreateTerm(blank);
        LoadTerms();

    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string filter = e.NewTextValue?.ToLower() ?? "";

        var filteredTerms = _allTerms
            .Where(t => t.TermTitle.ToLower().Contains(filter) ||
                        t.StartDate.ToString("MM/dd/yyyy").Contains(filter) ||
                        t.EndDate.ToString("MM/dd/yyyy").Contains(filter))
            .ToList();

        TermListView.ItemsSource = filteredTerms;
    }

    private async void Edit_Term(object sender, EventArgs e)
    {
        Debug.WriteLine("Method called!");
        if (sender is Button button && button.BindingContext is WguClasses.Term term)
        {
            await Navigation.PushAsync(new EditTerm(term));
        }
    }




    private async void Delete_Term(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            WguClasses.Term term = (WguClasses.Term)button.BindingContext;
            await _database.DeleteTerm(term);
        }
        LoadTerms();
    }
    
    private async void TermListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is WguClasses.Term term)
        {
            await Navigation.PushAsync(new CourseView(term));
        }
        ((ListView)sender).SelectedItem = null;

    }

    private async void EvalButton_Clicked(object sender, EventArgs e)
    {
        var sampleTerm = new WguClasses.Term
        {
            TermTitle = "Spring Term 2025",
            StartDate = new DateTime(2025, 3, 1),
            EndDate = new DateTime(2025, 6, 30),
            UserId = _login.UserId

        };
        await _database.CreateTerm(sampleTerm);

        var sampleCourse = new WguClasses.Course
        {
            Title = "C971: Software Development Fundamentals",
            StartDate = new DateTime(2025, 3, 1),
            EndDate = new DateTime(2025, 6, 1),
            Status = "In Progress",
            InstructorName = "Anika Patel",
            InstructorPhone = "555-123-4567",
            InstructorEmail = "anika.patel@strimeuniversity.edu",
            Notes = "Seeded for evaluation.",
            TermID = sampleTerm.TermID

        };
        await _database.AddCourse(sampleCourse);

        var performanceAssessment = new WguClasses.Assessment
        {
            CourseId = sampleCourse.Id,
            Name = "Final Project",
            StartDate = new DateTime(2025, 5, 1),
            EndDate = new DateTime(2025, 5, 15),
            Type = "Performance",
            Turnedin = false
        };
        await _database.AddAssessment(performanceAssessment);
        
        var objectiveAssessment = new WguClasses.Assessment
        {
            CourseId = sampleCourse.Id,
            Name = "Midterm Exam",
            StartDate = new DateTime(2025, 4, 1),
            EndDate = new DateTime(2025, 4, 15),
            Type = "Objective",
            Turnedin = false
        };
        await _database.AddAssessment(objectiveAssessment);
    }

    private void SearchBarTerms_TextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue?.ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(keyword))
        {

            TermListView.ItemsSource = _allTerms; // Restore full list when search is empty
        }
        else
        {
            TermListView.ItemsSource = _allTerms
                .Where(term => term.TermTitle != null && term.TermTitle.ToLower().Contains(keyword))
                .ToList();
        }
    }

    public async Task SaveCSV(string csvContent)
    {
        var filename = $"Terms_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        var filePath = Path.Combine(FileSystem.CacheDirectory, filename);

        File.WriteAllText(filePath, csvContent);

        await Share.RequestAsync(new ShareFileRequest
        {
            Title = "Share Terms CSV",
            File = new ShareFile(filePath, "text/csv")
        });

    }

    
    
    public async Task<string> GenerateTermsCsvAsync(WguClasses.Login user)
    {
        var terms = await _database.PullTerms(user); // Assumes PullTerms filters by user

        if (terms == null || !terms.Any())
            return string.Empty;

        var csv = new StringBuilder();
        csv.AppendLine("TermTitle,StartDate,EndDate");

        foreach (var term in terms)
        {
            string title = term.TermTitle.Replace(",", " "); // Avoid breaking CSV format
            string start = term.StartDate.ToString("MM/dd/yyyy");
            string end = term.EndDate.ToString("MM/dd/yyyy");
            csv.AppendLine($"{title},{start},{end}");
        }

        return csv.ToString();
    }

    private async void ExportTermsCsv_Clicked(object sender, EventArgs e)
    {
        try
        {
            string csvContent = await GenerateTermsCsvAsync(_login);
            if (string.IsNullOrEmpty(csvContent))
            {
                await DisplayAlert("No Data", "No terms available to export.", "OK");
                return;
            }
            await SaveCSV(csvContent);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error exporting CSV: {ex.Message}");
            await DisplayAlert("Error", "Failed to export terms to CSV.", "OK");
        }
    }
}

