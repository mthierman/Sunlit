using System;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;

namespace WinUI_Todo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/minimal_mode,msOverlayScrollbarWinStyleAnimation");
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
            ImportTheme.SetPreferredAppMode(PreferredAppMode.AllowDark);
        }

        class ImportTheme
        {
            [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);

        }

        private enum PreferredAppMode
        {
            Default,
            AllowDark,
            ForceDark,
            ForceLight,
            Max
        }

        private Window m_window;
    }
}
