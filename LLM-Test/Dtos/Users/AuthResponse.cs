using LLM_Test.Constants;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.Users;

public record AuthResponse
{
    public required string DisplayedName { get; init; }

    public required string Gmail { get; init; }

    public required string Token { get; init; }

    public required Guid Id { get; init; }


}
