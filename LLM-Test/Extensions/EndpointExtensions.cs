using LLM_Test.Enpoints;
using Microsoft.AspNetCore.Routing;

namespace LLM_Test.Extensions;

public static class EndpointExtensions
{
    public static void MapApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthEnpoints();
        app.MapThreadEndpoints();

    }
}
