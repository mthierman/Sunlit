using Microsoft.UI.Xaml;

namespace Todo;

public partial class App : Application
{
    public App()
    {
        WebViewEnv();
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _myAppWindow = new MainWindow();
        _myAppWindow.Activate();
    }

    private Window _myAppWindow;
}
