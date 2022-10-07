using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Windows.UI.ViewManagement;

namespace WinUI_Todo
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            if (App.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                SetWindowImmersiveDarkMode(hWnd, true);
            }

            Title = "Todo";
            InitializeComponent();
            _uiSettings.ColorValuesChanged += ColorValuesChanged;

            m_AppWindow = GetAppWindowForCurrentWindow();
            m_AppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 480, Height = 480 });
        }

        private AppWindow m_AppWindow;

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        private void SwitchPresenter_CompOverlay(object sender, RoutedEventArgs e)
        {
            m_AppWindow = GetAppWindowForCurrentWindow();
            ToggleButton toggleButton = sender as ToggleButton;
            if (((ToggleButton)sender).IsChecked == false)
            {
                m_AppWindow.SetPresenter(AppWindowPresenterKind.Default);
                toggleButton.Content = "\uEE49";
            }
            else
            {
                m_AppWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);
                toggleButton.Content = "\uEE47";
                m_AppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 600, Height = 600 });
            }
        }

        public class ImportWindow
        {
            [DllImport("dwmapi.dll")]
            public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttr, ref int pvAttr, int cbAttr);
        }

        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20
        }

        public static void SetWindowImmersiveDarkMode(IntPtr hWnd, bool enabled)
        {
            int isEnabled = enabled ? 1 : 0;
            int result = ImportWindow.DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref isEnabled, sizeof(int));
            if (result != 0) throw new Win32Exception(result);
        }

        public void TestColor()
        {
            var uiSettings = new Windows.UI.ViewManagement.UISettings();
            var color = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background);
            System.Diagnostics.Debug.WriteLine(color);
        }

        private readonly UISettings _uiSettings = new UISettings();

        private void ColorValuesChanged(UISettings sender, object args)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            System.Diagnostics.Debug.WriteLine("CHANGES");
            var uiSettings = new Windows.UI.ViewManagement.UISettings();
            var color = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background);
            System.Diagnostics.Debug.WriteLine(color);
            if (color.ToString() == "#FF000000")
            {
                SetWindowImmersiveDarkMode(hWnd, true);
            }
            else
            {
                SetWindowImmersiveDarkMode(hWnd, false);
            }
        }
    }
}
