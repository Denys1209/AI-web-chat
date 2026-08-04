using LLM_Test.Dtos.Message;
using LLM_Test.Dtos.Thread;

namespace LLM_Test.Services.ThreadService;

public interface IThreadService
{

    public Task CreateThreadAsync(CreateThreadDto createThreadDto, CancellationToken cancellationToken);

    public Task DeleteThreadAsync(Guid id, CancellationToken cancellationToken);

    public Task<GetThreadDto> GetThreadAsync(Guid id, CancellationToken cancellationToken);

    public void CreateThread(CreateThreadDto createThreadDto); 

    public void DeleteThread(Guid id);

    public GetThreadDto Get(Guid id);

    public Task AddMessageToThreadAsync(Guid ThreadId, CreateMessageDto createMessageDto);

    public void AddMessageToThread(Guid ThreadId, CreateMessageDto createMessageDto);


}
