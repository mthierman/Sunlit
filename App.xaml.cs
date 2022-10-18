using Microsoft.UI.Xaml;
using System;

namespace Calendar;

public sealed partial class App : Application
{
    private Window _appWindow;

    public App()
    {
        const string _full = "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/full_mode,msOverlayScrollbarWinStyleAnimation";
        //const string _min = "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/minimal_mode,msOverlayScrollbarWinStyleAnimation";
        Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", _full);
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _appWindow = new MainWindow();
        _appWindow.Activate();
    }
}
