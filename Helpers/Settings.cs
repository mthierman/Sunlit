//using Microsoft.UI.Xaml;
//using System;
//using Windows.Storage;

//namespace Todo;

//public partial class App : Application
//{
//    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
//    StorageFolder localFolder = ApplicationData.Current.LocalFolder;

//    public void SaveTestSetting()
//    {
//        localSettings.Values["TestSetting"] = "Hello";
//    }

//    public void LoadTestSetting()
//    {
//        Object value = localSettings.Values["TestSetting"];
//        System.Diagnostics.Debug.WriteLine(value);
//    }
//}