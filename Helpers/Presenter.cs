using Microsoft.UI.Windowing;
using System.IO;

namespace Todo;

public class Presenter
{
    Settings _settings = new Settings();
    Setting _setting = new Setting();
    private OverlappedPresenter _overlapped;
    private OverlappedPresenter _compact;

    public void Initialize(AppWindow appwindow)
    {
        if (File.Exists(_settings.Filename))
        { _setting = _settings.LoadWindow(); }
        else
        {
            _setting.DefaultWidth = 200;
            _setting.DefaultHeight = 200;
            _setting.CompactWidth = 400;
            _setting.CompactHeight = 400;
        }
        Resize(appwindow, _setting);
    }

    public void Resize(AppWindow appwindow, Setting setting)
    {
        appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = setting.DefaultWidth, Height = setting.DefaultHeight });
    }

    public void DefaultPresenter(AppWindow appwindow)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Default);
        _overlapped = appwindow.Presenter as OverlappedPresenter;
        _overlapped.IsAlwaysOnTop = false;
        _overlapped.IsMaximizable = true;
        _overlapped.IsMinimizable = true;

    }

    public void CompactPresenter(AppWindow appwindow)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        _compact = appwindow.Presenter as OverlappedPresenter;
        _compact.IsAlwaysOnTop = true;
        _compact.IsMaximizable = false;
        _compact.IsMinimizable = false;
    }
}
