using System.Globalization;
using WeatherAppMAUI.Models;

namespace WeatherAppMAUI.Converters;

public class DescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not WeatherResponse weather || weather.Weather.Count == 0)
            return string.Empty;

        var description = weather.Weather[0].Description;
        return string.IsNullOrEmpty(description)
            ? string.Empty
            : char.ToUpper(description[0]) + description[1..];
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}