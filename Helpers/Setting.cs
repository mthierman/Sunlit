using System;
using System.IO;
using System.Text.Json;

namespace Todo;

public class Setting
{
    public string PresenterType { get; set; } = "Default";
    public int DefaultWidth { get; set; } = 800;
    public int DefaultHeight { get; set; } = 600;
    public int CompactWidth { get; set; } = 200;
    public int CompactHeight { get; set; } = 400;

    public static string LocalAppData
    {
        get
        { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); }
    }

    public static string AppData
    {
        get
        { return Path.Combine(LocalAppData, "mthierman", "todo"); }
    }

    public static string Filename
    {
        get
        { return (Path.Combine(AppData, "Settings.json")); }
    }

    public static void Initialize()
    {
        if (!Directory.Exists(AppData))
        { Directory.CreateDirectory(AppData); };
    }

    public static void Save(Setting setting)
    {
        using FileStream stream = File.Create(Filename);
        using Utf8JsonWriter writer = new(stream);
        JsonSerializer.Serialize(writer, setting);
    }

    public static Setting Load()
    {
        byte[] stream = File.ReadAllBytes(Filename);
        return JsonSerializer.Deserialize<Setting>(stream);
    }
}
