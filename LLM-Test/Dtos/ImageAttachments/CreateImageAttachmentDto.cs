using LLM_Test.Constants;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.ImageAttachments;

public record CreateImageAttachmentDto
{
    public required byte[] Data { get; init; }

    [MaxLength(NumberConstants.MaxLengthImageType)]
    public required string MimeType { get; init; }
}
