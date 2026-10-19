using LLM_Test.Constants;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.Threads;

public record CreateThreadDto
{
    public required Guid UserId { get; init; }

    [MaxLength(NumberConstants.MaxLengthTheadName)]
    public required string Name { get; init; }
}
