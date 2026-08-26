using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfxCustomControls;

public class LedClockControl : Control
{
    private readonly DispatcherTimer _timer = new DispatcherTimer();
    private readonly DispatcherTimer _flashTimer = new DispatcherTimer();

    public static Style DefaultStyle { get; }

    //------------------------------------------------------------------------------
    // Static Constructor

    static LedClockControl()
    {
        DefaultStyle = LedClockStyle.Build();

        StyleProperty.OverrideMetadata(typeof(LedClockControl), new FrameworkPropertyMetadata(DefaultStyle));
    }

    //------------------------------------------------------------------------------
    // Instance Constructor

    public LedClockControl()
    {
        _timer.Tick += Timer_Tick;

        _flashTimer.Tick += FlashTimer_Tick;

        Loaded += ClockControl_Loaded;
        Unloaded += ClockControl_Unloaded;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        Timestamp = DateTime.Now;

        int phaseOffsetMs = Timestamp.Millisecond;

        int nextSecondDelayMs = 1000 - phaseOffsetMs;

        int flashDelayMs = 500 - phaseOffsetMs;

        if (flashDelayMs <= 0)
        {
            flashDelayMs += 1000;
        }

        _timer.Interval = TimeSpan.FromMilliseconds(nextSecondDelayMs);

        SecondsColonActive = true;
        MinutesColonActive = true;

        if (IsSecondsColonFlashEnabled || IsMinutesColonFlashEnabled)
        {
            _flashTimer.Interval = TimeSpan.FromMilliseconds(flashDelayMs);

            _flashTimer.Stop();
            _flashTimer.Start();
        }
    }

    private void FlashTimer_Tick(object? sender, EventArgs e)
    {
        _flashTimer.Stop();

        if (IsSecondsColonFlashEnabled)
        {
            SecondsColonActive = false;
        }

        if (IsMinutesColonFlashEnabled)
        {
            MinutesColonActive = false;
        }
    }

    private void ClockControl_Loaded(object sender, RoutedEventArgs e)
    {
        _timer.Interval = TimeSpan.Zero;
        _timer.Start();
    }

    private void ClockControl_Unloaded(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _flashTimer.Stop();
    }

    //------------------------------------------------------------------------------
    // Properties

    // --- Dependency Properties ---

    public static readonly DependencyProperty TimestampProperty = DependencyProperty.Register(
        nameof(Timestamp),
        typeof(DateTime),
        typeof(LedClockControl),
        new PropertyMetadata(DateTime.Now, TimestampPropertyChangedCallback)
    );

    public static readonly DependencyProperty IsSecondsColonFlashEnabledProperty = DependencyProperty.Register(
        nameof(IsSecondsColonFlashEnabled),
        typeof(bool),
        typeof(LedClockControl),
        new PropertyMetadata(false)
    );

    public static readonly DependencyProperty IsMinutesColonFlashEnabledProperty = DependencyProperty.Register(
        nameof(IsMinutesColonFlashEnabled),
        typeof(bool),
        typeof(LedClockControl),
        new PropertyMetadata(false)
    );

    public static readonly DependencyProperty IsGlowEffectEnabledProperty = DependencyProperty.Register(
        nameof(IsGlowEffectEnabled),
        typeof(bool),
        typeof(LedClockControl),
        new PropertyMetadata(false)
    );

    public static readonly DependencyProperty SecondsColonActiveProperty = DependencyProperty.Register(
        nameof(SecondsColonActive),
        typeof(bool),
        typeof(LedClockControl),
        new PropertyMetadata(true)
    );

    public static readonly DependencyProperty MinutesColonActiveProperty = DependencyProperty.Register(
        nameof(MinutesColonActive),
        typeof(bool),
        typeof(LedClockControl),
        new PropertyMetadata(true)
    );


    // --- CLR Properties ---

    public DateTime Timestamp
    {
        get => (DateTime)GetValue(TimestampProperty);
        set => SetValue(TimestampProperty, value);
    }

    public bool IsSecondsColonFlashEnabled
    {
        get => (bool)GetValue(IsSecondsColonFlashEnabledProperty);
        set => SetValue(IsSecondsColonFlashEnabledProperty, value);
    }

    public bool IsMinutesColonFlashEnabled
    {
        get => (bool)GetValue(IsMinutesColonFlashEnabledProperty);
        set => SetValue(IsMinutesColonFlashEnabledProperty, value);
    }

    public bool IsGlowEffectEnabled
    {
        get => (bool)GetValue(IsGlowEffectEnabledProperty);
        set => SetValue(IsGlowEffectEnabledProperty, value);
    }

    public bool SecondsColonActive
    {
        get => (bool)GetValue(SecondsColonActiveProperty);
        private set => SetValue(SecondsColonActiveProperty, value);
    }

    public bool MinutesColonActive
    {
        get => (bool)GetValue(MinutesColonActiveProperty);
        private set => SetValue(MinutesColonActiveProperty, value);
    }


    //------------------------------------------------------------------------------
    // Events

    private static void TimestampPropertyChangedCallback(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
    {
        LedClockControl ctrl = (LedClockControl)depObj;

        DateTime oldValue = (DateTime)e.OldValue;
        DateTime newValue = (DateTime)e.NewValue;

        RoutedPropertyChangedEventArgs<DateTime> args = new RoutedPropertyChangedEventArgs<DateTime>(oldValue, newValue);
        args.RoutedEvent = LedClockControl.TimestampChangedEvent;

        ctrl.RaiseEvent(args);
    }

    // RoutedEvent
    public static readonly RoutedEvent TimestampChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(TimestampChanged),
        RoutingStrategy.Bubble,
        typeof(RoutedPropertyChangedEventHandler<DateTime>),
        typeof(LedClockControl)
    );

    // CLR Event
    public event RoutedPropertyChangedEventHandler<DateTime> TimestampChanged
    {
        add
        {
            AddHandler(TimestampChangedEvent, value);
        }

        remove
        {
            RemoveHandler(TimestampChangedEvent, value);
        }
    }
}
