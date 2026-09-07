using LLM_Test.Dtos.Messages;
using LLM_Test.Dtos.Threads;
using LLM_Test.Extensions;
using LLM_Test.Services.GrpcChatService;
using LLM_Test.Services.ThreadService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Immutable;
using System.Security.Claims;

namespace LLM_Test.Enpoints;

public static class ThreadEndpoints
{
    public static void MapThreadEndpoints(this IEndpointRouteBuilder app) 
    {
        var group = app.MapGroup("/api/threads").RequireAuthorization();

        group.MapPost("/", async (CreateThreadRequest request,
            ClaimsPrincipal user,
            IThreadService threadService,
            CancellationToken cancellationToken
            ) =>
        {
            var dto = new CreateThreadDto()
            {
                Name = request.Name,
                UserId = user.GetUserId()
            };

            var id = await threadService.CreateThreadAsync(dto, cancellationToken);

            return Results.Created($"/api/threads/{id}", new { Id = id });
        }
        );

        group.MapGet("/{threadId:guid}", async (Guid threadId,
            ClaimsPrincipal user,
            IThreadService threadService,
            CancellationToken cancellationToken) =>
        {
            if (!await threadService.CheckIfTheThreadBelongsToUser(threadId, user.GetUserId(), cancellationToken)) 
            {
                return Results.Forbid();
            }

            var threadMessages = await threadService.GetAllMessagesForThreadOrderedByCreatedAtAsync(threadId, cancellationToken);

            return Results.Ok(threadMessages);
        });

        group.MapDelete("/{threadId:guid}", async (Guid threadId,
            ClaimsPrincipal user,
            IThreadService threadService,
            CancellationToken cancellationToken) =>
        {
            if (!await threadService.CheckIfTheThreadBelongsToUser(threadId, user.GetUserId(), cancellationToken))
            {
                return Results.Forbid();
            }
            await threadService.DeleteThreadAsync(threadId, cancellationToken);
            return Results.NoContent();
        });

        group.MapGet("/", async (ClaimsPrincipal user, IThreadService thread, CancellationToken cancellationToken) =>
        {
            var threads = await thread.GetAllThreadsForUserAsync(user.GetUserId(), cancellationToken);
            return Results.Ok(threads);
        });

        group.MapPost("/{threadId:guid}/messages", async (
            Guid threadId,
            CreateMessageDto request,
            ClaimsPrincipal user,
            IThreadService threadService,
            IChatGrpcService chatGrpcService,
            CancellationToken cancellationToken
            ) =>
        {
            if (!await threadService.CheckIfTheThreadBelongsToUser(threadId, user.GetUserId(), cancellationToken) || user.GetUserId() != request.UserId ) 
            {
                return Results.Forbid();
            }

            try
            {
                var (thread, history, userMessage) = await threadService.AddMessageToThreadAsync(threadId, request, cancellationToken);

                var responseMessage = await chatGrpcService.MakeRequestAsync(thread, history.ToImmutableList(), userMessage, cancellationToken);

                var (newThread, newHistory, newMessage) = await threadService.AddMessageToThreadAsync(threadId, new CreateMessageDto()
                {
                    UserId = user.GetUserId(),
                    Text = responseMessage.Text,
                    Thoughts = responseMessage.Thoughts,
                    Role = responseMessage.Role,
                    ImageAttachments = []
                }, cancellationToken);


                return Results.Ok(responseMessage);

            }
            catch (Exception ex)
            {

                return Results.BadRequest(ex.Message);
            }
        });


    }
}

public record CreateThreadRequest() 
{
    public required string Name { get; init; }
}
