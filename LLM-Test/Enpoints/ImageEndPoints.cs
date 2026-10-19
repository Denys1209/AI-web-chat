using LLM_Test.Services.ImageAttachmentService;
using LLM_Test.Services.ImageStoreService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LLM_Test.Enpoints;

public static class ImageEndPoints
{
    public static void MapImageEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/image");

        group.MapGet("/{id:guid}", async (Guid id, IImageAttachmentService imageAttachemntService, IImageStorageService imageStorageService, 
            CancellationToken cancellationToken
            ) => {
            try
            {
                var image = await imageAttachemntService.GetAsync(id, cancellationToken);

                var imageData = await imageStorageService.ReadAsync(image.Path, cancellationToken);

                return Results.File(imageData, image.Type);
            }
            catch (KeyNotFoundException)
            {

                return Results.NotFound();
            }
            catch (Exception)
            {
                return Results.StatusCode(500);
            }
        }
        );
    }
}
