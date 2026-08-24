namespace LLM_Test.Dtos.Threads;

public record GetThreadDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}
