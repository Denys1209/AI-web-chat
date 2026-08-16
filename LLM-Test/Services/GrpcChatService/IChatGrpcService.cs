using LLM_Test.Data.Entities;
using System.Runtime.CompilerServices;
using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Services.GrpcChatService;

public interface IChatGrpcService
{
    public Task<Message> MakeRequestAsync(Thread thread, IReadOnlyCollection<Message> history, Message userMessage, CancellationToken cancellationToken);

    public IAsyncEnumerable<string> MakeRequestReturnTokenByToken(IReadOnlyCollection<Message> history, Message userMessage,
        [EnumeratorCancellation] CancellationToken cancellationToken);


}
