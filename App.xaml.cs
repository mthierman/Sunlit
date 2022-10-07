using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Win32;
using Windows.UI.Xaml;
using System.Threading;

namespace WinUI_Todo
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/minimal_mode,msOverlayScrollbarWinStyleAnimation");
            //Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--enable-features=OverlayScrollbar,msOverlayScrollbarWinStyle:scrollbar_mode/full_mode,msOverlayScrollbarWinStyleAnimation");
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
            //new Thread(RunMessagePump).Start();
        }

        //void RunMessagePump()
        //{
        //    System.Diagnostics.Debug.WriteLine("Starting Message Pump!!!");
        //}

        private Window m_window;
    }
}
