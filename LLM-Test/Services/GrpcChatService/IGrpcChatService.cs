
using Chat;
using Grpc.Core;

using Message = LLM_Test.Data.Entities.Message;
using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Services.GrpcChatService;

public interface IGrpcChatService
{
    public Message MakeRequest(Thread thread, IReadOnlyCollection<Message> history, Message userMessage);

    public  IAsyncEnumerable<string> MakeRequestReturnTokenByTokenAsync(IReadOnlyCollection<Message> history, Message userMessage, CancellationToken cancellationToken);
}
