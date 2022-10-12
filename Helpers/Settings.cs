using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Todo;

[XmlRoot(Namespace = "todo",
    ElementName = "Settings",
    DataType = "string",
    IsNullable = false)]

public class Setting
{
    public string test;
}

public class Settings
{

    private string LocalAppData
    {
        get
        { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); }
    }

    private string AppData
    {
        get
        { return Path.Combine(LocalAppData, "mthierman", "todo"); }
    }

    private string Filename
    {
        get
        { return (Path.Combine(AppData, "Settings.xml")); }
    }

    public void Print()
    {
        Debug.Print("Local App Data folder: {0}", LocalAppData);
        Debug.Print("App Data folder: {0}", AppData);
        Debug.Print("Settings file: {0}", Filename);
    }

    public void Save()
    {
        if (!Directory.Exists(AppData))
        {
            Directory.CreateDirectory(AppData);
        };
        StreamWriter writer = new StreamWriter(Filename);
        XmlSerializer serializer = new XmlSerializer(typeof(Setting));

        Setting Setting = new Setting();
        Setting.test = "test";
        serializer.Serialize(writer, Setting);
        writer.Close();
    }

    public void Load()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Setting));
        FileStream fs = new FileStream(Filename, FileMode.Open);
        Setting Setting;
        Setting = (Setting)serializer.Deserialize(fs);
        Debug.Print(Setting.test);
    }
}