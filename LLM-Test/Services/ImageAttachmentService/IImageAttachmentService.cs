using LLM_Test.Data.Entities;
using LLM_Test.Dtos.ImageAttachmented;

namespace LLM_Test.Services.ImageAttachmentService;

public interface IImageAttachmentService
{
    public Task<ImageAttached> CreateAsync(CreateImageAttachmentDto dto, Message message, CancellationToken cancellationToken);
    public Task DeleteAsync(ImageAttached image, CancellationToken cancellationToken);
}
