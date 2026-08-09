using LLM_Test.Dtos.User;
using LLM_Test.Services.AuthService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LLM_Test.Endpoints;

public static class AuthEndpoints 
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app) 
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", async (RegisterUserDto dto, IAuthService service, CancellationToken cancellationToken) => 
        {
            try 
            {
                var result = await service.RegisterAsync(dto, cancellationToken);
                return Results.Ok(result);

            }
            catch (InvalidOperationException ex) 
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPost("/login", async (LoginUserDto dto, IAuthService service, CancellationToken cancellationToken) => 
        {
            try 
            {
                var result = await service.LoginAsync(dto, cancellationToken);
                return Results.Ok(result);
            }
            catch (UnauthorizedAccessException ex) 
            {
                return Results.Unauthorized();
            }
            catch (Exception ex) 
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}
