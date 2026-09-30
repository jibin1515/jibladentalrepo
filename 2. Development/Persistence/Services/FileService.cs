using Application.Interfaces.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Persistence.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public FileService(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<string> SaveFile(IFormFile file, string folderPath, CancellationToken cancellationToken = default)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var forbiddenExtensions = new[] { ".exe", ".bat", ".cmd", ".sh", ".php", ".asp", ".aspx", ".cshtml", ".js", ".vbs", ".ps1", ".cgi", ".com", ".scr" };
        if (forbiddenExtensions.Contains(ext))
        {
            throw new InvalidOperationException("File format not allowed for upload.");
        }

        var fileNameWithOutExtension = Guid.NewGuid();
        var fileNameWithExtension = fileNameWithOutExtension + ext;

        var filePath = Path.Combine(folderPath, fileNameWithExtension);
        filePath = filePath.Replace("\\", "/");
        var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);
        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // Shrink big camera photos etc. in place (same name and format, so the stored path stays valid).
        // Never fail an upload because of this.
        try
        {
            await ImageOptimizer.OptimizeInPlace(fullPath);
        }
        catch
        {
            // keep the original file
        }

        return filePath;
    }

    public Task<bool> DeleteFile(string filePath)
    {
        var filepath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);
        if (!File.Exists(filepath)) return Task.FromResult(false);

        File.Delete(filepath);
        return Task.FromResult(true);
    }

    public async Task SaveAllFiles<TEntity, TModel>(TEntity entity, TModel model, string filePath)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        if (model == null) throw new ArgumentNullException(nameof(model));

        var properties = entity.GetType()
            .GetProperties()
            .Select(property => property.Name)
            .Where(x => x.EndsWith("Path"))
            .Select(x => x[..^4])
            .ToList();

        foreach (var property in properties)
        {
            var entityFilePathProperty = entity.GetType().GetProperty(property + "Path");
            var modelFilePathProperty = model.GetType().GetProperty(property + "Path");
            var modelFileProperty = model.GetType().GetProperty(property);

            if (entityFilePathProperty == null || modelFilePathProperty == null || modelFileProperty == null
                || entityFilePathProperty.PropertyType != typeof(string)
                || modelFilePathProperty.PropertyType != typeof(string)
                || modelFileProperty.PropertyType != typeof(IFormFile)) continue;

            var entityFilePath = entityFilePathProperty.GetValue(entity) as string;
            var modelFilePath = modelFilePathProperty.GetValue(model) as string;
            var modelFile = modelFileProperty.GetValue(model) as IFormFile;


            if ((modelFile != null || string.IsNullOrWhiteSpace(modelFilePath))
                && !string.IsNullOrWhiteSpace(entityFilePath))
            {
                await DeleteFile(entityFilePath);
                modelFilePathProperty.SetValue(model, null);
                entityFilePathProperty.SetValue(entity, null);
            }

            if (modelFile == null) continue;

            var path = await SaveFile(modelFile, filePath);
            modelFilePathProperty.SetValue(model, path);
            entityFilePathProperty.SetValue(entity, path);
        }
    }

    public async Task DeleteAllFiles<TEntity>(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var properties = entity.GetType()
            .GetProperties()
            .Select(property => property.Name)
            .Where(x => x.EndsWith("Path"))
            .Select(x => x[..^4])
            .ToList();

        foreach (var property in properties)
        {
            var entityFilePathProperty = entity.GetType().GetProperty(property + "Path");
            if (entityFilePathProperty == null || entityFilePathProperty.PropertyType != typeof(string)) continue;

            if (entityFilePathProperty.GetValue(entity) is string entityFilePath) await DeleteFile(entityFilePath);
        }
    }
}