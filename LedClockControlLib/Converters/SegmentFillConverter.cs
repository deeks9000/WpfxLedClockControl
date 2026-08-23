using LedClockControlLib.Models;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfxCustomControls.Converters;

public class SegmentFillConverter : IValueConverter
{
    public static readonly Brush SegmentOffBrush = new SolidColorBrush(Color.FromArgb(32, 150, 150, 150));  // Small Alpha means MORE transparent!

    public static readonly Brush SecondsSegmentOnBrush = Brushes.Red;

    public static readonly Brush MinutesSegmentOnBrush = Brushes.DeepSkyBlue;

    public static readonly SegmentFillConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {       
        if (value is not DateTime timestamp)
            return SegmentOffBrush;

        if (parameter is not SegmentModel segmentModel)
            return SegmentOffBrush;

        // No lit segment = OFF brush
        if (!segmentModel.IsSegmentLit(timestamp))
            return SegmentOffBrush;

        return segmentModel.IsSeconds()
            ? SecondsSegmentOnBrush
            : MinutesSegmentOnBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
