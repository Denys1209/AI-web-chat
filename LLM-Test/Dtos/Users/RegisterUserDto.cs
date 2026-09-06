using LLM_Test.Constants;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.User;

public record RegisterUserDto
{
    [Required, MaxLength(NumberConstants.MaxLengthDisplayedName)] public required string DisplayedName { get; init; }
    [Required, EmailAddress] public required string Gmail { get; init; }
    [Required] public required string Password { get; init; }

}
