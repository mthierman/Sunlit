using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace Todo;

public sealed partial class MainWindow : Window
{
    private void InitTheme()
    {
        InitialTheme();
        DefaultPresenter();
        ActivateListeners();
        TrySetMicaBackdrop();
        //TrySetAcrylicBackdrop();

        Settings.LoadWindow();
        Debug.Print(Settings.ToString());
        Debug.Print(Setting.DefaultWidth.ToString());
        Debug.Print(Setting.DefaultHeight.ToString());
        Debug.Print(Setting.CompactWidth.ToString());
        Debug.Print(Setting.DefaultHeight.ToString());
    }
}
