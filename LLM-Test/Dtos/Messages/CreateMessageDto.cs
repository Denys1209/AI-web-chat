using Chat;
using LLM_Test.Constants;
using LLM_Test.Dtos.ImageAttachmented;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.Messages;

public record CreateMessageDto
{
    public required Guid UserId { get; init; }

    [Required, MaxLength(NumberConstants.MaxLengthText)] public required string Text { get; init; }

    public string Thoughts { get; init; } = "";

    [Required] public required Roles Role { get; init; }

    public IReadOnlyList<CreateImageAttachmentDto> ImageAttachments { get; init; } = [];

}
