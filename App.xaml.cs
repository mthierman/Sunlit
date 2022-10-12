using Microsoft.UI.Xaml;

namespace Todo;

public partial class App : Application
{
    private Window _myAppWindow;

    Settings Settings = new Settings();


    public App()
    {
        WebView.WebViewEnv();
        InitializeComponent();
        //Settings.Print();
        //Settings.Save();
        //Settings.Load();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _myAppWindow = new MainWindow();
        _myAppWindow.Activate();
    }
}
