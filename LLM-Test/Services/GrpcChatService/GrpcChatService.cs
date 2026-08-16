using LLM_Test.Data.Entities;
using System.Runtime.CompilerServices;

using Message = LLM_Test.Data.Entities.Message;
using GrpcMessage = Chat.Message;
using Thread = LLM_Test.Data.Entities.Thread;
using Chat;
using LLM_Test.Services.ImageStoreService;
using Grpc.Core;

namespace LLM_Test.Services.GrpcChatService;

public class GrpcChatService : IChatGrpcService
{
    private Gemma4Server.Gemma4ServerClient _client;
    private IImageStorageService _imageStorageService;

    public GrpcChatService(Gemma4Server.Gemma4ServerClient client, IImageStorageService imageStorageService)
    {
        _client = client;
        _imageStorageService = imageStorageService;
    }

    private async Task<GrpcMessage> MapMessageAsync(Message message, CancellationToken cancellationToken) 
    {
        var protoMessage = new GrpcMessage
        {
            Text = message.Text,
            Role = message.Role
        };

        foreach (var image in message.ImageAttacheds)
        {
            protoMessage.ImageAttachment.Add(new Chat.ImageAttachment
            {
                Data = Google.Protobuf.ByteString.CopyFrom(await _imageStorageService.ReadAsync(image.Path, cancellationToken)),
                MimeType = image.Type
            });
        }

        return protoMessage;
    }

    private async Task<History> BuildHistoryAsync(IReadOnlyCollection<Message> messages, CancellationToken cancellationToken) 
    {
        var history = new History();
        foreach (var message in messages)
            history.Messages.Add(await MapMessageAsync(message, cancellationToken));

        return history;
    }

    public async Task<Message> MakeRequestAsync(Thread thread, IReadOnlyCollection<Message> history, Message userMessage, CancellationToken cancellationToken)
    {
        var request = new Request
        {
            History = await BuildHistoryAsync(history.ToList(), cancellationToken),
            UserMessage = await MapMessageAsync(userMessage, cancellationToken)
        };

        var response = await _client.MakeRequestAsync(request, cancellationToken:cancellationToken);

        var message = new Message 
        {
            Text = response.Answer,
            Thoughts = response.Thoughts,
            Role = Roles.Assistant,
            Thread = thread,
        };

        return message;
    }

    public async IAsyncEnumerable<string> MakeRequestReturnTokenByToken(IReadOnlyCollection<Message> history, Message userMessage, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new Request
        {
            History = await BuildHistoryAsync(history.ToList(), cancellationToken),
            UserMessage = await MapMessageAsync(userMessage, cancellationToken)
        };

        using var call = _client.MakeRequestStreamBackTokenByToken(request, cancellationToken: cancellationToken);

        await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken)) 
        {
            yield return response.Answer;
        }
    }
}
