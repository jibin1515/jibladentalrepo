using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Services;

namespace JiblaDental.Areas.Admin.Controllers;

/// <summary>
/// One-off tool to shrink the images that are already uploaded, keeping every file name, extension and folder
/// (so nothing stored in the database changes).
///   /admin/optimize-images            -> dry run: lists the possible savings
///   /admin/optimize-images?apply=true -> does it; originals are copied to Uploads/_backup first
/// </summary>
[Authorize]
[Area("Admin")]
public class ImageMaintenanceController : Controller
{
    private readonly IWebHostEnvironment _env;

    public ImageMaintenanceController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpGet("/admin/optimize-images")]
    public async Task<IActionResult> Optimize(bool apply = false)
    {
        var uploads = Path.Combine(_env.WebRootPath, "Uploads");
        if (!Directory.Exists(uploads)) return Content("No Uploads folder.", "text/plain");

        var report = new StringBuilder();
        report.AppendLine(apply ? "APPLYING (backups in Uploads/_backup)" : "DRY RUN - add ?apply=true to apply");
        long totalBefore = 0, totalAfter = 0;
        var changed = 0;

        foreach (var file in Directory.EnumerateFiles(uploads, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(uploads, file).Replace(Path.DirectorySeparatorChar, '/');
            if (relative.StartsWith("_cache/") || relative.StartsWith("_backup/") || !ImageOptimizer.IsSupported(file))
                continue;

            try
            {
                if (apply)
                {
                    var backup = Path.Combine(uploads, "_backup", relative);
                    Directory.CreateDirectory(Path.GetDirectoryName(backup)!);
                    if (!System.IO.File.Exists(backup)) System.IO.File.Copy(file, backup);
                }

                var (before, after) = await ImageOptimizer.OptimizeInPlace(file, apply);
                totalBefore += before;
                totalAfter += after;
                if (after < before)
                {
                    changed++;
                    report.AppendLine($"{relative}: {before / 1024} KB -> {after / 1024} KB");
                }
            }
            catch (Exception ex)
            {
                report.AppendLine($"{relative}: skipped ({ex.Message})");
            }
        }

        report.AppendLine();
        report.AppendLine($"{changed} files {(apply ? "optimized" : "can be optimized")}: " +
                          $"{totalBefore / 1024} KB -> {totalAfter / 1024} KB");
        return Content(report.ToString(), "text/plain");
    }
}
