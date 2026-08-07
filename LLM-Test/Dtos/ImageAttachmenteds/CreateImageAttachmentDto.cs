namespace LLM_Test.Dtos.ImageAttachmented;

public record CreateImageAttachmentDto
{
    public required byte[] Data { get; init; }
    public required string MimeType { get; init; }
}
