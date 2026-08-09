using System.Runtime.CompilerServices;
using Message = LLM_Test.Data.Entities.Message;
using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Services.GrpcChatService;

public interface IGrpcChatService
{
    public Task<Message> MakeRequestAsync(Thread thread, IReadOnlyCollection<Message> history, Message userMessage, CancellationToken cancellationToken);

    public IAsyncEnumerable<string> MakeRequestReturnTokenByTokenAsync(IReadOnlyCollection<Message> history, Message userMessage, [EnumeratorCancellation] CancellationToken cancellationToken);
}
