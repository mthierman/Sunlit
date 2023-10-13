namespace Sunlit;

public sealed partial class MainWindow : Window
{
    public SystemBackdropConfiguration _configurationSource;

    public static void SetDarkMode(MainWindow mainwindow)
    {
        var handle = Presenter.FetchWindowHandle(mainwindow);
        var window = Presenter.FetchAppWindow(mainwindow);
        SetWindowImmersiveDarkMode(handle, true);
    }

    public static void SetLightMode(MainWindow mainwindow)
    {
        var handle = Presenter.FetchWindowHandle(mainwindow);
        var window = Presenter.FetchAppWindow(mainwindow);
        SetWindowImmersiveDarkMode(handle, false);
    }

    public static void InitializeDarkMode(MainWindow mainwindow)
    {
        _ = ImportTheme.SetPreferredAppMode(PreferredAppMode.AllowDark);
        if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
        { SetDarkMode(mainwindow); }
        else
        { SetLightMode(mainwindow); }
    }

    public void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)Content).ActualTheme)
        {
            case ElementTheme.Dark: _configurationSource.Theme = SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: _configurationSource.Theme = SystemBackdropTheme.Light; break;
            case ElementTheme.Default: _configurationSource.Theme = SystemBackdropTheme.Default; break;
        }
    }

    private class ImportTheme
    {
        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);
    }

    private enum PreferredAppMode
    {
        Default,
        AllowDark,
        ForceDark,
        ForceLight,
        Max
    }

    private class ImportWindow
    {
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr handle, DWMWINDOWATTRIBUTE dwAttr, ref int pvAttr, int cbAttr);
    }

    private enum DWMWINDOWATTRIBUTE
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20
    }

    public static void SetWindowImmersiveDarkMode(IntPtr handle, bool enabled)
    {
        int isEnabled = enabled ? 1 : 0;
        _ = ImportWindow.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref isEnabled, sizeof(int));
    }
}
