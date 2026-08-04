namespace LLM_Test.Dtos.User;

public record RegisterUserDto
{
    public required string DisplayedName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }

}
