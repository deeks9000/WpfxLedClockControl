using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using WpfxCustomControls;

internal class SegmentBrushConverter : IValueConverter
{
    public static readonly Brush SegmentOffBrush = new SolidColorBrush(Color.FromArgb(32, 150, 150, 150));  // Small Alpha means MORE transparent!

    public static readonly Brush SecondsSegmentOnBrush = Brushes.Red;
    public static readonly Brush MinutesSegmentOnBrush = Brushes.DeepSkyBlue;

    public static readonly SegmentBrushConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not byte[] ledDigits)
            return SegmentOffBrush;

        if (parameter is not SegmentModel segmentModel)
            return SegmentOffBrush;

        byte digitBitMask = ledDigits[(int)segmentModel.Unit];

        bool isLit = (digitBitMask & segmentModel.SegmentBitMask) != 0;

        if (!isLit)
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
