using Microsoft.UI.Windowing;
using System.IO;

namespace Todo;

public class Presenter
{
    Settings _settings = new Settings();
    public Setting _setting = new Setting();
    private OverlappedPresenter _overlapped;
    private OverlappedPresenter _compact;

    public void Initialize(AppWindow appwindow)
    {
        if (File.Exists(_settings.Filename))
        { _setting = _settings.LoadWindow(); }
        else
        {
            _setting.DefaultWidth = 800;
            _setting.DefaultHeight = 800;
            _setting.CompactWidth = 300;
            _setting.CompactHeight = 300;
            _setting.PresenterType = "Default";
            _settings.SaveWindow(_setting);
        }
    }

    public void InitializePresenterType(AppWindow appwindow)
    {
        if (_setting.PresenterType == "Compact")
        {
            CompactPresenter(appwindow);
            //MainWindow.CompactToggle.IsChecked = true;
        }
        else
        {
            DefaultPresenter(appwindow);
        }
    }

    public void Resize(AppWindow appwindow)
    {
        if (_setting.PresenterType == "Compact")
        {
            appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = _setting.CompactWidth, Height = _setting.CompactHeight });
        }
        else
        {
            appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = _setting.DefaultWidth, Height = _setting.DefaultHeight });
        }
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
