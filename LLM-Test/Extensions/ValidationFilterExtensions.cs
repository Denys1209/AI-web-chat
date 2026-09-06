using LLM_Test.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LLM_Test.Extensions;

public static class ValidationFilterExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder) where T : class 
        => builder.AddEndpointFilter<ValidationFilter<T>>();
}
