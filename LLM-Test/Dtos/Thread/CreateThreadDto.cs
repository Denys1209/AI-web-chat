namespace LLM_Test.Dtos.Thread;

public record CreateThreadDto 
{
    public required Guid UserId { get; init; } 

    public required string Name { get; init; } 
}
