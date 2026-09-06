using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.User;

public record LoginUserDto
{
    [Required, EmailAddress] public required string Gmail { get; init; }

    [Required] public required string Password { get; init; }
}
