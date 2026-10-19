using Chat;
using LLM_Test.Constants;
using LLM_Test.Dtos.ImageAttachments;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.Messages;

public record CreateMessageDto
{
    public required Guid UserId { get; init; }


    [MaxLength(NumberConstants.MaxLengthText)]
    public required string Text { get; init; }

    public string Thoughts { get; init; } = "";

    public required Roles Role { get; init; }

    public required IReadOnlyList<CreateImageAttachmentDto> ImageAttachments { get; init; }
}
