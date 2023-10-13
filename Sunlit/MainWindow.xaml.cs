namespace Sunlit;

public sealed partial class MainWindow : Window
{
    Settings settings;

    public MainWindow()
    {
        InitializeComponent();
        SystemBackdrop = new MicaBackdrop();
        Title = "Sunlit";
        settings = Settings.Load(Settings.appdata, Settings.json);
        var window = Presenter.FetchAppWindow(this);
        window.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "icon.ico"));
        Presenter.InitializeWindow(window, settings);
        InitializePresenterToggleButton();
        InitializeListener();
        InitializeDarkMode(this);
    }

    public void InitializePresenterToggleButton()
    {
        if (settings.Presenter.Type == "Default")
        { PresenterToggleButton.IsChecked = false; }
        else if (settings.Presenter.Type == "Compact")
        { PresenterToggleButton.IsChecked = true; }
    }

    public void PresenterToggleButtonChecked(object sender, RoutedEventArgs e)
    {
        settings.Presenter.Type = "Compact";
        var window = Presenter.FetchAppWindow(this);
        Presenter.InitializeWindow(window, settings);
        PresenterToggleButtonIcon.Glyph = Icons.Contract;
    }

    public void PresenterToggleButtonUnchecked(object sender, RoutedEventArgs e)
    {
        settings.Presenter.Type = "Default";
        var window = Presenter.FetchAppWindow(this);
        Presenter.InitializeWindow(window, settings);
        PresenterToggleButtonIcon.Glyph = Icons.Expand;
    }
}
