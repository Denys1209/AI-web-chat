using LLM_Test.Data.Entities;
using LLM_Test.Dtos.ImageAttachmented;
using LLM_Test.Services.ImageServices;

namespace LLM_Test.Services.ImageAttachmentService;

public class ImageAttachmentService : IImageAttachmentService
{
    private readonly IImageStorageService _storage;

    public ImageAttachmentService(IImageStorageService storage)
    {
        _storage = storage;
    }


    public async Task<ImageAttached> CreateAsync(CreateImageAttachmentDto dto, Message message, CancellationToken cancellationToken)
    {
        var path = await _storage.SaveAsync(dto.Data, dto.MimeType, cancellationToken);


        return new ImageAttached
        {
            Path = path,
            Type = dto.MimeType,
            Message = message
        };
    }

    public Task DeleteAsync(ImageAttached image, CancellationToken cancellationToken)
    {
        return _storage.DeleteAsync(image.Path, cancellationToken);
    }
}
