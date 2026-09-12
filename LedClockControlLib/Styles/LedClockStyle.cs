using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfxCustomControls;

internal static class LedClockStyle
{
    private const int PixelSize = 10;

    // Each seven-segment digit occupies 5 x 9 logical pixels
    private const int DigitWidth = 5;
    private const int DigitHeight = 9;

    // The colon occupies 1 logical pixel
    private const int ColonWidth = 1;

    // There are 7 one-pixel gaps:
    // 5 + 1 + 5 + 1 + 1: + 1 + 5 + 1 + 5 + 1 + 1: + 1 + 5 + 1 + 5
    private const int GapCount = 7;

    private const int DigitCount = 6;
    private const int ColonCount = 2;

    private const int DisplayWidthPixels = (DigitCount * DigitWidth) + (ColonCount * ColonWidth) + GapCount;
    private const int DisplayHeightPixels = DigitHeight;

    public static string Name = nameof(LedClockStyle);

    public static Style Build()
    {
        var visualTree = FrameworkElementFactoryX<Canvas>(
            name: "PART_Root",
            setters: [
                SetterX(Canvas.WidthProperty, (double)(DisplayWidthPixels * PixelSize)),
                SetterX(Canvas.HeightProperty, (double)(DisplayHeightPixels * PixelSize)),
            ],
            children: BuildClockDisplay()
        );

        var template = ControlTemplateX<LedClockControl>(visualTree);

        var style = StyleX<LedClockControl>(
            setters: [
                SetterX(StackPanel.BackgroundProperty, Brushes.Black),
                SetterX(Control.TemplateProperty, template),
            ]
        );

        return style;
    }

    private static FrameworkElementFactory[] BuildClockDisplay()
    {
        var children = new List<FrameworkElementFactory>();

        // ---- HOURS ----
        children.AddRange(BuildDigit(DigitUnit.TenHours, x: 0));
        children.AddRange(BuildDigit(DigitUnit.OneHours, x: 60));

        // ---- HOURS COLON ----
        children.Add(BuildColon(x: 120));

        // ---- MINUTES ----
        children.AddRange(BuildDigit(DigitUnit.TenMinutes, x: 140));
        children.AddRange(BuildDigit(DigitUnit.OneMinutes, x: 200));

        // --- SECONDS COLON ----
        children.Add(BuildColon(x: 260, isSeconds: true));

        // ---- SECONDS ----
        children.AddRange(BuildDigit(DigitUnit.TenSeconds, x: 280));
        children.AddRange(BuildDigit(DigitUnit.OneSeconds, x: 340));

        return children.ToArray();
    }

    private static FrameworkElementFactory[] BuildDigit(DigitUnit unit, int x)
    {
        return [
            // [A] Top
            BuildSegment(unit, LedSegment.A, x + PixelSize, 0, width: 3 * PixelSize, height: PixelSize),

            // [B] Upper Right
            BuildSegment(unit, LedSegment.B, x + 4 * PixelSize, PixelSize, width: PixelSize, height: 3 * PixelSize),

            // [C] Lower Right
            BuildSegment(unit, LedSegment.C, x + 4 * PixelSize, 5 * PixelSize, width: PixelSize, height: 3 * PixelSize),

            // [D] Bottom
            BuildSegment(unit, LedSegment.D, x + PixelSize, 8 * PixelSize, width: 3 * PixelSize, height: PixelSize),

            // [E] Lower Left
            BuildSegment(unit, LedSegment.E, x, 5 * PixelSize, width: PixelSize, height: 3 * PixelSize),

            // [F] Upper Left
            BuildSegment(unit, LedSegment.F, x, PixelSize, width: PixelSize, height: 3 * PixelSize),

            // [G] Middle
            BuildSegment(unit, LedSegment.G, x + PixelSize, 4 * PixelSize, width: 3 * PixelSize, height: PixelSize),
        ];
    }

