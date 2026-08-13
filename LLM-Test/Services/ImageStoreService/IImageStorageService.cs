namespace LLM_Test.Services.ImageStoreService;

public interface IImageStorageService
{
    public Task<string> SaveAsync(byte[] data, string mineType, CancellationToken cancellationToken);

    public Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken);

    public Task DeleteAsync(string path, CancellationToken cancellationToken);
}
