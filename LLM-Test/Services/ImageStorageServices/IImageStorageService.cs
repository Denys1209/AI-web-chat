namespace LLM_Test.Services.ImageServices;

public interface IImageStorageService
{
    public Task<string> SaveAsync(byte[] data, string mimeType, CancellationToken cancellationToken);
    public Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken)
    public Task DeleteAsync(string path, CancellationToken cancellationToken);
}
