using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using WinRT.Interop;

namespace Calendar;

public sealed partial class MainWindow : Window
{
    public Setting setting = Setting.Load();

    public MainWindow()
    {
        var window = FetchAppWindow(this);
        Title = setting.Title;
        InitializeComponent();
        InitializeListener();
        InitializePresenter(window);
        InitializeDarkMode(this);
        InitializeMica();
        CheckPresenter();
    }

    public static IntPtr FetchWindowHandle(MainWindow appwindow)
    {
        return WindowNative.GetWindowHandle(appwindow);
    }

    public static WindowId FetchWindowId(MainWindow appwindow)
    {
        IntPtr handle = FetchWindowHandle(appwindow);
        return Win32Interop.GetWindowIdFromWindow(handle);
    }

    public static AppWindow FetchAppWindow(MainWindow appwindow)
    {
        WindowId id = FetchWindowId(appwindow);
        return AppWindow.GetFromWindowId(id);
    }
}
