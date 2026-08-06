using Chat;
using LLM_Test.Dtos.ImageAttachmented;

namespace LLM_Test.Dtos.Messages;

public class GetMessageDto
{
    public required Guid Id { get; init; }

    public required string Text { get; init; }

    public string Thoughts { get; init; } = "";

    public required Roles Role {  get; init; }  

    public required ICollection<GetImageAttachmentDto> ImageAttachments { get; init; }
}
