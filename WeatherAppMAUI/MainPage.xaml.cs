using WeatherAppMAUI.Exceptions;
using WeatherAppMAUI.Services;

namespace WeatherAppMAUI;

public partial class MainPage : ContentPage
{
    private readonly IWeatherClient _weatherClient;

    public MainPage(IWeatherClient weatherClient)
    {
        InitializeComponent();
        _weatherClient = weatherClient;
    }

    private async void OnSearchClicked(object? sender, EventArgs e)
    {
        var city = CityEntry.Text.Trim();
        if (string.IsNullOrEmpty(city))
            return;

        // Show loading, hide everything else
        LoadingIndicator.IsRunning = true;
        LoadingIndicator.IsVisible = true;
        WeatherCard.IsVisible = false;
        ErrorLabel.IsVisible = false;
        SearchButton.IsEnabled = false;

        try
        {
            var weather = await _weatherClient.GetWeatherAsync(city);

            LocationLabel.Text = $"{weather.Name}, {weather.Sys.Country}";
            WeatherIcon.Source = $"https://openweathermap.org/img/wn/{weather.Weather[0].Icon}@2x.png";
            TempLabel.Text = $"{Math.Round(weather.Main.Temp)}°";
            DescriptionLabel.Text = Capitalize(weather.Weather[0].Description);
            FeelsLikeLabel.Text = $"Feels like {Math.Round(weather.Main.FeelsLike)}°";
            HumidityLabel.Text = $"{weather.Main.Humidity}%";
            WindLabel.Text = $"{weather.Wind.Speed:0.0} m/s";
            PressureLabel.Text = $"{weather.Main.Pressure} hPa";

            WeatherCard.IsVisible = true;
        }
        catch (CityNotFoundException ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.IsVisible = true;
        }
        catch (InvalidApiKeyException ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.IsVisible = true;
        }
        catch (HttpRequestException)
        {
            ErrorLabel.Text = "Network error. Check your connection";
            ErrorLabel.IsVisible = true;
        }
        catch (TaskCanceledException)
        {
            ErrorLabel.Text = "Request timed out.";
            ErrorLabel.IsVisible = true;
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
            SearchButton.IsEnabled = true;
        }
    }

    private static string Capitalize(string s) =>
        string.IsNullOrEmpty(s) ? s : char.ToUpper(s[0]) + s.Substring(1);
}