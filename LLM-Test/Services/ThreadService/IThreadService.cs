using LLM_Test.Data.Entities;
using LLM_Test.Dtos.Messages;
using LLM_Test.Dtos.Threads;

using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Services.ThreadService;

public interface IThreadService
{

    public Task<Guid> CreateThreadAsync(CreateThreadDto createThreadDto, CancellationToken cancellationToken);

    public Task DeleteThreadAsync(Guid id, CancellationToken cancellationToken);

    public Task<GetThreadDto> GetThreadDtoAsync(Guid id, CancellationToken cancellationToken);

    public Task<Thread> GetThreadAsync(Guid id, CancellationToken cancellationToken);


    public Task<(Thread thread, ICollection<Message> history, Message userMessage)> AddMessageToThreadAsync(Guid ThreadId, CreateMessageDto createMessageDto, CancellationToken cancellationToken);


    public Task<ICollection<GetThreadDto>> GetAllThreadsForUser(Guid userId, CancellationToken cancellationToken);

    public Task<ICollection<GetMessageDto>> GetAllMessagesForThreadOrderedByCreatedAt(Guid threadId, CancellationToken cancellationToken);

    public Task<bool> CheckIfTheThreadBelongsToUser(Guid threadId, Guid userId, CancellationToken cancellationToken);



}
