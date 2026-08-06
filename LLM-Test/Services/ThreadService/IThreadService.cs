using LLM_Test.Dtos.Messages;
using LLM_Test.Dtos.Threads;

namespace LLM_Test.Services.ThreadService;

public interface IThreadService
{

    public Task<Guid> CreateThreadAsync(CreateThreadDto createThreadDto, CancellationToken cancellationToken);

    public Task DeleteThreadAsync(Guid id, CancellationToken cancellationToken);

    public Task<GetThreadDto> GetThreadAsync(Guid id, CancellationToken cancellationToken);


    public Task<GetMessageDto> AddMessageToThreadAsync(Guid ThreadId, CreateMessageDto createMessageDto);



}
