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
    public string PresenterType;
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

    public string Filename
    {
        get
        { return (Path.Combine(AppData, "Settings.xml")); }
    }

    public void Initialize()
    {
        Debug.Print("Local App Data folder: {0}", LocalAppData);
        Debug.Print("App Data folder: {0}", AppData);
        Debug.Print("Settings file: {0}", Filename);
        if (!Directory.Exists(AppData))
        { Directory.CreateDirectory(AppData); };
    }

    public void SaveWindow(Setting setting)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Setting));
        StreamWriter writer = new StreamWriter(Filename);
        serializer.Serialize(writer, setting);
        writer.Close();
        Debug.Print("Successfuly saved settings");
    }

    public Setting LoadWindow()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Setting));
        FileStream reader = new FileStream(Filename, FileMode.Open);
        Setting Setting = new Setting();
        Setting = (Setting)serializer.Deserialize(reader);
        reader.Close();
        Debug.Print("Successfuly loaded settings");
        return Setting;
    }
}