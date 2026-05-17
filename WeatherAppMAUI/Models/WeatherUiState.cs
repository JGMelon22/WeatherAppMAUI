namespace WeatherAppMAUI.Models;

public abstract record WeatherUiState
{
    public sealed record Idle : WeatherUiState;

    public sealed record Loading : WeatherUiState;

    public sealed record Success(WeatherResponse Data) : WeatherUiState;

    public sealed record Error(string Message) : WeatherUiState;
}