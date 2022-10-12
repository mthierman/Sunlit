using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace Todo;

public sealed partial class MainWindow : Window
{
    private OverlappedPresenter MyDefaultPresenter;
    private OverlappedPresenter MyCompactPresenter;

    Setting Setting = new Setting();
    Settings Settings = new Settings();

    private void DefaultPresenter()
    {
        MyAppWindow.SetPresenter(AppWindowPresenterKind.Default);
        MyDefaultPresenter = MyAppWindow.Presenter as OverlappedPresenter;
        MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = Setting.DefaultWidth, Height = Setting.DefaultHeight });
        MyDefaultPresenter.IsAlwaysOnTop = false;
        MyDefaultPresenter.IsMaximizable = true;
        MyDefaultPresenter.IsMinimizable = true;
    }

    private void CompactPresenter()
    {
        MyAppWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
        MyCompactPresenter = MyAppWindow.Presenter as OverlappedPresenter;
        MyAppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = Setting.CompactWidth, Height = Setting.CompactHeight });
        MyCompactPresenter.IsAlwaysOnTop = true;
        MyCompactPresenter.IsMaximizable = false;
        MyCompactPresenter.IsMinimizable = false;
    }

    private void SwitchPresenter_CompactOverlay(object sender, RoutedEventArgs e)
    {
        ToggleButton toggleButton = sender as ToggleButton;
        if (((ToggleButton)sender).IsChecked == false)
        {
            Setting.CompactWidth = MyAppWindow.Size.Width;
            Setting.CompactHeight = MyAppWindow.Size.Height;
            DefaultPresenter();
            toggleButton.Content = "\uEE49";
        }
        else
        {
            Setting.DefaultWidth = MyAppWindow.Size.Width;
            Setting.DefaultHeight = MyAppWindow.Size.Height;
            CompactPresenter();
            toggleButton.Content = "\uEE47";

        }
    }
}
