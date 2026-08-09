using Chat;
using LLM_Test.Data;
using LLM_Test.Dtos.Messages;
using LLM_Test.Dtos.Threads;
using LLM_Test.Extensions;
using LLM_Test.Services.GrpcChatService;
using LLM_Test.Services.ThreadService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LLM_Test.Endpoints;

public static class ThreadEndpoints
{
    public static void MapThreadEndpoints(this IEndpointRouteBuilder app) 
    {
        var group = app.MapGroup("/api/threads").RequireAuthorization();

        group.MapPost("/", async (CreateThreadRequest request, ClaimsPrincipal user, IThreadService threadService, CancellationToken cancellationToken) =>
        {
            var dto = new CreateThreadDto
            {
                UserId = user.GetUserId(),
                Name = request.Name
            };

            var id = await threadService.CreateThreadAsync(dto, cancellationToken);

            return Results.Created($"/api/threads/{id}", new { Id = id });

        });

        group.MapGet("/{threadId:guid}", async (Guid threadId, ClaimsPrincipal user, IThreadService threadService, CancellationToken cancellationToken) =>
        {
            if (!await threadService.CheckIfTheThreadBelongsToUser(threadId, user.GetUserId(), cancellationToken))
            {
                return Results.Forbid();
            }

           
            var threadMessages = await threadService.GetAllMessagesForThreadOrderedByCreatedAt(threadId, cancellationToken);

            return Results.Ok(threadMessages);
        });

        group.MapDelete("/{threadId:guid}", async (Guid threadId, ClaimsPrincipal user, IThreadService threadService, CancellationToken cancellationToken) =>
        {
            if (!await threadService.CheckIfTheThreadBelongsToUser(threadId, user.GetUserId(), cancellationToken))
            {
                return Results.Forbid();
            }
            await threadService.DeleteThreadAsync(threadId, cancellationToken);
            return Results.NoContent();
        });

        group.MapGet("/", async (ClaimsPrincipal user, IThreadService threadService, CancellationToken cancellationToken) =>
        {
            var threads = await threadService.GetAllThreadsForUser(user.GetUserId(), cancellationToken);
            return Results.Ok(threads);
        });

        group.MapPost("/{threadId:guid}/messages", async (Guid threadId, CreateMessageDto request, ClaimsPrincipal user, IThreadService threadService, IGrpcChatService chatService, CancellationToken cancellationToken) =>
        {
            if (!await threadService.CheckIfTheThreadBelongsToUser(threadId, user.GetUserId(), cancellationToken) || request.UserId != user.GetUserId())
            {
                return Results.Forbid();
            }


            try
            {
                var (thread, history, userMessage) = await threadService.AddMessageToThreadAsync(threadId, request, cancellationToken);

                var responseMessage = await chatService.MakeRequestAsync(thread, history.ToImmutableList(), userMessage, cancellationToken);

                var responseMessageDto = responseMessage.ToGetDto();

                await threadService.AddMessageToThreadAsync(threadId, new CreateMessageDto()
                {
                    ImageAttachments = [],
                    Role = responseMessage.Role,
                    Text = responseMessage.Text,
                    Thoughts = responseMessage.Thoughts,
                    UserId = user.GetUserId()
                }, cancellationToken);

                return Results.Ok(responseMessage.ToGetDto());
            }
            catch (Exception ex)
            {

                return Results.BadRequest(ex.Message);
            }


        });

    }
}

public record CreateThreadRequest(string Name);
