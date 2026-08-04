using Chat;
using LLM_Test.Dtos.ImageAttachmented;

namespace LLM_Test.Dtos.Message;

public class GetMessageDto
{
    public required Guid Id { get; init; }

    public required string Text { get; init; }

    public required Roles Role {  get; init; }  

    public required IReadOnlyCollection<GetImageAttachmentDto> ImageAttachments { get; init; }
}
