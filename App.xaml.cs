using Microsoft.UI.Xaml;
using System;

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
            m_window.TrySetMicaBackdrop();
        }

        //public enum BackdropType
        //{
        //    Mica,
        //    MicaAlt
        //}

        private Window m_window;
    }
}
