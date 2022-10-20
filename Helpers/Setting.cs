using System;
using System.IO;
using System.Text.Json;

namespace Calendar;

public sealed class Setting
{
    public string Type { get; } = nameof(Setting);
    public string Title { get; set; } = "Calendar";
    public string PresenterType { get; set; } = "Default";
    public int DefaultWidth { get; set; } = 800;
    public int DefaultHeight { get; set; } = 600;
    public int CompactWidth { get; set; } = 200;
    public int CompactHeight { get; set; } = 400;

    public static void Save(Setting setting)
    {
        DirectoryInfo appdata = new(_appData);
        FileInfo json = new(_json);
        if (!appdata.Exists)
        {
            Directory.CreateDirectory(appdata.FullName);
        }
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        using FileStream stream = File.Create(json.FullName);
        JsonSerializer.Serialize(stream, setting, options);
    }

    public static Setting Load()
    {
        FileInfo json = new(_json);
        Setting setting = new();
        if (json.Exists)
        {
            try
            {
                using FileStream stream = File.OpenRead(json.FullName);
                setting = JsonSerializer.Deserialize<Setting>(stream);
            }
            catch
            { }
        }
        return setting;
    }

    private static string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private static string _appData = Path.Combine(_localAppData, "mthierman", "calendar");
    private static string _json = Path.Combine(_appData, "Settings.json");
}
