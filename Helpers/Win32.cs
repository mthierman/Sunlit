using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace Todo;

public sealed partial class MainWindow : Window
{
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
