
using Microsoft.AspNetCore.Hosting;

namespace LLM_Test.Services.ImageStoreService;

public class LocalImageStorageService : IImageStorageService
{

    private readonly string _rootPath;

    private static readonly Dictionary<string, string> _mimeTypeToExtension = new()
    {
        { "image/jpeg", ".jpg" },
        { "image/png", ".png" },
        { "image/webp", ".webp" }
    };

    public LocalImageStorageService(IWebHostEnvironment env)
    {
        _rootPath = Path.Combine(env.ContentRootPath, "Uploads", "Images");
        Directory.CreateDirectory(_rootPath);
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(_rootPath, path);
        if (File.Exists(fullPath))         {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken)
    {
        return File.ReadAllBytesAsync(Path.Combine(_rootPath, path), cancellationToken);
    }

    public async Task<string> SaveAsync(byte[] data, string mineType, CancellationToken cancellationToken)
    {
        if (!_mimeTypeToExtension.TryGetValue(mineType, out var extension))
        {
            throw new ArgumentException($"Unsupported MIME type: {mineType}");
        }

        var fileName = $"{Guid.CreateVersion7()}{extension}";
        await File.WriteAllBytesAsync(Path.Combine(_rootPath, fileName), data, cancellationToken);

        return fileName;
    }
}
