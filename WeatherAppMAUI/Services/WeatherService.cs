using System.Net;
using System.Net.Http.Json;
using WeatherAppMAUI.Exceptions;
using WeatherAppMAUI.Models;

namespace WeatherAppMAUI.Services;

public class WeatherService(HttpClient httpClient) : IWeatherClient
{
    private const string ApiKey = "TODO";

    public async Task<WeatherResponse> GetWeatherAsync(string city, CancellationToken cancellationToken = default)
    {
        var url = $"data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={ApiKey}&units=metrics";

        var response = await httpClient.GetAsync(url, cancellationToken);

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                throw new CityNotFoundException(city);
            case HttpStatusCode.Unauthorized:
                throw new InvalidApiKeyException();
        }

        response.EnsureSuccessStatusCode();

        var weather = await response.Content.ReadFromJsonAsync<WeatherResponse>(cancellationToken);

        return weather ?? throw new InvalidOperationException("Empty response from weather API.");
    }
}