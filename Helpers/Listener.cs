using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;

namespace Todo;

public sealed partial class MainWindow : Window
{
    private readonly UISettings _uiSettings = new();

    private void WindowThemeChanged(FrameworkElement sender, object args)
    {
        if (m_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
        var color = _uiSettings.GetColorValue(UIColorType.Background);
        if (color.ToString() == "#FF000000")
        {
            Win32.SetWindowImmersiveDarkMode(WinHandle, true);
        }
        else
        {
            Win32.SetWindowImmersiveDarkMode(WinHandle, false);
        }
    }

    public void InitializeListener()
    {
        this.Activated += WindowActivated;
        this.Closed += WindowClosed;
        ((FrameworkElement)this.Content).ActualThemeChanged += WindowThemeChanged;
    }

    private void WindowActivated(object sender, WindowActivatedEventArgs e)
    {
        if (m_configurationSource != null)
        {
            m_configurationSource.IsInputActive = e.WindowActivationState != WindowActivationState.Deactivated;
        }
    }

    private void WindowClosed(object sender, WindowEventArgs e)
    {
        if (m_acrylicController != null)
        {
            m_acrylicController.Dispose();
            m_acrylicController = null;
        }
        if (m_micaController != null)
        {
            m_micaController.Dispose();
            m_micaController = null;
        }
        this.Activated -= WindowActivated;
        this.Closed -= WindowClosed;
        ((FrameworkElement)this.Content).ActualThemeChanged -= WindowThemeChanged;
        m_configurationSource = null;
        //Settings.SaveWindow(Setting.DefaultWidth, Setting.DefaultHeight, Setting.CompactWidth, Setting.CompactHeight);
    }
}
