using LLM_Test.Dtos.Messages;

namespace LLM_Test.Dtos.Threads;

public record GetThreadDto 
{

   public required Guid Id { get; init; } 

   public required ICollection<GetMessageDto> Messages { get; init; } 


   public required string Name { get; init; } 
}
