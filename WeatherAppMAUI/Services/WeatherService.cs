using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using WeatherAppMAUI.Constants;
using WeatherAppMAUI.Exceptions;
using WeatherAppMAUI.Models;

namespace WeatherAppMAUI.Services;

public class WeatherService(HttpClient httpClient, IMemoryCache cache) : IWeatherClient
{
    public async Task<WeatherResponse?> GetWeatherAsync(string city, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"city:{city.ToLowerInvariant()}";

        if (cache.TryGetValue(cacheKey, out WeatherResponse? weatherResponse))
            return weatherResponse;

        var url = $"data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={ApiKeyConstant.ApiKey}&units=metric";

        var response = await httpClient.GetAsync(url, cancellationToken);

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                throw new CityNotFoundException(city);
            case HttpStatusCode.Unauthorized:
                throw new InvalidApiKeyException();
        }

        response.EnsureSuccessStatusCode();

        weatherResponse = await response.Content.ReadFromJsonAsync<WeatherResponse>(cancellationToken);

        if (weatherResponse is null)
            throw new InvalidOperationException("Empty response from weather API.");

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetPriority(CacheItemPriority.Normal)
            .SetSize(1);

        cache.Set(cacheKey, weatherResponse, cacheOptions);

        return weatherResponse;
    }
}