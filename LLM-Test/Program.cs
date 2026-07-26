using Grpc.Net.Client;
using Chat;
using Google.Protobuf;
using Grpc.Core;


AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

using var channel = GrpcChannel.ForAddress("http://localhost:50051");
var client = new Gemma4Server.Gemma4ServerClient(channel);

var history = new History();
history.Messages.Add(new Message { Text = "You are a helpful assistant.", Role = Roles.System });


var imageBytes = await File.ReadAllBytesAsync("testImage.jpg");

var userMessage = new Message {
    Text = "Describe the image",
    Role = Roles.User
};

userMessage.ImageAttachment.Add(new ImageAttachment
{
    Data = ByteString.CopyFrom(imageBytes),
    MimeType = "image/jpeg"
});

var request = new Request { History = history, UserMessage = userMessage };


Console.WriteLine("First Request");

try
{
    var response = client.MakeRequest(request);
    Console.WriteLine($"Thoughts: {response.Thoughts}");

    Console.WriteLine($"answer: {response.Answer}");
}
catch (Grpc.Core.RpcException ex)
{
    Console.WriteLine($"gRPC call failed: {ex.Status}");
}


Console.WriteLine("Second Request");

try
{
    using var call = client.MakeRequestStreamBackTokenByToken(request);
    await foreach (var chunk in call.ResponseStream.ReadAllAsync()) 
    {
        Console.Write(chunk.Answer);
    }
}
catch (Grpc.Core.RpcException ex)
{
    Console.WriteLine($"gRPC call failed: {ex.Status}");
}















