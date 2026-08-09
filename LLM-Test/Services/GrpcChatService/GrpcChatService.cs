using Chat;
using Grpc.Core;

using Message = LLM_Test.Data.Entities.Message;
using GrpcMessage = Chat.Message;
using Thread = LLM_Test.Data.Entities.Thread;
using System.Runtime.CompilerServices;
using LLM_Test.Services.ImageServices;

namespace LLM_Test.Services.GrpcChatService;

public class GrpcChatService : IGrpcChatService
{
    private  Gemma4Server.Gemma4ServerClient _client;
    private  IImageStorageService _imageStorageService;


    public GrpcChatService(Gemma4Server.Gemma4ServerClient client, IImageStorageService imageStorageService)
    {
        _client = client;
        _imageStorageService = imageStorageService;
    }

    private async Task<History> BuildHistoryAsync(IReadOnlyList<Message> messages, CancellationToken cancellationToken )
    {
        var history = new History();
        foreach (var m in messages)
            history.Messages.Add( await MapMessageAsync(m, cancellationToken));
        return history;
    }

    private async Task<GrpcMessage> MapMessageAsync(Message m, CancellationToken cancellationToken)
    {
        var protoMessage = new GrpcMessage
        {
            Text = m.Text,
            Role = m.Role
        };

        foreach (var img in m.ImageAttacheds)
        {
            protoMessage.ImageAttachment.Add(new Chat.ImageAttachment
            {
                Data = Google.Protobuf.ByteString.CopyFrom(await _imageStorageService.ReadAsync(img.Path, cancellationToken)),
                MimeType = img.Type
            });
        }

        return protoMessage;
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
            Thread = thread
        };

        return message;

    }

    public async IAsyncEnumerable<string> MakeRequestReturnTokenByTokenAsync(
        IReadOnlyCollection<Message> history,
        Message userMessage,
        [EnumeratorCancellation] CancellationToken cancellationToken)
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
