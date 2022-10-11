using Microsoft.UI.Xaml;

namespace Todo;

public partial class App : Application
{
    public App()
    {
        WebView.WebViewEnv();
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _myAppWindow = new MainWindow();
        _myAppWindow.Activate();
        //SaveTestSetting();
        //LoadTestSetting();
    }

    private Window _myAppWindow;
}
