﻿namespace Sunlit;

public class Presenter
{
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

    public static void InitializeWindow(AppWindow appwindow, Settings settings)
    {
        if (settings.Presenter.Type == "Default")
        {
            DefaultPresenter(appwindow, settings);
            appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = settings.Default.Width, Height = settings.Default.Height });
        }
        else if (settings.Presenter.Type == "Compact")
        {
            CompactPresenter(appwindow, settings);
            appwindow.Resize(new Windows.Graphics.SizeInt32 { Width = settings.Compact.Width, Height = settings.Compact.Height });
        }
    }

    public static void DefaultPresenter(AppWindow appwindow, Settings settings)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        OverlappedPresenter _overlapped = appwindow.Presenter as OverlappedPresenter;
        _overlapped.IsAlwaysOnTop = false;
        _overlapped.IsMaximizable = true;
        _overlapped.IsMinimizable = true;
        settings.Presenter.Type = "Default";
    }

    public static void CompactPresenter(AppWindow appwindow, Settings settings)
    {
        appwindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        OverlappedPresenter _compact = appwindow.Presenter as OverlappedPresenter;
        _compact.IsAlwaysOnTop = true;
        _compact.IsMaximizable = false;
        _compact.IsMinimizable = false;
        settings.Presenter.Type = "Compact";
    }
}
