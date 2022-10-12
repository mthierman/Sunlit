using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Todo;

[XmlRoot(Namespace = "todo",
    ElementName = "Settings",
    DataType = "string",
    IsNullable = true)]

public class Setting
{
    public int DefaultWidth;
    public int DefaultHeight;
    public int CompactWidth;
    public int CompactHeight;
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

    public void Check()
    {
        if (!Directory.Exists(AppData))
        { Directory.CreateDirectory(AppData); };
    }

    public void SaveWindow(int DefaultWidth, int DefaultHeight, int CompactWidth, int CompactHeight)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Setting));
        StreamWriter writer = new StreamWriter(Filename);
        Setting Setting = new Setting();
        Setting.DefaultWidth = DefaultWidth;
        Setting.DefaultHeight = DefaultHeight;
        Setting.CompactWidth = CompactWidth;
        Setting.CompactHeight = CompactWidth;
        serializer.Serialize(writer, Setting);
        writer.Close();
    }

    public void LoadWindow()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Setting));
        FileStream fs = new FileStream(Filename, FileMode.Open);
        Setting Setting;
        Setting = (Setting)serializer.Deserialize(fs);
        fs.Close();
    }
}