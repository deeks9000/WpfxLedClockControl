using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace WpfxCustomControls;

internal sealed class SegmentEffectConverter : IMultiValueConverter
{
    public static readonly Effect SecondsEffect = new DropShadowEffect
    {
        ShadowDepth = 0d,
        BlurRadius = 10d,
        Color = Colors.Red
    };

    public static readonly Effect MinutesEffect = new DropShadowEffect
    {
        ShadowDepth = 0d,
        BlurRadius = 10d,
        Color = Colors.DeepSkyBlue
    };

    public static readonly SegmentEffectConverter Instance = new();

    public object? Convert(object[] values, Type targetType, object? parameter,  CultureInfo culture)
    {
        // [0] GlowEffectEnable
        if (values.Length < 2 || values[0] is not true)
            return null;

        // [1] LedDigits
        if (values[1] is not byte[] ledDigits)
            return null;

        // Static template metadata
        if (parameter is not SegmentModel segmentModel)
            return null;

        // No lit segment = no glow
        byte digitMask = ledDigits[(int)segmentModel.Unit];

        bool isLit = (digitMask & segmentModel.SegmentMask) != 0;

        if (!isLit) 
            return null;

        return segmentModel.IsSeconds()
            ? SecondsEffect
            : MinutesEffect;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
