using WeatherAppMAUI.Models;

namespace WeatherAppMAUI.Services;

public interface IWeatherClient
{
    Task<WeatherResponse?> GetWeatherAsync(string city, CancellationToken cancellationToken = default);
}