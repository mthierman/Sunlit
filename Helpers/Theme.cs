using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using WinRT;

namespace Todo;

public sealed partial class MainWindow : Window
{
    private WindowsSystemDispatcherQueueHelper _wsdqHelper;
    public Microsoft.UI.Composition.SystemBackdrops.MicaController _micaController;
    public Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController _acrylicController;
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

    public void InitializeDarkMode(IntPtr hWnd)
    {
        ImportTheme.SetPreferredAppMode(PreferredAppMode.AllowDark);
        if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
        {
            SetWindowImmersiveDarkMode(hWnd, true);
        }
        else
        {
            SetWindowImmersiveDarkMode(hWnd, false);
        }
    }

    //public void SetStyles()
    //{
    //    var color = _uiSettings.GetColorValue(UIColorType.Background);
    //    if (color.ToString() == "#FF000000")
    //    {
    //        Application.Current.Resources["ToggleButtonBackground"] = ColorHelper.FromArgb(255, 255, 255, 255);
    //        Application.Current.Resources["ToggleButtonBackgroundChecked"] = ColorHelper.FromArgb(255, 255, 255, 255);
    //    }
    //    else
    //    {
    //        Application.Current.Resources["ToggleButtonBackground"] = ColorHelper.FromArgb(255, 255, 255, 255);
    //        Application.Current.Resources["ToggleButtonBackgroundChecked"] = ColorHelper.FromArgb(255, 255, 255, 255);
    //    }
    //}

    public void SetStyles()
    {
        //var color = _uiSettings.GetColorValue(UIColorType.Background);
        if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
        {
            Application.Current.Resources["ToggleButtonBackground"] = ColorHelper.FromArgb(0, 255, 255, 255);
            Application.Current.Resources["ToggleButtonBackgroundChecked"] = ColorHelper.FromArgb(0, 255, 255, 255);
        }
        else
        {
            Application.Current.Resources["ToggleButtonBackground"] = ColorHelper.FromArgb(255, 255, 255, 255);
            Application.Current.Resources["ToggleButtonBackgroundChecked"] = ColorHelper.FromArgb(255, 255, 255, 255);
        }
    }

    bool InitializeMicaAcrylic()
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
        else if (Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController.IsSupported())
        {
            _wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            _wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
            _configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
            _configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();
            _acrylicController = new Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController();
            _acrylicController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            _acrylicController.SetSystemBackdropConfiguration(_configurationSource);
            return true;
        }
        return false;
    }

    public void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)this.Content).ActualTheme)
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
        public static extern int DwmSetWindowAttribute(IntPtr hWnd, DWMWINDOWATTRIBUTE dwAttr, ref int pvAttr, int cbAttr);
    }

    private enum DWMWINDOWATTRIBUTE
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20
    }

    public static void SetWindowImmersiveDarkMode(IntPtr hWnd, bool enabled)
    {
        int isEnabled = enabled ? 1 : 0;
        int result = ImportWindow.DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref isEnabled, sizeof(int));
        if (result != 0) throw new Win32Exception(result);
    }
}
