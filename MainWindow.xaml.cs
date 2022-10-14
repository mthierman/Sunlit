using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.IO;
using WinRT.Interop;

namespace Todo;

public sealed partial class MainWindow : Window
{
    Presenter _presenter = new Presenter();
    Listener _listeners = new Listener();
    Theme _theme = new Theme();
    private readonly Setting _setting;

    public MainWindow()
    {
        InitializeComponent();

        Title = "Todo";
        appWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/logo.ico"));

        _listeners.InitializeListener();

        _presenter.Initialize(appWindow);
        _presenter.InitializePresenterType(appWindow);
        _presenter.Resize(appWindow);

        _theme.DarkMode(hWnd);

        CompactToggleCheck();
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

    private void CompactToggleCheck()
    {
        if (_setting.PresenterType == "Compact")
        {
            CompactToggle.IsChecked = true;
        }
        else
        {
            CompactToggle.IsChecked = false;
        }
    }

    public void TogglePresenter(object sender, RoutedEventArgs e)
    {
        ToggleButton toggleButton = sender as ToggleButton;
        if (((ToggleButton)sender).IsChecked == false)
        {
            _presenter.DefaultPresenter(appWindow);
            _presenter.Resize(appWindow);
            toggleButton.Content = "\uEE49";
        }
        else
        {
            _presenter.CompactPresenter(appWindow);
            _presenter.Resize(appWindow);
            toggleButton.Content = "\uEE47";
        }
    }
}
