using LLM_Test.Data;
using LLM_Test.Data.Entities;
using LLM_Test.Dtos.ImageAttachmented;
using LLM_Test.Services.ImageServices;
using Microsoft.EntityFrameworkCore;

namespace LLM_Test.Services.ImageAttachmentServices;

public class ImageAttachmentService : IImageAttachmentService
{
    private readonly IImageStorageService _storage;
    private readonly AppDbContext _db;


    public ImageAttachmentService(IImageStorageService storage, AppDbContext db)
    {
        _storage = storage;
        _db = db;
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

    public async Task<ImageAttached> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var image = await _db.ImageAttacheds.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (image == null)
        {
            throw new KeyNotFoundException($"Image with ID {id} not found.");
        }


        return image;
    }
}
