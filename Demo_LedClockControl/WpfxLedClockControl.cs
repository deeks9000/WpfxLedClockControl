using WpfxCustomControls;

namespace UserExtensions;

public static class WpfxLedClockControl
{
    // FQN: WpfxCustomControls.LedClockControl

    // This static class provides the helper function:
    // UserExtensions.WpfxClockControl.LedClockControlX(...)

    // Add the following line to the file `GlobalUsings.cs`:
    // global using static UserExtensions.WpfxLedClockControl;

    // `LedClockControlX(...)` can then be used directly when building the `UIElement` tree using WPFX

    public static LedClockControl LedClockControlX(Action<LedClockControl>? configure = null)
    {
        var element = new LedClockControl();
        configure?.Invoke(element);
        return element;
    }   
}
