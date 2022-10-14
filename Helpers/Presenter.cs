using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.IO;

namespace Todo;

public sealed partial class MainWindow : Window
{
    private OverlappedPresenter _overlapped;
    private OverlappedPresenter _compact;

    public void InitializePresenter()
    {
        if (File.Exists(Setting.Filename))
        { _setting = Setting.Load(); }
        else
        {
            Setting.Save(_setting);
        }
    }

    public void InitializePresenterType(AppWindow appwindow)
    {
        if (_setting.PresenterType == "Compact")
        { CompactPresenter(appwindow); }
        else
        { DefaultPresenter(appwindow); }
    }

    public void Resize(AppWindow appwindow)
    {
        if (_setting.PresenterType == "Compact")
        { appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = _setting.CompactWidth, Height = _setting.CompactHeight }); }
        else
        { appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = _setting.DefaultWidth, Height = _setting.DefaultHeight }); }
    }

    public void DefaultPresenter(AppWindow appwindow)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Default);
        _overlapped = appwindow.Presenter as OverlappedPresenter;
        _overlapped.IsAlwaysOnTop = false;
        _overlapped.IsMaximizable = true;
        _overlapped.IsMinimizable = true;
        _setting.PresenterType = "Default";
    }

    public void CompactPresenter(AppWindow appwindow)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        _compact = appwindow.Presenter as OverlappedPresenter;
        _compact.IsAlwaysOnTop = true;
        _compact.IsMaximizable = false;
        _compact.IsMinimizable = false;
        _setting.PresenterType = "Compact";
    }
}
