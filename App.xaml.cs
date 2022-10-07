using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using Windows.UI.ViewManagement;

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
            //Test.SetPreferredAppMode(PreferredAppMode.AllowDark);
        }

        //public static event Microsoft.Win32.UserPreferenceChangedEventHandler UserPreferenceChanged;
        //public delegate void UserPreferenceChangedEventHandler(object sender, UserPreferenceChangedEventArgs e);

        //class Test
        //{
        //    [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
        //    public static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);

        //}

        //private enum PreferredAppMode
        //{
        //    Default,
        //    AllowDark,
        //    ForceDark,
        //    ForceLight,
        //    Max
        //}

        //class Test2
        //{
        //    [DllImport("uxtheme.dll", EntryPoint = "#132", SetLastError = true, CharSet = CharSet.Unicode)]
        //    public static extern bool ShouldAppsUseDarkMode(bool attr);
        //};

        //class Test3
        //{
        //    [DllImport("dwmapi.dll")]
        //    private static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttr, ref int pvAttr, int cbAttr);
        //};

        //private enum DWMWINDOWATTRIBUTE
        //{
        //    DWMWA_USE_IMMERSIVE_DARK_MODE = 20
        //}

        //private static void SetWindowImmersiveDarkMode(IntPtr hWnd, bool enabled)
        //{
        //    int isEnabled = enabled ? 1 : 0;
        //    int result = DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref isEnabled, sizeof(int));
        //    if (result != 0) throw new Win32Exception(result);
        //};

        private Window m_window;
    }
}
