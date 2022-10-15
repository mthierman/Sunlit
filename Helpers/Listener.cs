using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;

namespace Todo;

public sealed partial class MainWindow : Window
{
    private readonly UISettings _uiSettings = new();

    public void InitializeListener()
    {
        this.Activated += WindowActivated;
        this.Closed += WindowClosed;
        this.SizeChanged += WindowSizeChanged;
        ((FrameworkElement)this.Content).ActualThemeChanged += WindowThemeChanged;
    }

    private void WindowThemeChanged(FrameworkElement sender, object args)
    {
        SetStyles();
        var color = _uiSettings.GetColorValue(UIColorType.Background);
        if (_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
        if (color.ToString() == "#FF000000")
        {
            SetWindowImmersiveDarkMode(hWnd, true);
        }
        else
        {
            SetWindowImmersiveDarkMode(hWnd, false);
        }
    }

    private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
    {
        if (_setting.PresenterType == "Compact")
        {
            _setting.CompactWidth = appWindow.Size.Width;
            _setting.CompactHeight = appWindow.Size.Height;
        }
        else
        {
            _setting.DefaultWidth = appWindow.Size.Width;
            _setting.DefaultHeight = appWindow.Size.Height;
        }
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
        if (_acrylicController != null)
        {
            _acrylicController.Dispose();
            _acrylicController = null;
        }
        if (_micaController != null)
        {
            _micaController.Dispose();
            _micaController = null;
        }
        this.Activated -= WindowActivated;
        this.Closed -= WindowClosed;
        this.SizeChanged -= WindowSizeChanged;
        ((FrameworkElement)this.Content).ActualThemeChanged -= WindowThemeChanged;
        _configurationSource = null;
        Setting.Save(_setting);
    }
}
