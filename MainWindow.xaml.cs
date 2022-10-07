using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Web.WebView2.Core;
using WinRT;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Windows.UI.Core;
using Microsoft.Win32;

namespace WinUI_Todo
{
    public sealed partial class MainWindow : Window
    {
        private AppWindow m_AppWindow;

        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20
        }

        public class PInvokeTheme
        {
            [DllImport("dwmapi.dll")]
            public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttr, ref int pvAttr, int cbAttr);
        }

        public static void SetWindowImmersiveDarkMode(IntPtr hWnd, bool enabled)
        {
            int isEnabled = enabled ? 1 : 0;
            int result = PInvokeTheme.DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref isEnabled, sizeof(int));
            if (result != 0) throw new Win32Exception(result);
        }

        public MainWindow()
        {
            Title = "Test Title";

            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            if (App.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                SetWindowImmersiveDarkMode(hWnd, true);
            }

            InitializeComponent();

            m_AppWindow = GetAppWindowForCurrentWindow();

            m_AppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 480, Height = 480 });

            //SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            //StartListening();

            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                ExtendsContentIntoTitleBar = true;
                SetTitleBar(AppTitleBar);
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
                //titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
            }

            //SetTitleBarColors();

            //MyWebView.NavigationStarting += EnsureHttps;
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

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        //public delegate void UserPreferenceChangedEventHandler(object sender, UserPreferenceChangedEventArgs e);

        //public event UserPreferenceChangedEventHandler UserPreferenceChanged;

        //public bool eventHandlersCreated;

        //public void StartListening()
        //{
        //    SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        //    System.Diagnostics.Debug.WriteLine("Started listening!!!");
        //    eventHandlersCreated = true;
        //}

        //public void StopListening()
        //{
        //    SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        //    System.Diagnostics.Debug.WriteLine("Stopped listening!!!");
        //    eventHandlersCreated = false;
        //}

        //private void StartListening()
        //{
        //    SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        //    System.Diagnostics.Debug.WriteLine("Started listening!!!");
        //}

        //private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("Theme changed!!!");
        //}

        //public void WindowClosed(object sender, WindowEventArgs e)
        //{
        //    SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        //    System.Diagnostics.Debug.WriteLine("Closed!!!");
        //}

        //public void WindowClosed(object sender, WindowEventArgs e)
        //{
        //    if (eventHandlersCreated)
        //    {
        //        SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        //        System.Diagnostics.Debug.WriteLine("Closed!!!");
        //        //StopListening();
        //    }
        //}

        //private bool SetTitleBarColors()
        //{
        //    // Check to see if customization is supported.
        //    // Currently only supported on Windows 11.
        //    if (AppWindowTitleBar.IsCustomizationSupported())
        //    {
        //        if (m_AppWindow is null)
        //        {
        //            m_AppWindow = GetAppWindowForCurrentWindow();
        //        }
        //        var titleBar = m_AppWindow.TitleBar;

        //        // Set active window colors
        //        titleBar.ForegroundColor = Colors.White;
        //        titleBar.BackgroundColor = Colors.Green;
        //        titleBar.ButtonForegroundColor = Colors.White;
        //        titleBar.ButtonBackgroundColor = Colors.SeaGreen;
        //        titleBar.ButtonHoverForegroundColor = Colors.Gainsboro;
        //        titleBar.ButtonHoverBackgroundColor = Colors.DarkSeaGreen;
        //        titleBar.ButtonPressedForegroundColor = Colors.Gray;
        //        titleBar.ButtonPressedBackgroundColor = Colors.LightGreen;

        //        // Set inactive window colors
        //        titleBar.InactiveForegroundColor = Colors.Gainsboro;
        //        titleBar.InactiveBackgroundColor = Colors.SeaGreen;
        //        titleBar.ButtonInactiveForegroundColor = Colors.Gainsboro;
        //        titleBar.ButtonInactiveBackgroundColor = Colors.SeaGreen;
        //        return true;
        //    }
        //    return false;
        //}

        //private void EnsureHttps(WebView2 sender, CoreWebView2NavigationStartingEventArgs args)
        //{
        //    String uri = args.Uri;
        //    if (!uri.StartsWith("https://"))
        //    {
        //        MyWebView.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
        //        args.Cancel = true;
        //    }
        //    else
        //    {
        //        addressBar.Text = uri;
        //    }
        //}
    }
}
