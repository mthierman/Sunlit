using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        var options = new JsonSerializerOptions() { WriteIndented = true };
        using FileStream stream = File.Create(json.FullName);
        using Utf8JsonWriter writer = new(stream);
        JsonSerializer.Serialize(writer, setting, options);
    }

    public static Setting Load()
    {
        FileInfo json = new(_json);
        if (json.Exists)
        {
            var options = new JsonSerializerOptions() { NumberHandling = JsonNumberHandling.AllowReadingFromString };
            byte[] stream = File.ReadAllBytes(json.FullName);
            return JsonSerializer.Deserialize<Setting>(stream, options);
        }
        else
        {
            return new Setting();
        }
    }

    private static string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private static string _appData = Path.Combine(_localAppData, "mthierman", "calendar");
    private static string _json = Path.Combine(_appData, "Settings.json");
}
