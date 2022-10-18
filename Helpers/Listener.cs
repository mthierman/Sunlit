using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;

namespace Calendar;

internal sealed partial class MainWindow : Window
{
    private readonly UISettings _uiSettings = new();

    public void InitializeListener()
    {
        Activated += WindowActivated;
        Closed += WindowClosed;
        ((FrameworkElement)Content).ActualThemeChanged += WindowThemeChanged;
        SizeChanged += WindowSizeChanged;
    }

    private void WindowActivated(object sender, WindowActivatedEventArgs e)
    {
        if (_configurationSource != null)
        {
            _configurationSource.IsInputActive = e.WindowActivationState != WindowActivationState.Deactivated;
        }
    }

    private void WindowClosed(object sender, WindowEventArgs e)
    {
        //if (_acrylicController != null)
        //{
        //    _acrylicController.Dispose();
        //    _acrylicController = null;
        //}
        if (_micaController != null)
        {
            _micaController.Dispose();
            _micaController = null;
        }
        Activated -= WindowActivated;
        Closed -= WindowClosed;
        SizeChanged -= WindowSizeChanged;
        ((FrameworkElement)Content).ActualThemeChanged -= WindowThemeChanged;
        _configurationSource = null;
        Setting.Save(setting);
    }

    private void WindowThemeChanged(FrameworkElement sender, object args)
    {
        if (_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
        switch (((FrameworkElement)Content).ActualTheme)
        {
            case ElementTheme.Dark: SetDarkMode(this); break;
            case ElementTheme.Light: SetLightMode(this); break;
        }
    }

    private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
    {
        var window = FetchAppWindow(this);
        if (setting.PresenterType == "Compact")
        {
            setting.CompactWidth = window.Size.Width;
            setting.CompactHeight = window.Size.Height;
        }
        else
        {
            setting.DefaultWidth = window.Size.Width;
            setting.DefaultHeight = window.Size.Height;
        }
    }
}
