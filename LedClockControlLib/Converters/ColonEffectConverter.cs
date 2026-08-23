using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;

public sealed class ColonEffectConverter : IMultiValueConverter
{
    public static readonly Effect SecondsColonEffect = new DropShadowEffect
    {
        ShadowDepth = 0d,
        BlurRadius = 10d,
        Color = Colors.Red
    };

    public static readonly Effect MinutesColonEffect = new DropShadowEffect
    {
        ShadowDepth = 0d,
        BlurRadius = 10d,
        Color = Colors.DeepSkyBlue
    };

    public static readonly ColonEffectConverter Instance = new();

    public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return null;

        // Glow disabled
        if (values[0] is not true)
            return null;

        // Colon is not active
        if (values[1] is not true)
            return null;

        // Static template metadata
        if (parameter is not bool isSeconds)
            return null;

        return isSeconds
            ? SecondsColonEffect
            : MinutesColonEffect;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
