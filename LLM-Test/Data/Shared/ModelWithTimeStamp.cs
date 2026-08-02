namespace LLM_Test.Data.Shared;

public class ModelWithTimeStamp : Model
{
    public DateTime CreatedAt { get; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
