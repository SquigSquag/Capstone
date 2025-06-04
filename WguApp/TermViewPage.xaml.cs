namespace WguApp;

public partial class TermViewPage : ContentPage
{
    private readonly SqlData.SqlDatahelper _datahelper;
    private readonly WguClasses.Login selectedLogin;

    public TermViewPage(WguClasses.Login login)
    {
        InitializeComponent();
        selectedLogin = login;
        _datahelper = new SqlData.SqlDatahelper();
        

    }
    
}