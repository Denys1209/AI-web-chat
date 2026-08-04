using LLM_Test.Dtos.Message;

namespace LLM_Test.Dtos.Thread;

public record GetThreadDto 
{

   public required Guid Id { get; init; } 

   public required IReadOnlyCollection<GetMessageDto> Messages { get; init; } 


   public required string Name { get; init; } 
}
