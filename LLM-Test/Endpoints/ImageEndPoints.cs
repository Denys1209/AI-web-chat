using LLM_Test.Extensions;
using LLM_Test.Services.ImageAttachmentServices;
using LLM_Test.Services.ImageServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace LLM_Test.Endpoints;

public static class ImageEndPoints
{
    public static void MapImageEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/image");

        group.MapGet("/{id:Guid}", async (Guid id, IImageAttachmentService imageAttachmentService, IImageStorageService imageStorageService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
        {

            try
            {
                var image = await imageAttachmentService.GetAsync(id, cancellationToken);

                //if (image.Message.Thread.Id != user.GetUserId())
                //{
                //    return Results.Unauthorized();
                    
                //}

                var imageData = await imageStorageService.ReadAsync(image.Path, cancellationToken);

                return Results.File(imageData, image.Type);

            }
            catch (KeyNotFoundException)
            {

                return Results.NotFound();
            }

        });
    }
}
