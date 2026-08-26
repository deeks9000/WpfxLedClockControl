using WpfxCustomControls;

namespace UserExtensions;

public static class WpfxAdditions
{    
    public static LedClockControl LedClockControlX(Action<LedClockControl>? configure = null)
    {
        var element = new LedClockControl();
        configure?.Invoke(element);
        return element;
    }   
}
