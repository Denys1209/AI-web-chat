using Chat;
using LLM_Test.Dtos.ImageAttachments;

namespace LLM_Test.Dtos.Messages;

public record GetMessageDto
{
    public required Guid Id { get; init; }

    public required string Text { get; init; }

    public string Thoughts { get; init; } = "";

    public required Roles Role { get; init; } 

    public required ICollection<GetImageAttachmentDto> ImageAttachmentDtos { get; init; }
}
