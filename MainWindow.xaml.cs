using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using WinRT.Interop;

namespace Todo;

public sealed partial class MainWindow : Window
{
    Presenter _presenter = new Presenter();
    Win32 _win32 = new Win32();

    public MainWindow()
    {
        InitializeComponent();

        InitializeListener();

        Title = "Todo";
        MyAppWindow.SetIcon("Assets/logo.ico");

        _presenter.Initialize(MyAppWindow);
        _win32.Initialize(WinHandle);
    }

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

    public void TogglePresenter(object sender, RoutedEventArgs e)
    {
        ToggleButton toggleButton = sender as ToggleButton;
        if (((ToggleButton)sender).IsChecked == false)
        {
            _presenter.DefaultPresenter(MyAppWindow);
            toggleButton.Content = "\uEE49";
        }
        else
        {
            _presenter.CompactPresenter(MyAppWindow);
            toggleButton.Content = "\uEE47";
        }
    }
}
