using LLM_Test.Data.Entities;
using LLM_Test.Dtos.Messages;
using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Dtos.Threads;

public static class ThreadMappingExtension
{
    public static GetThreadDto ToGetDto(this Thread thread) 
    {
        return new GetThreadDto()
        {
            Id = thread.Id,
            Messages = thread.Messages.ToGetDtoList(),
            Name = thread.Name,
        };
    }

}
