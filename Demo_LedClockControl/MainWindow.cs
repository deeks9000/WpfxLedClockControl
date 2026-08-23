using System.Windows;
using System.Windows.Media;

namespace Demo_LedClockControl;

public class MainWindow : Window
{
    public MainWindow()
    {
        Title = "Demo LedClockControl";
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Width = 800;
        Height = 500;
        //Content = Build();
        Content = BuildWithViewbox();
    }

    private UIElement Build()
    {
        return GridX(
            configure: x => {
                x.Background = Brushes.Black;
            },
            children: [
                LedClockControlX(
                    configure: x => {
                        x.HorizontalAlignment = HorizontalAlignment.Center;
                        x.VerticalAlignment = VerticalAlignment.Center;
                        x.Margin = ThicknessX(10);
                        x.IsSecondsColonFlashEnabled = true;
                        x.IsMinutesColonFlashEnabled = true;
                        x.IsGlowEffectEnabled = true;
                    }
                )
            ]
        );
    }

    private UIElement BuildWithViewbox()
    {
        return GridX(
            configure: x => {
                x.Background = Brushes.Black;
            },
            children: [
                ViewboxX(
                    child: LedClockControlX(
                        configure: x => {
                            x.HorizontalAlignment = HorizontalAlignment.Center;
                            x.VerticalAlignment = VerticalAlignment.Center;
                            x.Margin = ThicknessX(10);
                            x.IsSecondsColonFlashEnabled = true;
                            x.IsMinutesColonFlashEnabled = true;
                            x.IsGlowEffectEnabled = true;
                        }
                    )
                )
            ]
        );
    }
}
