using Chat;
using Grpc.Core;

using Message = LLM_Test.Data.Entities.Message;
using GrpcMessage = Chat.Message;
using Thread = LLM_Test.Data.Entities.Thread;
using System.Runtime.CompilerServices;

namespace LLM_Test.Services.GrpcChatService;

public class GrpcChatService : IGrpcChatService
{
    private  Gemma4Server.Gemma4ServerClient _client;


    public GrpcChatService(Gemma4Server.Gemma4ServerClient client)
    {
        _client = client;
    }

    private History BuildHistory(IReadOnlyList<Message> messages)
    {
        var history = new History();
        foreach (var m in messages)
            history.Messages.Add(MapMessage(m));
        return history;
    }

    private GrpcMessage MapMessage(Message m)
    {
        var proto = new GrpcMessage
        {
            Text = m.Text,
            Role = m.Role
        };

        foreach (var img in m.ImageAttacheds)
        {
            proto.ImageAttachment.Add(new Chat.ImageAttachment
            {
                Data = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes(img.Path)),
                MimeType = img.Type
            });
        }

        return proto;
    }

    public async Task<Message> MakeRequestAsync(Thread thread, IReadOnlyCollection<Message> history, Message userMessage, CancellationToken cancellationToken)
    {
        var request = new Request
        {
            History = BuildHistory(history.ToList()),
            UserMessage = MapMessage(userMessage)
        };

        var response = await _client.MakeRequestAsync(request, cancellationToken:cancellationToken);
        var message = new Message
        {
            Text = response.Answer,
            Thoughts = response.Thoughts,
            Role = Roles.Assistent,
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
            History = BuildHistory(history.ToList()),
            UserMessage = MapMessage(userMessage)
        };

        using var call = _client.MakeRequestStreamBackTokenByToken(request, cancellationToken: cancellationToken);

        await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response.Answer;
        }


    }
}
