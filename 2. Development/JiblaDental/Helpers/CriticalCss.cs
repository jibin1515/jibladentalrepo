namespace JiblaDental.Helpers;

/// <summary>
/// Above-the-fold CSS for the home page (wwwroot/assets/dist/critical.min.css, built by 2. Development/tools).
/// Returns null when the file does not exist, in which case the layout falls back to the blocking stylesheet.
/// </summary>
public static class CriticalCss
{
    private static readonly object Sync = new();
    private static bool _loaded;
    private static string? _css;

    public static string? Get(string webRootPath)
    {
        if (_loaded) return _css;

        lock (Sync)
        {
            if (_loaded) return _css;

            var path = Path.Combine(webRootPath, "assets", "dist", "critical.min.css");
            _css = File.Exists(path) ? File.ReadAllText(path) : null;
            _loaded = true;
            return _css;
        }
    }
}
