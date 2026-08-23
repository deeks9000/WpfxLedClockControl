using System.Windows;

namespace Demo_LedClockControl;

public static class Program
{
    [STAThread]
    public static void Main()
    { 
        var app = new Application();

        var win = new MainWindow();

        app.Run(win);
    }
}
