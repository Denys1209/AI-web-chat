namespace LLM_Test.Dtos.ImageAttachmented;

public record GetImageAttachmentDto
{
    public required Guid Id { get; init; }
    public required string Url { get; init; }

    public required string MimeType { get; init; }
}
