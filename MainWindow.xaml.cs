namespace Calendar;
public sealed partial class MainWindow : Window
{
  private readonly static string appdata = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mthierman", "calendar")).FullName;
  private readonly static string json = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mthierman", "calendar", "settings.json")).FullName;
  public Settings settings;
  public MainWindow()
  {
    InitializeComponent();

    Title = "Calendar";

    settings = Settings.Load(appdata, json);
    var window = FetchAppWindow(this);
    Presenter.InitializeWindow(window, settings);
    InitializeToggleButton();
    InitializeListener();
    InitializeDarkMode(this);
    InitializeMica();
  }
  public void InitializeToggleButton()
  {
    if (settings.Presenter.Type == "Default")
    { PresenterToggleButton.IsChecked = false; }
    else if (settings.Presenter.Type == "Compact")
    { PresenterToggleButton.IsChecked = true; }
  }
  public void PresenterToggleButtonChecked(object sender, RoutedEventArgs e)
  {
    settings.Presenter.Type = "Compact";
    var window = FetchAppWindow(this);
    Presenter.InitializeWindow(window, settings);
    //PresenterToggleButton.Content = Icons.Contract;
    PresenterToggleButtonIcon.Glyph = Icons.Contract;
  }
  public void PresenterToggleButtonUnchecked(object sender, RoutedEventArgs e)
  {
    settings.Presenter.Type = "Default";
    var window = FetchAppWindow(this);
    Presenter.InitializeWindow(window, settings);
    //PresenterToggleButton.Content = Icons.Expand;
    PresenterToggleButtonIcon.Glyph = Icons.Expand;
  }
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
}
