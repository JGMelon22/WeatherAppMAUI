using Microsoft.Extensions.Logging;
using WeatherAppMAUI.Services;

namespace WeatherAppMAUI;

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
            });

        builder.Services.AddHttpClient<IWeatherClient, WeatherService>(client =>
        {
            client.BaseAddress = new Uri("https://api.openweathermap.org/");
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}