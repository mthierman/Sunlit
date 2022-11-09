namespace Calendar;

public sealed class Settings
{
  public AppPresenter Presenter { get; set; }
  public DefaultWindow Default { get; set; }
  public CompactWindow Compact { get; set; }
  public readonly static string appdata = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mthierman", "calendar")).FullName;
  public readonly static string json = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mthierman", "calendar", "settings.json")).FullName;
  public readonly static JsonSerializerOptions Options = new() { WriteIndented = true };
  public Settings()
  {
    Presenter = new();
    Default = new();
    Compact = new();
  }
  public class AppPresenter
  {
    public string Type { get; set; } = "Default";
  }
  public class DefaultWindow
  {
    public int Width { get; set; } = 750;
    public int Height { get; set; } = 450;
  }
  public class CompactWindow
  {
    public int Width { get; set; } = 500;
    public int Height { get; set; } = 500;
  }
  public static Settings Load(string appdata, string json)
  {
    if (!Directory.Exists(appdata))
    { Directory.CreateDirectory(appdata); }
    if (!File.Exists(json))
    { using var stream = File.Create(json); }
    var settings = new Settings();
    if (File.Exists(json))
    {
      try
      {
        using var stream = File.OpenRead(json);
        settings = JsonSerializer.Deserialize<Settings>(stream);
        return settings;
      }
      catch { }
    }
    return settings;
  }
  public static void Save(string json, Settings settings)
  {
    using var stream = File.OpenWrite(json);
    JsonSerializer.Serialize(stream, settings, Options);
  }
  public static void Read(Settings settings)
  {
    Debug.Print(JsonSerializer.Serialize(settings, Options));
  }
}
