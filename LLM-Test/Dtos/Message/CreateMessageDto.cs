using Chat;
using LLM_Test.Dtos.ImageAttachmented;

namespace LLM_Test.Dtos.Message;

public record CreateMessageDto
{
    public required Guid UserId { get; init; }

    public required string Text { get; init; }

    public required Roles Role { get; init; }

    public required IReadOnlyList<CreateImageAttachmentDto> ImageAttachments { get; init; }

}
