using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.IO;

namespace Calendar;

internal sealed partial class MainWindow : Window
{


    private OverlappedPresenter _overlapped;
    private OverlappedPresenter _compact;

    public void InitializePresenter(AppWindow appwindow)
    {
        if (File.Exists(Setting.Filename))
        { setting = Setting.Load(); }
        else
        { Setting.Save(setting); }
        if (setting.PresenterType == "Compact")
        { CompactPresenter(appwindow); }
        else
        { DefaultPresenter(appwindow); }
        Resize(appwindow);
    }

    public void Resize(AppWindow appwindow)
    {
        if (setting.PresenterType == "Compact")
        { appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = setting.CompactWidth, Height = setting.CompactHeight }); }
        else
        { appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = setting.DefaultWidth, Height = setting.DefaultHeight }); }
    }

    public void DefaultPresenter(AppWindow appwindow)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Default);
        _overlapped = appwindow.Presenter as OverlappedPresenter;
        _overlapped.IsAlwaysOnTop = false;
        _overlapped.IsMaximizable = true;
        _overlapped.IsMinimizable = true;
        setting.PresenterType = "Default";
    }

    public void CompactPresenter(AppWindow appwindow)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        _compact = appwindow.Presenter as OverlappedPresenter;
        _compact.IsAlwaysOnTop = true;
        _compact.IsMaximizable = false;
        _compact.IsMinimizable = false;
        setting.PresenterType = "Compact";
    }

    private void CheckPresenter()
    {
        if (setting.PresenterType == "Compact")
        {
            PresenterToggleButton.IsChecked = true;
        }
        else
        {
            PresenterToggleButton.IsChecked = false;
        }
    }

    public void PresenterToggle(object sender, RoutedEventArgs e)
    {
        if (((ToggleButton)sender).IsChecked == false)
        {
            DefaultPresenter(appWindow);
            Resize(appWindow);
            PresenterToggleButton.Content = "\uEE49";
        }
        else
        {
            CompactPresenter(appWindow);
            Resize(appWindow);
            PresenterToggleButton.Content = "\uEE47";
        }
    }
}
