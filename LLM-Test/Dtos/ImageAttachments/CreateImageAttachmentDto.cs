namespace LLM_Test.Dtos.ImageAttachments;

public record CreateImageAttachmentDto
{
    public required byte[] Data { get; init; }
    public required string MimeType { get; init; }
}
