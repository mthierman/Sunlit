using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Windows.UI.ViewManagement;
using Windows.UI.Core;

namespace WinUI_Todo
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            _ = ImportTheme.SetPreferredAppMode(PreferredAppMode.AllowDark);

            if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                SetWindowImmersiveDarkMode(WinHandle, true);
            }
            else
            {
                SetWindowImmersiveDarkMode(WinHandle, false);
            }

            Title = "Todo";
            InitializeComponent();

            _uiSettings.ColorValuesChanged += ColorValuesChanged;

            MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 480, Height = 480 });
        }

        private readonly UISettings _uiSettings = new UISettings();

        private IntPtr WinHandle
        {
            get
            { return WindowNative.GetWindowHandle(this); }
        }

        private AppWindow MyAppWindow
        {
            get
            { return GetAppWindowForCurrentWindow(); }
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(WinHandle);
            return AppWindow.GetFromWindowId(wndId);
        }

        private void SwitchPresenter_CompOverlay(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;
            if (((ToggleButton)sender).IsChecked == false)
            {
                MyAppWindow.SetPresenter(AppWindowPresenterKind.Default);
                toggleButton.Content = "\uEE49";
            }
            else
            {
                MyAppWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);
                toggleButton.Content = "\uEE47";
                MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 600, Height = 600 });
            }
        }

        private class ImportTheme
        {
            [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);
        }

        private class ImportWindow
        {
            [DllImport("dwmapi.dll")]
            public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttr, ref int pvAttr, int cbAttr);
        }

        private enum PreferredAppMode
        {
            Default,
            AllowDark,
            ForceDark,
            ForceLight,
            Max
        }

        private enum DWMWINDOWATTRIBUTE
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20
        }

        private static void SetWindowImmersiveDarkMode(IntPtr hWnd, bool enabled)
        {
            int isEnabled = enabled ? 1 : 0;
            int result = ImportWindow.DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref isEnabled, sizeof(int));
            if (result != 0) throw new Win32Exception(result);
        }

        private void ColorValuesChanged(UISettings sender, object args)
        {
            var color = _uiSettings.GetColorValue(UIColorType.Background);
            if (color.ToString() == "#FF000000")
            {
                SetWindowImmersiveDarkMode(WinHandle, true);
            }
            else
            {
                SetWindowImmersiveDarkMode(WinHandle, false);
            }
        }

        public void WindowClosed(object sender, WindowEventArgs e)
        {
            _uiSettings.ColorValuesChanged -= ColorValuesChanged;
        }
    }
}
