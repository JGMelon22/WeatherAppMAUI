namespace WeatherAppMAUI.Exceptions;

public class CityNotFoundException(string city) : Exception($"City '{city}' not found.")
{
    public string City { get; set; } = city;
}