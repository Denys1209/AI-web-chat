using LLM_Test.Data;
using LLM_Test.Dtos.Messages;
using LLM_Test.Dtos.Threads;
using LLM_Test.Services.GrpcChatService;
using Microsoft.EntityFrameworkCore;
using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Services.ThreadService;

public class ThreadService : IThreadService
{
    private readonly AppDbContext _db;

    private readonly IGrpcChatService _chatService;

    public ThreadService(AppDbContext db, IGrpcChatService grpcChatService)
    {
        _db = db;
        _chatService = grpcChatService;
    }

    public Task<GetMessageDto> AddMessageToThreadAsync(Guid ThreadId, CreateMessageDto createMessageDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateThreadAsync(CreateThreadDto createThreadDto, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == createThreadDto.UserId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"User with this Id doesn't exist {createThreadDto.UserId}");



        var thread = new Thread
        {
            Name = createThreadDto.Name,
            User = user,
        };

        await _db.Threads.AddAsync(thread, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        return thread.Id;
    }

    public Task DeleteThreadAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<GetThreadDto> GetThreadAsync(Guid id, CancellationToken cancellationToken)
    {
        var thread = await _db.Threads.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (thread is null)
            throw new Exception($"Threads with this id: {id} wasn't found");

        return new GetThreadDto
        {
            Id = thread.Id,
            Messages  = new List<GetMessageDto>(),
            Name = thread.Name,
        };

    }
}
