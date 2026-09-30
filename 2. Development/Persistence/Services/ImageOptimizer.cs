using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;


namespace Persistence.Services;


/// <summary>
/// Makes uploaded JPEG/PNG files smaller <b>in place</b>: same file name, same format, same path (so the value stored
/// in the database stays valid). Oversized images are scaled down to <see cref="MaxWidth"/>, JPEGs are re-encoded at
/// quality 80 and PNGs with maximum compression. A file is only replaced when the result is clearly smaller.
/// </summary>
public static class ImageOptimizer
{
    public const int MaxWidth = 2000;
    private const double MinimumGain = 0.10; // replace only when at least 10% smaller

    public static bool IsSupported(string path) =>
        Path.GetExtension(path).ToLowerInvariant() is ".jpg" or ".jpeg" or ".png";

    /// <returns>Size before and after (equal when nothing was changed).</returns>
    public static async Task<(long Before, long After)> OptimizeInPlace(string path, bool apply = true)
    {
        var before = new FileInfo(path).Length;
        if (!IsSupported(path)) return (before, before);

        var extension = Path.GetExtension(path).ToLowerInvariant();
        using var image = await Image.LoadAsync(path);
        image.Mutate(x =>
        {
            x.AutoOrient();
            if (image.Width > MaxWidth) x.Resize(MaxWidth, 0);
        });

        using var buffer = new MemoryStream();
        if (extension == ".png")
            await image.SaveAsync(buffer, new PngEncoder
            {
                CompressionLevel = PngCompressionLevel.BestCompression,
                FilterMethod = PngFilterMethod.Adaptive
            });
        else
            await image.SaveAsync(buffer, new JpegEncoder { Quality = 80 });

        var after = buffer.Length;
        if (after >= before * (1 - MinimumGain)) return (before, before);
        if (!apply) return (before, after);

        var temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await File.WriteAllBytesAsync(temp, buffer.ToArray());
            File.Move(temp, path, true);
        }
        finally
        {
            if (File.Exists(temp)) File.Delete(temp);
        }

        return (before, after);
    }
}
