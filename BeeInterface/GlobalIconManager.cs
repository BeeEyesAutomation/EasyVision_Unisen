using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public static class GlobalIconManager
{
    private static Icon _icon;

    public static void Init(string fileName = "logo.ico", bool watch = true, int scanIntervalMs = 50)
    {
        var path = Path.IsPathRooted(fileName) ? fileName : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        _icon = TryLoadIcon(path);
        ApplyToAll();

        if (watch)
        {
            var t = new Timer { Interval = Math.Max(200, scanIntervalMs) };
            t.Tick += (s, e) => ApplyToAll();
            t.Start();
            Application.ApplicationExit += (s, e) => { try { t.Stop(); } catch { } };
        }
    }

    private static Icon TryLoadIcon(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    return new Icon(fs);
            }
        }
        catch { }
        return null;
    }

    public static void ApplyToAll()
    {
        if (_icon == null) return;
        try
        {
            foreach (Form f in Application.OpenForms)
                if (!f.IsDisposed && f.Icon != _icon) f.Icon = _icon;
        }
        catch { }
    }
}
