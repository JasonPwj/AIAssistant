using System.Globalization;

namespace AIAssistant.Converters;

class BoolToLayoutConverter : IValueConverter
{
    object? IValueConverter.Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (value is bool flag)
        {
            return flag ? LayoutOptions.End : LayoutOptions.Start;
        }
        return LayoutOptions.Start;
    }

    object? IValueConverter.ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        return Binding.DoNothing;
    }
}
