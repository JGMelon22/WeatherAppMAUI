using System.Text.Json.Serialization;

namespace WeatherAppMAUI.Models;

public record WeatherResponse(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("weather")]
    IReadOnlyList<WeatherCondition> Weather,
    [property: JsonPropertyName("main")] MainData Main,
    [property: JsonPropertyName("wind")] WindData Wind,
    [property: JsonPropertyName("sys")] SysData Sys
);

public record WeatherCondition(
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("icon")] string Icon
);

public record MainData(
    [property: JsonPropertyName("temp")] double Temp,
    [property: JsonPropertyName("feels_like")]
    double FeelsLike,
    [property: JsonPropertyName("humidity")]
    int Humidity,
    [property: JsonPropertyName("pressure")]
    int Pressure
);

public record WindData(
    [property: JsonPropertyName("speed")] double Speed
);

public record SysData(
    [property: JsonPropertyName("country")]
    string Country
);