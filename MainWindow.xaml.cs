using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.UI.ViewManagement;
using WinRT.Interop;

namespace WinUI_Todo
{
    public sealed partial class MainWindow : Window
    {
        // MAIN WINDOW
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

            DefaultPresenter();

            InitializeComponent();

            StartListener();

            //MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 480, Height = 480 });
        }

        // LISTENER
        private readonly UISettings _uiSettings = new();

        private bool EventHandlersCreated;

        private void StartListener()
        {
            EventHandlersCreated = true;
            _uiSettings.ColorValuesChanged += ColorValuesChanged;
        }

        private void StopListener()
        {
            EventHandlersCreated = false;
            _uiSettings.ColorValuesChanged -= ColorValuesChanged;
        }

        public void WindowClosed(object sender, WindowEventArgs e)
        {
            if (EventHandlersCreated)
            {
                StopListener();
            }
        }

        // APP WINDOW
        private IntPtr WinHandle
        {
            get
            { return WindowNative.GetWindowHandle(this); }
        }

        private WindowId WinID
        {
            get
            { return Win32Interop.GetWindowIdFromWindow(WinHandle); }
        }

        private AppWindow MyAppWindow
        {
            get
            { return AppWindow.GetFromWindowId(WinID); }
        }

        // COMPACT OVERLAY

        private OverlappedPresenter MyDefaultPresenter;
        private OverlappedPresenter MyCompactPresenter;

        public int DefaultWidth { get; set; } = 600;
        public int DefaultHeight { get; set; } = 600;

        public int CompactWidth { get; set; } = 400;
        public int CompactHeight { get; set; } = 400;

        private void DefaultPresenter()
        {
            MyAppWindow.SetPresenter(AppWindowPresenterKind.Default);
            MyDefaultPresenter = MyAppWindow.Presenter as OverlappedPresenter;
            MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = DefaultWidth, Height = DefaultHeight });
            MyDefaultPresenter.IsAlwaysOnTop = false;
            MyDefaultPresenter.IsMaximizable = true;
            MyDefaultPresenter.IsMinimizable = true;
        }

        private void CompactPresenter()
        {
            MyAppWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
            MyCompactPresenter = MyAppWindow.Presenter as OverlappedPresenter;
            MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = CompactWidth, Height = CompactHeight });
            MyCompactPresenter.IsAlwaysOnTop = true;
            MyCompactPresenter.IsMaximizable = false;
            MyCompactPresenter.IsMinimizable = false;
        }

        private void SwitchPresenter_CompactOverlay(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;
            if (((ToggleButton)sender).IsChecked == false)
            {
                CompactHeight = MyAppWindow.Size.Height;
                CompactWidth = MyAppWindow.Size.Width;
                DefaultPresenter();
                toggleButton.Content = "\uEE49";
            }
            else
            {
                DefaultHeight = MyAppWindow.Size.Height;
                DefaultWidth = MyAppWindow.Size.Width;
                CompactPresenter();
                toggleButton.Content = "\uEE47";

            }
        }

        // WIN32 THEMING
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
    }
}
