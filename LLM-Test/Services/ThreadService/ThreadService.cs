using LLM_Test.Data;
using LLM_Test.Data.Entities;
using LLM_Test.Dtos.Messages;
using LLM_Test.Dtos.Threads;
using LLM_Test.Services.GrpcChatService;
using Microsoft.EntityFrameworkCore;
using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Services.ThreadService;

public class ThreadService : IThreadService
{
    private readonly AppDbContext _db;


    public ThreadService(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddMessageToThreadAsync(Guid ThreadId, CreateMessageDto createMessageDto)
    {
        var thread = await _db.Threads.FirstOrDefaultAsync(t => t.Id == ThreadId);

        if (thread is null)
            throw new InvalidOperationException($"Thread with this Id doesn't exist {ThreadId}");


        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == createMessageDto.UserId);

        var message = new Message
        {
            Text = createMessageDto.Text,
            Thoughts = createMessageDto.Thoughts,
            Role = createMessageDto.Role,
            Thread = thread,
        };

        await _db.Messages.AddAsync(message);
        await _db.SaveChangesAsync();


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

    public async Task DeleteThreadAsync(Guid id, CancellationToken cancellationToken)
    {
        await _db.Threads.Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
    public async Task<GetThreadDto> GetThreadAsync(Guid id, CancellationToken cancellationToken)
    {
        var thread = await _db.Threads.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (thread is null)
            throw new Exception($"Threads with this id: {id} wasn't found");

        return new GetThreadDto
        {
            Id = thread.Id,
            Name = thread.Name,
        };
    }
    public async Task<ICollection<GetThreadDto>> GetAllThreadsForUser(Guid userId, CancellationToken cancellationToken)
    {
        var threads = await _db.Threads.Where(t => t.User.Id == userId)
            .Select(t => new GetThreadDto
            {
                Id = t.Id,
                Name = t.Name,
            }).ToListAsync(cancellationToken);

        return threads;
    }

    public async Task<ICollection<GetMessageDto>> GetAllMessagesForThreadOrderedByCreatedAt(Guid threadId, CancellationToken cancellationToken)
    {
        var messages = await _db.Messages.Where(m => m.Thread.Id == threadId)
            .OrderBy(m => m.CreatedAt)
            .Select(m => m.ToGetDto()).ToListAsync(cancellationToken);

        return messages;
    }

    

    

}
