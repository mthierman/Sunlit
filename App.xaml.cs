using Microsoft.UI.Xaml;

namespace Todo;

public partial class App : Application
{
    private Window _window;
    Settings _settings = new Settings();

    public App()
    {
        InitializeComponent();
        WebView.Initialize();
        _settings.Initialize();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }
}
