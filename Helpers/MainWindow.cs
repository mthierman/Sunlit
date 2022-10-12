using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.UI.ViewManagement;
using WinRT.Interop;

namespace Todo;

public sealed partial class MainWindow : Window
{
    private void InitTheme()
    {
        InitialTheme();
        DefaultPresenter();
        ActivateListeners();
        TrySetMicaBackdrop();
        //TrySetAcrylicBackdrop();

        Settings.LoadWindow();
        Debug.Print(Settings.ToString());
        Debug.Print(Setting.DefaultWidth.ToString());
        Debug.Print(Setting.DefaultHeight.ToString());
        Debug.Print(Setting.CompactWidth.ToString());
        Debug.Print(Setting.DefaultHeight.ToString());
    }

    // COMPACT OVERLAY
    private OverlappedPresenter MyDefaultPresenter;
    private OverlappedPresenter MyCompactPresenter;

    Setting Setting = new Setting();
    Settings Settings = new Settings();

    private void DefaultPresenter()
    {
        MyAppWindow.SetPresenter(AppWindowPresenterKind.Default);
        MyDefaultPresenter = MyAppWindow.Presenter as OverlappedPresenter;
        MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = Setting.DefaultWidth, Height = Setting.DefaultHeight });
        MyDefaultPresenter.IsAlwaysOnTop = false;
        MyDefaultPresenter.IsMaximizable = true;
        MyDefaultPresenter.IsMinimizable = true;
    }

    private void CompactPresenter()
    {
        MyAppWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        MyCompactPresenter = MyAppWindow.Presenter as OverlappedPresenter;
        MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = Setting.CompactWidth, Height = Setting.CompactHeight });
        MyCompactPresenter.IsAlwaysOnTop = true;
        MyCompactPresenter.IsMaximizable = false;
        MyCompactPresenter.IsMinimizable = false;
    }

    private void SwitchPresenter_CompactOverlay(object sender, RoutedEventArgs e)
    {
        ToggleButton toggleButton = sender as ToggleButton;
        if (((ToggleButton)sender).IsChecked == false)
        {
            Setting.CompactWidth = MyAppWindow.Size.Width;
            Setting.CompactHeight = MyAppWindow.Size.Height;
            DefaultPresenter();
            toggleButton.Content = "\uEE49";
        }
        else
        {
            Setting.DefaultWidth = MyAppWindow.Size.Width;
            Setting.DefaultHeight = MyAppWindow.Size.Height;
            CompactPresenter();
            toggleButton.Content = "\uEE47";

        }
    }

    // LISTENER
    private readonly UISettings _uiSettings = new();

    private void WindowThemeChanged(FrameworkElement sender, object args)
    {
        if (m_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
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

    private void ActivateListeners()
    {
        this.Activated += WindowActivated;
        this.Closed += WindowClosed;
        ((FrameworkElement)this.Content).ActualThemeChanged += WindowThemeChanged;
    }

    private void WindowActivated(object sender, WindowActivatedEventArgs e)
    {
        if (m_configurationSource != null)
        {
            m_configurationSource.IsInputActive = e.WindowActivationState != WindowActivationState.Deactivated;
        }
    }

    private void WindowClosed(object sender, WindowEventArgs e)
    {
        if (m_acrylicController != null)
        {
            m_acrylicController.Dispose();
            m_acrylicController = null;
        }
        if (m_micaController != null)
        {
            m_micaController.Dispose();
            m_micaController = null;
        }
        this.Activated -= WindowActivated;
        this.Closed -= WindowClosed;
        ((FrameworkElement)this.Content).ActualThemeChanged -= WindowThemeChanged;
        m_configurationSource = null;
        Settings.SaveWindow(Setting.DefaultWidth, Setting.DefaultHeight, Setting.CompactWidth, Setting.CompactHeight);
    }

    // APP WINDOW ID
    private IntPtr WinHandle
    {
        get
        { return WindowNative.GetWindowHandle(this); }
    }

    private WindowId WinId
    {
        get
        { return Win32Interop.GetWindowIdFromWindow(WinHandle); }
    }

    private AppWindow MyAppWindow
    {
        get
        { return AppWindow.GetFromWindowId(WinId); }
    }

    // ACRYLIC
    //private WindowsSystemDispatcherQueueHelper m_wsdqHelper;
    //private Microsoft.UI.Composition.SystemBackdrops.MicaController m_micaController;
    //private Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController m_acrylicController;
    //private Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration m_configurationSource;

    //private class WindowsSystemDispatcherQueueHelper
    //{
    //    [StructLayout(LayoutKind.Sequential)]
    //    struct DispatcherQueueOptions
    //    {
    //        internal int dwSize;
    //        internal int threadType;
    //        internal int apartmentType;
    //    }

    //    [DllImport("CoreMessaging.dll")]
    //    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);
    //    object m_dispatcherQueueController = null;
    //    public void EnsureWindowsSystemDispatcherQueueController()
    //    {
    //        if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
    //        {
    //            return;
    //        }

    //        if (m_dispatcherQueueController == null)
    //        {
    //            DispatcherQueueOptions options;
    //            options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
    //            options.threadType = 2;
    //            options.apartmentType = 2;
    //            CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
    //        }
    //    }
    //}

    //bool TrySetMicaBackdrop()
    //{
    //    if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
    //    {
    //        m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
    //        m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
    //        m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
    //        m_configurationSource.IsInputActive = true;
    //        SetConfigurationSourceTheme();
    //        m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();
    //        m_micaController.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;
    //        //m_micaController.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt;
    //        m_micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
    //        m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
    //        return true;
    //    }
    //    return false;
    //}

    //bool TrySetAcrylicBackdrop()
    //{
    //    if (Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController.IsSupported())
    //    {
    //        m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
    //        m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
    //        m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
    //        m_configurationSource.IsInputActive = true;
    //        SetConfigurationSourceTheme();
    //        m_acrylicController = new Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController();
    //        m_acrylicController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
    //        m_acrylicController.SetSystemBackdropConfiguration(m_configurationSource);
    //        return true;
    //    }
    //    return false;
    //}

    //private void SetConfigurationSourceTheme()
    //{
    //    switch (((FrameworkElement)Content).ActualTheme)
    //    {
    //        case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
    //        case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
    //        case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
    //    }
    //}

    // WIN32 THEMING
    private void InitialTheme()
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
    }

    private class ImportTheme
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

    private class ImportWindow
    {
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttr, ref int pvAttr, int cbAttr);
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
}
