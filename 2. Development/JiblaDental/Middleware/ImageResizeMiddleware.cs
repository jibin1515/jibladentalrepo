using System.Security.Cryptography;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace JiblaDental.Middleware;

/// <summary>
/// Serves resized copies of uploaded images: /Uploads/Folder/file.jpg?w=640.
/// Copies are generated once, cached on disk under Uploads/_cache and returned as WebP when the browser supports it.
/// Anything that does not qualify falls through to the normal static file handler (original file).
/// </summary>
public class ImageResizeMiddleware
{
    private static readonly HashSet<int> AllowedWidths = new() { 160, 240, 320, 480, 640, 768, 960, 1280, 1600 };

    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ImageResizeMiddleware> _logger;

    public ImageResizeMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ImageResizeMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        if (!HttpMethods.IsGet(request.Method)
            || !request.Path.StartsWithSegments("/Uploads", StringComparison.OrdinalIgnoreCase)
            || !int.TryParse(request.Query["w"].ToString(), out var width)
            || !AllowedWidths.Contains(width))
        {
            await _next(context);
            return;
        }

        var relative = Uri.UnescapeDataString(request.Path.Value ?? "").TrimStart('/');
        var extension = Path.GetExtension(relative).ToLowerInvariant();
        if (extension is not (".jpg" or ".jpeg" or ".png" or ".webp"))
        {
            await _next(context);
            return;
        }

        var webRoot = Path.GetFullPath(_env.WebRootPath);
        var uploadsRoot = Path.Combine(webRoot, "Uploads") + Path.DirectorySeparatorChar;
        var source = Path.GetFullPath(Path.Combine(webRoot, relative));
        if (!source.StartsWith(uploadsRoot, StringComparison.OrdinalIgnoreCase)
            || source.Contains(Path.DirectorySeparatorChar + "_cache" + Path.DirectorySeparatorChar,
                StringComparison.OrdinalIgnoreCase)
            || !File.Exists(source))
        {
            await _next(context);
            return;
        }

        try
        {
            var info = await Image.IdentifyAsync(source);
            if (info.Width <= width)
            {
                await _next(context);
                return;
            }

            var webp = request.Headers.Accept.ToString().Contains("image/webp", StringComparison.OrdinalIgnoreCase);
            var outputExtension = webp ? ".webp" : extension;
            var cachePath = await GetOrCreate(source, uploadsRoot, width, outputExtension);

            context.Response.ContentType = outputExtension switch
            {
                ".webp" => "image/webp",
                ".png" => "image/png",
                _ => "image/jpeg"
            };
            context.Response.Headers["Cache-Control"] = "public,max-age=31536000,immutable";
            context.Response.Headers["Vary"] = "Accept";
            await context.Response.SendFileAsync(cachePath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Image resize failed for {Path}; serving the original.", relative);
            if (!context.Response.HasStarted) await _next(context);
        }
    }

    private static async Task<string> GetOrCreate(string source, string uploadsRoot, int width, string extension)
    {
        var key = source + "|" + File.GetLastWriteTimeUtc(source).Ticks + "|" + width + "|" + extension;
        var name = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(key))) + extension;
        var cacheDirectory = Path.Combine(uploadsRoot, "_cache");
        var cachePath = Path.Combine(cacheDirectory, name);
        if (File.Exists(cachePath)) return cachePath;

        Directory.CreateDirectory(cacheDirectory);
        var temp = cachePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var image = await Image.LoadAsync(source))
            {
                image.Mutate(x => x.AutoOrient().Resize(width, 0));
                IImageEncoder encoder = extension switch
                {
                    ".webp" => new WebpEncoder { Quality = 75 },
                    ".png" => new PngEncoder(),
                    _ => new JpegEncoder { Quality = 78 }
                };
                await image.SaveAsync(temp, encoder);
            }

            File.Move(temp, cachePath, true);
        }
        finally
        {
            if (File.Exists(temp)) File.Delete(temp);
        }

        return cachePath;
    }
}
