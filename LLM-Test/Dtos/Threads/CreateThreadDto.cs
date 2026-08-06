namespace LLM_Test.Dtos.Threads;

public record CreateThreadDto 
{
    public required Guid UserId { get; init; } 

    public required string Name { get; init; } 
}
