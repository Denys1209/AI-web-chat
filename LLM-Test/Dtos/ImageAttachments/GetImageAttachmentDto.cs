namespace LLM_Test.Dtos.ImageAttachments;

public record GetImageAttachmentDto
{
    public required Guid Id { get; init; }
    public required string Url { get; init; }
    public required string MimeType { get; init; }
}