    private static FrameworkElementFactory BuildSegment(DigitUnit unit, LedSegment segment, int x, int y, int width, int height)
    {
        var segmentModel = new SegmentModel(unit, segment);

        return FrameworkElementFactoryX<Rectangle>(
            setters: [
                SetterX(Rectangle.EffectProperty, MultiBindingX(mb => {
                    mb.Bindings.Add(BindingX(b => {
                        b.Path = PropertyPathX(nameof(LedClockControl.IsGlowEffectEnabled));
                        b.RelativeSource = RelativeSourceX(RelativeSourceMode.TemplatedParent);
                    }));

                    mb.Bindings.Add(BindingX(b => {
                        b.Path = PropertyPathX(nameof(LedClockControl.LedDigits));
                        b.RelativeSource = RelativeSourceX(RelativeSourceMode.TemplatedParent);
                    }));

                    mb.Converter = SegmentEffectConverter.Instance;
                    mb.ConverterParameter = segmentModel;
                })),

                SetterX(Canvas.LeftProperty, (double)x),
                SetterX(Canvas.TopProperty,(double)y),
                SetterX(Rectangle.WidthProperty,(double)width),
                SetterX(Rectangle.HeightProperty, (double)height),
                SetterX(Rectangle.SnapsToDevicePixelsProperty, true),

                SetterX(Rectangle.FillProperty, BindingX(b => {
                    b.Path = PropertyPathX(nameof(LedClockControl.LedDigits));
                    b.RelativeSource = RelativeSourceX(RelativeSourceMode.TemplatedParent);
                    b.Converter = SegmentBrushConverter.Instance;
                    b.ConverterParameter = segmentModel;
                }))
            ]
        );
    }

    private static FrameworkElementFactory BuildColon(int x, bool isSeconds = false)
    {       
        return FrameworkElementFactoryX<Canvas>(
            children: [
                BuildColonPixel(x, y: 2 * PixelSize, isSeconds),
                BuildColonPixel(x, y: 6 * PixelSize, isSeconds),
            ]
        );
    }   

    private static FrameworkElementFactory BuildColonPixel(int x, int y, bool isSeconds)
    {
        return FrameworkElementFactoryX<Rectangle>(
            setters: [
                SetterX(Rectangle.EffectProperty, MultiBindingX(mb => {
                    mb.Bindings.Add(BindingX(b => {
                        b.Path = PropertyPathX(nameof(LedClockControl.IsGlowEffectEnabled));
                        b.RelativeSource = RelativeSourceX(RelativeSourceMode.TemplatedParent);
                    }));

                    mb.Bindings.Add(BindingX(b => {
                        b.Path = isSeconds
                            ? PropertyPathX(nameof(LedClockControl.SecondsColonActive))
                            : PropertyPathX(nameof(LedClockControl.MinutesColonActive));
                        b.RelativeSource = RelativeSourceX(RelativeSourceMode.TemplatedParent);
                    }));

                    mb.Converter = ColonEffectConverter.Instance;
                    mb.ConverterParameter = isSeconds;
                })),

                SetterX(Canvas.LeftProperty, (double)x),
                SetterX(Canvas.TopProperty, (double)y),
                SetterX(Rectangle.WidthProperty, (double)PixelSize),
                SetterX(Rectangle.HeightProperty, (double)PixelSize),
                SetterX(Rectangle.SnapsToDevicePixelsProperty, true),
               
                SetterX(Rectangle.FillProperty, BindingX(b => {
                    b.Path = isSeconds
                        ? PropertyPathX(nameof(LedClockControl.SecondsColonActive))
                        : PropertyPathX(nameof(LedClockControl.MinutesColonActive));
                    b.RelativeSource = RelativeSourceX(RelativeSourceMode.TemplatedParent);
                    b.Converter = ColonBrushConverter.Instance;
                    b.ConverterParameter = isSeconds;
                }))
            ]
        );
    }
}
