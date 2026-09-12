using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfxCustomControls;

internal class ColonBrushConverter : IValueConverter
{
    public static readonly Brush ColonOffBrush = new SolidColorBrush(Color.FromArgb(32, 150, 150, 150));   // Small Alpha means MORE transparent!

    public static readonly Brush SecondsColonOnBrush = Brushes.Red;

    public static readonly Brush MinutesColonOnBrush = Brushes.DeepSkyBlue;

    public static readonly ColonBrushConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool colonState)
            return ColonOffBrush;

        if (parameter is not bool isSeconds)
            return ColonOffBrush;
                
        if (!colonState)
            return ColonOffBrush;

        return isSeconds is true
            ? SecondsColonOnBrush
            : MinutesColonOnBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
