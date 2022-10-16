using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using WinRT.Interop;

namespace Todo;

public sealed partial class MainWindow : Window
{
    public Setting setting = new();

    public MainWindow()
    {
        InitializeComponent();

        Title = "Todo";
        appWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/logo.ico"));

        InitializeListener();
        InitializePresenter(appWindow);
        InitializeDarkMode(hWnd);
        InitializeMica();
        CheckPresenter();
    }

    public IntPtr hWnd
    {
        get
        { return WindowNative.GetWindowHandle(this); }
    }

    public WindowId wndId
    {
        get
        { return Win32Interop.GetWindowIdFromWindow(hWnd); }
    }

    public AppWindow appWindow
    {
        get
        { return AppWindow.GetFromWindowId(wndId); }
    }
}
