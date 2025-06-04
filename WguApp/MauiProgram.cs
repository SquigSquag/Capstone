using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace WguApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
            .UseLocalNotification(); 
        builder.Services.AddTransient<EditTerm>();

        builder.Services.AddSingleton<SqlData.SqlDatahelper>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
