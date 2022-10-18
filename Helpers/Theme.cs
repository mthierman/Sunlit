﻿using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Runtime.InteropServices;
using WinRT;

namespace Calendar;

internal sealed partial class MainWindow : Window
{
    private WindowsSystemDispatcherQueueHelper _wsdqHelper;
    public Microsoft.UI.Composition.SystemBackdrops.MicaController _micaController;
    //public Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController _acrylicController;
    public Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration _configurationSource;

    private class WindowsSystemDispatcherQueueHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DispatcherQueueOptions
        {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        [DllImport("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);
        object _dispatcherQueueController = null;
        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
            {
                return;
            }

            if (_dispatcherQueueController == null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;
                options.apartmentType = 2;
                CreateDispatcherQueueController(options, ref _dispatcherQueueController);
            }
        }
    }

    public void SetDarkMode(MainWindow mainwindow)
    {
        var handle = FetchWindowHandle(mainwindow);
        var window = FetchAppWindow(mainwindow);
        SetWindowImmersiveDarkMode(handle, true);
        window.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/dark.ico"));
    }

    public void SetLightMode(MainWindow mainwindow)
    {
        var handle = FetchWindowHandle(mainwindow);
        var window = FetchAppWindow(mainwindow);
        SetWindowImmersiveDarkMode(handle, false);
        window.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/light.ico"));
    }

    public void InitializeDarkMode(MainWindow mainwindow)
    {
        ImportTheme.SetPreferredAppMode(PreferredAppMode.AllowDark);
        if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
        { SetDarkMode(mainwindow); }
        else
        { SetLightMode(mainwindow); }
    }

    bool InitializeMica()
    {
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            _wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            _wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
            _configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
            _configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();
            _micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();
            _micaController.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;
            //m_micaController.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt;
            _micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            _micaController.SetSystemBackdropConfiguration(_configurationSource);
            return true;
        }
        return false;
    }

    //bool InitializeAcrylic()
    //{
    //    if (Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController.IsSupported())
    //    {
    //        _wsdqHelper = new WindowsSystemDispatcherQueueHelper();
    //        _wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
    //        _configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
    //        _configurationSource.IsInputActive = true;
    //        SetConfigurationSourceTheme();
    //        _acrylicController = new Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController();
    //        _acrylicController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
    //        _acrylicController.SetSystemBackdropConfiguration(_configurationSource);
    //        return true;
    //    }
    //    return false;
    //}

    public void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)Content).ActualTheme)
        {
            case ElementTheme.Dark: _configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: _configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
            case ElementTheme.Default: _configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
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
        public static extern int DwmSetWindowAttribute(IntPtr handle, DWMWINDOWATTRIBUTE dwAttr, ref int pvAttr, int cbAttr);
    }

    private enum DWMWINDOWATTRIBUTE
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20
    }

    public static void SetWindowImmersiveDarkMode(IntPtr handle, bool enabled)
    {
        int isEnabled = enabled ? 1 : 0;
        ImportWindow.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref isEnabled, sizeof(int));
    }
}
