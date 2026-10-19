using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.Users;

public record LoginUserDto
{
    [EmailAddress]
    public required string Gmail { get; init; }

    public required string Password { get; init; }
}
