using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfxCustomControls;

public class LedClockControl : Control, INotifyPropertyChanged
{
    private readonly DispatcherTimer _timer = new DispatcherTimer();
    private readonly DispatcherTimer _flashTimer = new DispatcherTimer();
    private bool _secondsColonActive = false;
    private bool _minutesColonActive = false;
    private byte[] _ledDigits = new byte[6];

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

        //----------------------------------------------
        // Computation

        LedDigitsComputer.UpdateLedDigits(Timestamp, _ledDigits);

        OnPropertyChanged(nameof(LedDigits));

        //----------------------------------------------
        // Scheduling

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

    // --- DP Wrapper CLR Properties ---

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

    // --- Observable CLR Properties ---

    public byte[] LedDigits => _ledDigits;

    public bool SecondsColonActive
    {
        get => _secondsColonActive;

        private set  
        {
            _secondsColonActive = value;

            OnPropertyChanged(nameof(SecondsColonActive));
        }
    }

    public bool MinutesColonActive
    {
        get => _minutesColonActive;

        private set
        {
            _minutesColonActive = value;

            OnPropertyChanged(nameof(MinutesColonActive));
        }
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
