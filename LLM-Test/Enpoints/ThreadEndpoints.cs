using LLM_Test.Dtos.Threads;
using LLM_Test.Extensions;
using LLM_Test.Services.ThreadService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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


    }
}

public record CreateThreadRequest() 
{
    public required string Name { get; init; }
}
