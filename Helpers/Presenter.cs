using Microsoft.UI.Windowing;
using System.IO;

namespace Todo;

public class Presenter
{
    private readonly MainWindow _mainWindow;

    private OverlappedPresenter _overlapped;
    private OverlappedPresenter _compact;

    public void Initialize()
    {
        if (File.Exists(Setting.Filename))
        { _mainWindow._setting = Setting.Load(); }
        else
        {
            Setting.Save(_mainWindow._setting);
        }
    }

    public void InitializePresenterType(AppWindow appwindow)
    {
        if (_mainWindow._setting.PresenterType == "Compact")
        { CompactPresenter(appwindow); }
        else
        { DefaultPresenter(appwindow); }
    }

    public void Resize(AppWindow appwindow)
    {
        if (_mainWindow._setting.PresenterType == "Compact")
        { appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = _mainWindow._setting.CompactWidth, Height = _mainWindow._setting.CompactHeight }); }
        else
        { appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = _mainWindow._setting.DefaultWidth, Height = _mainWindow._setting.DefaultHeight }); }
    }

    public void DefaultPresenter(AppWindow appwindow)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Default);
        _overlapped = appwindow.Presenter as OverlappedPresenter;
        _overlapped.IsAlwaysOnTop = false;
        _overlapped.IsMaximizable = true;
        _overlapped.IsMinimizable = true;
        _mainWindow._setting.PresenterType = "Default";
    }

    public void CompactPresenter(AppWindow appwindow)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        _compact = appwindow.Presenter as OverlappedPresenter;
        _compact.IsAlwaysOnTop = true;
        _compact.IsMaximizable = false;
        _compact.IsMinimizable = false;
        _mainWindow._setting.PresenterType = "Compact";
    }
}
