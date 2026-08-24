using LLM_Test.Data;
using LLM_Test.Data.Entities;
using LLM_Test.Dtos.Messages;
using LLM_Test.Dtos.Threads;
using LLM_Test.Services.ImageAttachmentService;
using Microsoft.EntityFrameworkCore;
using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Services.ThreadService;

public class ThreadService : IThreadService
{

    private readonly AppDbContext _db;
    private readonly IImageAttachmentService _imageAttachmentService;


    public ThreadService(AppDbContext db, IImageAttachmentService imageAttachmentService)
    {
        _db = db;
        _imageAttachmentService = imageAttachmentService;
    }

    public async Task<(Thread thread, ICollection<Message> history, Message userMessage)> AddMessageToThreadAsync(Guid ThreadId, CreateMessageDto createMessageDto, CancellationToken cancellationToken)
    {
        var thread = await _db.Threads.FirstOrDefaultAsync(t => t.Id == ThreadId, cancellationToken);

        if (thread is null)
            throw new InvalidOperationException($"Thread with this id doesn't exist {ThreadId}");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == createMessageDto.UserId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"User with this id doesn't exist {createMessageDto.UserId}");

        var history = thread.Messages.OrderBy(m => m.CreatedAt).ToList();


        var message = new Message
        {
            Text = createMessageDto.Text,
            Thoughts = createMessageDto.Thoughts,
            Role = createMessageDto.Role,
            Thread = thread,
        };

        foreach (var imageAttachmentDto in createMessageDto.ImageAttachments)
        {
            var imageAttachment = await _imageAttachmentService.CreateAsync(imageAttachmentDto, message, cancellationToken);
            message.ImageAttacheds.Add(imageAttachment);
        }

        await _db.Messages.AddAsync(message);
        await _db.SaveChangesAsync();

        return (thread, history, message);

    }
    public async Task<Guid> CreateThreadAsync(CreateThreadDto createThreadDto, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == createThreadDto.UserId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"User with this id {createThreadDto.UserId} doesn't exist");

        var thread = new Thread
        {
            Name = createThreadDto.Name,
            User = user,
        };

        await _db.Threads.AddAsync(thread, cancellationToken);

        await _db.SaveChangesAsync();

        return thread.Id;

    }
    public Task DeleteThreadAsync(Guid threadId, CancellationToken cancellationToken)
    {
        return _db.Threads.Where(t => t.Id == threadId).ExecuteDeleteAsync(cancellationToken);
    }


    public async Task<bool> CheckIfTheThreadBelongsToUser(Guid threadId, Guid userId, CancellationToken cancellationToken)
    {
        return await _db.Threads.AnyAsync(t => t.Id == threadId && t.User.Id == userId, cancellationToken);
    }
    public async Task<GetThreadDto> GetThreadDtoAsync(Guid id, CancellationToken cancellationToken)
    {
        var thread = await _db.Threads.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (thread is null)
            throw new InvalidOperationException($"Thread with this id doesn't exist {id}");

        return new GetThreadDto
        {
            Id = id,
            Name = thread.Name,
        };
   
    }


  
    

    public async Task<ICollection<GetMessageDto>> GetAllMessagesForThreadOrderedByCreatedAtAsync(Guid threadId, CancellationToken cancellationToken)
    {
        var thread = await _db.Threads.FirstOrDefaultAsync(t => t.Id == threadId, cancellationToken);

        if (thread is null)
            throw new InvalidOperationException($"Thread with this id doesn't exist {threadId}");

        return thread.Messages.OrderBy(m => m.CreatedAt).Select(m => m.ToGetDto()).ToList();
    }

    public async Task<ICollection<GetThreadDto>> GetAllThreadsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"User with this Id doesn't exist {userId}");


        return await _db.Threads.Where(t => t.User.Id == userId).Select(t => t.ToGetDto()).ToListAsync(cancellationToken);
    }

    public async Task<Thread> GetThreadAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Threads.FirstOrDefaultAsync(t => t.Id == id, cancellationToken) ?? throw new InvalidOperationException($"Thread with this id doesn't exist {id}");
    }

   }
