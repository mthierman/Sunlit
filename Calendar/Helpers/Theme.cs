namespace Calendar;

public sealed partial class MainWindow : Window
{
    private WindowsSystemDispatcherQueueHelper _wsdqHelper;
    public MicaController _micaController;
    public SystemBackdropConfiguration _configurationSource;
    private class WindowsSystemDispatcherQueueHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DispatcherQueueOptions
        {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        [DllImport("CoreMessaging.dll", PreserveSig = false)]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);
        object _dispatcherQueueController = null;
        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
            {
                return;
            }

            if (_dispatcherQueueController == null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;
                options.apartmentType = 2;
                _ = CreateDispatcherQueueController(options, ref _dispatcherQueueController);
            }
        }
    }
    public static void SetDarkMode(MainWindow mainwindow)
    {
        var handle = Presenter.FetchWindowHandle(mainwindow);
        var window = Presenter.FetchAppWindow(mainwindow);
        SetWindowImmersiveDarkMode(handle, true);
        window.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "Dark.ico"));
    }
    public static void SetLightMode(MainWindow mainwindow)
    {
        var handle = Presenter.FetchWindowHandle(mainwindow);
        var window = Presenter.FetchAppWindow(mainwindow);
        SetWindowImmersiveDarkMode(handle, false);
        window.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "Light.ico"));
    }
    public static void InitializeDarkMode(MainWindow mainwindow)
    {
        _ = ImportTheme.SetPreferredAppMode(PreferredAppMode.AllowDark);
        if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
        { SetDarkMode(mainwindow); }
        else
        { SetLightMode(mainwindow); }
    }
    bool InitializeMica()
    {
        if (MicaController.IsSupported())
        {
            _wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            _wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
            _configurationSource = new SystemBackdropConfiguration
            {
                IsInputActive = true
            };
            SetConfigurationSourceTheme();
            _micaController = new MicaController
            {
                Kind = MicaKind.Base
                //Kind = MicaKind.BaseAlt
            };
            _micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            _micaController.SetSystemBackdropConfiguration(_configurationSource);
            return true;
        }
        return false;
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
