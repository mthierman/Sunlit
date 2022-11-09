namespace Calendar;

public sealed partial class App : Application
{
  public App()
  {
    InitializeComponent();
  }
  protected override void OnLaunched(LaunchActivatedEventArgs args)
  {
    MainWindow _appWindow = new();
    _appWindow.Activate();
  }
}
