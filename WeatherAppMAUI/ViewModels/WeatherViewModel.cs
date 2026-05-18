using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherAppMAUI.Exceptions;
using WeatherAppMAUI.Models;
using WeatherAppMAUI.Services;

namespace WeatherAppMAUI.ViewModels;

public partial class WeatherViewModel(IWeatherClient weatherClient) : ObservableObject
{
    [ObservableProperty] private string _city = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsIdle))]
    [NotifyPropertyChangedFor(nameof(IsLoading))]
    [NotifyPropertyChangedFor(nameof(IsError))]
    [NotifyPropertyChangedFor(nameof(HasWeather))]
    [NotifyPropertyChangedFor(nameof(Weather))]
    [NotifyPropertyChangedFor(nameof(ErrorMessage))]
    private WeatherUiState _state = new WeatherUiState.Idle();

    public bool IsIdle => State is WeatherUiState.Idle;
    public bool IsLoading => State is WeatherUiState.Loading;
    public bool IsError => State is WeatherUiState.Error;
    public bool HasWeather => State is WeatherUiState.Success;

    public WeatherResponse? Weather => (State as WeatherUiState.Success)?.Data;
    public string? ErrorMessage => (State as WeatherUiState.Error)?.Message;

    [RelayCommand]
    private async Task SearchAsync()
    {
        var city = City.Trim();
        if (string.IsNullOrEmpty(city))
            return;

        State = new WeatherUiState.Loading();

        try
        {
            var weather = await weatherClient.GetWeatherAsync(city);
            State = new WeatherUiState.Success(weather!);
        }
        catch (CityNotFoundException ex)
        {
            State = new WeatherUiState.Error(ex.Message);
        }
        catch (InvalidApiKeyException ex)
        {
            State = new WeatherUiState.Error(ex.Message);
        }
        catch (HttpRequestException)
        {
            State = new WeatherUiState.Error("Network error. Check your connection.");
        }
        catch (TaskCanceledException)
        {
            State = new WeatherUiState.Error("Request timed out.");
        }
    }
}