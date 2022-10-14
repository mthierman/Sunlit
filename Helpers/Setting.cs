﻿using System;
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

    //public Setting(string type, uint defWidth, uint defHeight, uint compWidth, uint compHeight)
    //{
    //    PresenterType = type;
    //    DefaultWidth = defWidth;
    //    DefaultHeight = defHeight;
    //    CompactWidth = compWidth;
    //    CompactHeight = compHeight;
    //}

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
        { return (Path.Combine(AppData, "Settings.xml")); }
    }

    public void Initialize()
    {
        if (!Directory.Exists(AppData))
        { Directory.CreateDirectory(AppData); };
    }

    public static void Save(Setting setting)
    {
        using var stream = File.Create(Filename);
        using var writer = new Utf8JsonWriter(stream);
        JsonSerializer.Serialize(writer, setting);
    }

    public static Setting Load()
    {
        byte[] bytes = File.ReadAllBytes(Filename);
        var span = new ReadOnlySpan<byte>(bytes);
        return JsonSerializer.Deserialize<Setting>(span);
    }
}