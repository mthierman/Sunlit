using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI_Todo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private AppWindow m_AppWindow;
        public MainWindow()
        {
            InitializeComponent();

            Title = "Test Title";
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            m_AppWindow = GetAppWindowForCurrentWindow();

            //var windowHandle = WindowNative.GetWindowHandle(this);
            //var windowID = Win32Interop.GetWindowIdFromWindow(windowHandle);
            //var appWindow = AppWindow.GetFromWindowId(windowID);
            //appWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);
            //AppTitleBar.Visibility = Visibility.Collapsed;

            //m_AppWindow = GetAppWindowForCurrentWindow();
            //if (AppWindowTitleBar.IsCustomizationSupported())
            //{
            //    var titleBar = m_AppWindow.TitleBar;
            //    //titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
            //    titleBar.ExtendsContentIntoTitleBar = true;
            //    SetTitleBar(AppTitleBar);
            //}
            //else
            //{
            //    AppTitleBar.Visibility = Visibility.Collapsed;
            //}
            //m_AppWindow = GetAppWindowForCurrentWindow();
            //m_AppWindow.Title = "App title";

            //SetTitleBarColors();

            //MyWebView.NavigationStarting += EnsureHttps;
        }
        private void SwtichPresenter_CompOverlay(object sender, RoutedEventArgs e)
        {
            m_AppWindow = GetAppWindowForCurrentWindow();

            if (((ToggleButton)sender).IsChecked == false)
            {
                m_AppWindow.SetPresenter(AppWindowPresenterKind.Default);
            }
            else
            {
                m_AppWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);
            }

            //if (((ToggleButton)sender).IsEnabled == true)
            //{
            //    m_AppWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);
            //}
            //if (((ToggleButton)sender).IsEnabled == false)
            //{
            //    m_AppWindow.SetPresenter(AppWindowPresenterKind.Default);
            //}
            //else
            //{
            //    m_AppWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
            //}
        }
        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }
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
        //private void myButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Uri targetUri = new Uri(addressBar.Text);
        //        MyWebView.Source = targetUri;
        //    }
        //    catch (FormatException ex)
        //    {
        //        // Incorrect address entered.
        //    }
        //}
    }
}
