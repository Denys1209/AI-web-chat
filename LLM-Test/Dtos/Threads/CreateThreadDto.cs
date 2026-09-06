using LLM_Test.Constants;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Dtos.Threads;

public record CreateThreadDto 
{
    [Required] public required Guid UserId { get; init; } 

    [Required, MaxLength(NumberConstants.MaxLengthThreadName)] public required string Name { get; init; } 
}
