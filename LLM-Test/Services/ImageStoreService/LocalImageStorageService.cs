
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
        _rootPath = rootPath;
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> SaveAsync(byte[] data, string mineType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
