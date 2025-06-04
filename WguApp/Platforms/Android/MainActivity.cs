using Android.App;
using Android.Content.PM;
using Android.OS;

namespace WguApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

#if ANDROID
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu) // API 33+
        {
            if (CheckSelfPermission(Android.Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.PostNotifications }, 1001);
            }
            if(CheckSelfPermission(Android.Manifest.Permission.ManageExternalStorage) != Permission.Granted)
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.ManageExternalStorage }, 1002);
            }
            if(CheckSelfPermission(Android.Manifest.Permission.ManageDocuments) != Permission.Granted)
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.ManageDocuments }, 1003);

            }
        }
#endif
    }
}
