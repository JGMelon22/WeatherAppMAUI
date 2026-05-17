using System.Globalization;
using WeatherAppMAUI.Models;

namespace WeatherAppMAUI.Converters;

public class WeatherIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not WeatherResponse weather || weather.Weather.Count == 0)
            return null;

        var icon = weather.Weather[0].Icon;
        return $"https://openweathermap.org/img/wn/{icon}@2x.png";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}