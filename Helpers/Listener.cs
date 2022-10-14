using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;

namespace Todo;

public class Listener
{
    UISettings _uiSettings = new();
    private readonly MainWindow _mainWindow;

    private void WindowThemeChanged(FrameworkElement sender, object args)
    {
        var color = _uiSettings.GetColorValue(UIColorType.Background);
        if (_mainWindow._theme._configurationSource != null)
        {
            _mainWindow._theme.SetConfigurationSourceTheme();
        }
        if (color.ToString() == "#FF000000")
        {
            Theme.SetWindowImmersiveDarkMode(_mainWindow.hWnd, true);
        }
        else
        {
            Theme.SetWindowImmersiveDarkMode(_mainWindow.hWnd, false);
        }
    }

    private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
    {
        if (_mainWindow._setting.PresenterType == "Compact")
        {
            _mainWindow._setting.CompactWidth = _mainWindow.appWindow.Size.Width;
            _mainWindow._setting.CompactHeight = _mainWindow.appWindow.Size.Height;
        }
        else
        {
            _mainWindow._setting.DefaultWidth = _mainWindow.appWindow.Size.Width;
            _mainWindow._setting.DefaultHeight = _mainWindow.appWindow.Size.Height;
        }
    }

    public void InitializeListener()
    {
        _mainWindow.Activated += WindowActivated;
        _mainWindow.Closed += WindowClosed;
        _mainWindow.SizeChanged += WindowSizeChanged;
        ((FrameworkElement)_mainWindow.Content).ActualThemeChanged += WindowThemeChanged;
    }

    private void WindowActivated(object sender, WindowActivatedEventArgs e)
    {
        if (_mainWindow._theme._configurationSource != null)
        {
            _mainWindow._theme._configurationSource.IsInputActive = e.WindowActivationState != WindowActivationState.Deactivated;
        }
    }

    private void WindowClosed(object sender, WindowEventArgs e)
    {
        if (_mainWindow._theme._acrylicController != null)
        {
            _mainWindow._theme._acrylicController.Dispose();
            _mainWindow._theme._acrylicController = null;
        }
        if (_mainWindow._theme._micaController != null)
        {
            _mainWindow._theme._micaController.Dispose();
            _mainWindow._theme._micaController = null;
        }
        _mainWindow.Activated -= WindowActivated;
        _mainWindow.Closed -= WindowClosed;
        _mainWindow.SizeChanged -= WindowSizeChanged;
        ((FrameworkElement)_mainWindow.Content).ActualThemeChanged -= WindowThemeChanged;
        _mainWindow._theme._configurationSource = null;
        //_setting.SaveWindow(_setting);
    }
}
