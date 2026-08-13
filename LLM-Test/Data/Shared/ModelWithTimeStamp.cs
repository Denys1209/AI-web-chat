namespace LLM_Test.Data.Shared;

public class ModelWithTimeStamp : Model
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
