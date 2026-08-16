using LLM_Test.Constants;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.Users;

public record RegisterUserDto
{
    [MaxLength(NumberConstants.MaxLengthDisplayedName)]
    public required string DisplayedName { get; init; }

    [EmailAddress]
    public required string Gmail { get; init; }

    public required string Password { get; init; }
}
