namespace LLM_Test.Dtos.User;

public record LoginUserDto
{
    public required string Gmail { get; init; }

    public required string Password { get; init; }
}
