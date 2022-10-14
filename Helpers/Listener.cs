using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;

namespace Todo;

public class Listener
{
    //private readonly UISettings _uiSettings = new();
    //MainWindow _mainWindow = new();

    private readonly UISettings _uiSettings = new();
    private readonly Theme _theme;
    private readonly MainWindow _window;
    private readonly Setting _setting;

    private void WindowThemeChanged(FrameworkElement sender, object args)
    {
        var color = _uiSettings.GetColorValue(UIColorType.Background);
        if (_theme._configurationSource != null)
        {
            _theme.SetConfigurationSourceTheme();
        }
        if (color.ToString() == "#FF000000")
        {
            Theme.SetWindowImmersiveDarkMode(_window.hWnd, true);
        }
        else
        {
            Theme.SetWindowImmersiveDarkMode(_window.hWnd, false);
        }
    }

    private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
    {
        if (_setting.PresenterType == "Compact")
        {
            _setting.CompactWidth = _window.appWindow.Size.Width;
            _setting.CompactHeight = _window.appWindow.Size.Height;
        }
        else
        {
            _setting.DefaultWidth = _window.appWindow.Size.Width;
            _setting.DefaultHeight = _window.appWindow.Size.Height;
        }
    }

    public void InitializeListener()
    {
        _window.Activated += WindowActivated;
        _window.Closed += WindowClosed;
        _window.SizeChanged += WindowSizeChanged;
        ((FrameworkElement)_window.Content).ActualThemeChanged += WindowThemeChanged;
    }

    private void WindowActivated(object sender, WindowActivatedEventArgs e)
    {
        if (_theme._configurationSource != null)
        {
            _theme._configurationSource.IsInputActive = e.WindowActivationState != WindowActivationState.Deactivated;
        }
    }

    private void WindowClosed(object sender, WindowEventArgs e)
    {
        if (_theme._acrylicController != null)
        {
            _theme._acrylicController.Dispose();
            _theme._acrylicController = null;
        }
        if (_theme._micaController != null)
        {
            _theme._micaController.Dispose();
            _theme._micaController = null;
        }
        _window.Activated -= WindowActivated;
        _window.Closed -= WindowClosed;
        _window.SizeChanged -= WindowSizeChanged;
        ((FrameworkElement)_window.Content).ActualThemeChanged -= WindowThemeChanged;
        _theme._configurationSource = null;
        //_setting.SaveWindow(_setting);
    }
}
