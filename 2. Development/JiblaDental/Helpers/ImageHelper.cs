using System.Collections.Concurrent;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Html;
using SixLabors.ImageSharp;

namespace JiblaDental.Helpers;

/// <summary>
/// Renders responsive &lt;img&gt; tags for uploaded images: srcset (served by ImageResizeMiddleware),
/// intrinsic width/height (prevents layout shift) and lazy/priority loading.
/// </summary>
public static class ImageHelper
{
    public static readonly int[] DefaultWidths = { 320, 480, 640, 768, 960, 1280, 1600 };

    private static readonly string[] ResizableExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
    private static readonly ConcurrentDictionary<string, int[]> Sizes = new(StringComparer.OrdinalIgnoreCase);
    private static string _webRoot = "";

    public static void Init(string webRootPath) => _webRoot = Path.GetFullPath(webRootPath);

    public static HtmlString Img(string? path, string? alt, string sizes = "100vw", int[]? widths = null,
        bool priority = false, string? cssClass = null, bool lazy = true)
    {
        if (string.IsNullOrWhiteSpace(path)) return HtmlString.Empty;

        var url = ToUrl(path);
        var size = GetSize(url);
        var sb = new StringBuilder("<img");
        sb.Append(" src=\"").Append(WebUtility.HtmlEncode(url)).Append('"');

        var srcSet = BuildSrcSet(url, size, widths ?? DefaultWidths);
        if (srcSet != null)
        {
            sb.Append(" srcset=\"").Append(WebUtility.HtmlEncode(srcSet)).Append('"');
            sb.Append(" sizes=\"").Append(WebUtility.HtmlEncode(sizes)).Append('"');
        }

        if (size != null) sb.Append(" width=\"").Append(size[0]).Append("\" height=\"").Append(size[1]).Append('"');
        sb.Append(" alt=\"").Append(WebUtility.HtmlEncode(alt ?? "")).Append('"');
        if (!string.IsNullOrEmpty(cssClass)) sb.Append(" class=\"").Append(WebUtility.HtmlEncode(cssClass)).Append('"');
        sb.Append(" decoding=\"async\"");
        if (priority) sb.Append(" fetchpriority=\"high\"");
        else if (lazy) sb.Append(" loading=\"lazy\"");
        sb.Append(" />");
        return new HtmlString(sb.ToString());
    }

    /// <summary>&lt;link rel="preload"&gt; for the LCP image so the browser fetches it before CSS/JS finish.</summary>
    public static HtmlString PreloadImage(string? path, string sizes = "100vw", int[]? widths = null)
    {
        if (string.IsNullOrWhiteSpace(path)) return HtmlString.Empty;

        var url = ToUrl(path);
        var srcSet = BuildSrcSet(url, GetSize(url), widths ?? DefaultWidths);
        var sb = new StringBuilder("<link rel=\"preload\" as=\"image\" fetchpriority=\"high\"");
        sb.Append(" href=\"").Append(WebUtility.HtmlEncode(url)).Append('"');
        if (srcSet != null)
        {
            sb.Append(" imagesrcset=\"").Append(WebUtility.HtmlEncode(srcSet)).Append('"');
            sb.Append(" imagesizes=\"").Append(WebUtility.HtmlEncode(sizes)).Append('"');
        }

        sb.Append(" />");
        return new HtmlString(sb.ToString());
    }

    /// <summary>URL of a resized copy (used for CSS background images).</summary>
    public static string Resized(string? path, int width)
    {
        if (string.IsNullOrWhiteSpace(path)) return "";
        var url = ToUrl(path);
        return IsResizable(url) ? url + "?w=" + width : url;
    }

    private static string ToUrl(string path) => "/" + path.TrimStart('/');

    private static bool IsResizable(string url) =>
        ResizableExtensions.Contains(Path.GetExtension(url).ToLowerInvariant());

    private static string? BuildSrcSet(string url, int[]? size, int[] widths)
    {
        if (size == null || !IsResizable(url)) return null;

        var parts = widths.Where(w => w < size[0]).OrderBy(w => w)
            .Select(w => url + "?w=" + w + " " + w + "w").ToList();
        if (parts.Count == 0) return null;

        parts.Add(url + "?f=auto " + size[0] + "w"); // original size, but as WebP when the browser accepts it
        return string.Join(", ", parts);
    }

    private static int[]? GetSize(string url)
    {
        if (string.IsNullOrEmpty(_webRoot)) return null;

        var size = Sizes.GetOrAdd(url, key =>
        {
            try
            {
                var full = Path.GetFullPath(Path.Combine(_webRoot, key.TrimStart('/')));
                if (!full.StartsWith(_webRoot, StringComparison.OrdinalIgnoreCase) || !File.Exists(full))
                    return Array.Empty<int>();

                var info = Image.Identify(full);
                return new[] { info.Width, info.Height };
            }
            catch
            {
                return Array.Empty<int>();
            }
        });
        return size.Length == 2 ? size : null;
    }
}
