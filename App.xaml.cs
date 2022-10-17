using Microsoft.UI.Xaml;
using System;

namespace Calendar;

public sealed partial class App : Application
{
    private Window _appWindow;

    public string _webviewFull = "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/full_mode,msOverlayScrollbarWinStyleAnimation";
    public string _webviewMinimal = "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/minimal_mode,msOverlayScrollbarWinStyleAnimation";

    public App()
    {
        Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", _webviewMinimal);
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _appWindow = new MainWindow();
        _appWindow.Activate();
    }
}
