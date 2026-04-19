using System.Globalization;

namespace Lab01Ivanov.Converters
{
    /// <summary>Inverts a bool — used to disable buttons while IsBusy=true.</summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b && !b;
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is bool b && !b;
    }
}
