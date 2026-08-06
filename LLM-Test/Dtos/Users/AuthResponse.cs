
namespace LLM_Test.Dtos.User;

public record AuthResponse
{
    public required string DisplayedName { get; init; }

    public required string Gmail { get; init; }

    public required string Token { get; init; }

    public required Guid Id { get; init; }

}
