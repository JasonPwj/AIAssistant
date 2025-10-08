using System.Globalization;

namespace AIAssistant.Converters;

class BoolToBackgroundColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool flag && flag)
        {
            return Colors.LightGray;
        }
        return Colors.Transparent;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        return Binding.DoNothing;
    }
}
