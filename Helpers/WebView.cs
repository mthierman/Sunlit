using Microsoft.UI.Xaml;
using System;

namespace Todo;

public partial class App : Application
{
    private void WebViewEnv()
    {
        //Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/minimal_mode,msOverlayScrollbarWinStyleAnimation");
        Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/full_mode,msOverlayScrollbarWinStyleAnimation");
    }
}