using Microsoft.UI.Xaml;

namespace Todo;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        Title = "Todo";
        MyAppWindow.SetIcon("Assets/logo.ico");
        InitializeComponent();
        InitTheme();
    }
}
