using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Persistence;

public interface IFileService
{
    Task<string> SaveFile(IFormFile file, string folderPath, CancellationToken cancellationToken = default);
    Task<bool> DeleteFile(string filePath);

    Task SaveAllFiles<TEntity, TModel>(TEntity entity, TModel model, string filePath);
    Task DeleteAllFiles<TEntity>(TEntity entity);
}