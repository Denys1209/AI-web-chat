using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Validation;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();

        if (argument is null)
        {
            return await next(context);
        }

        var errors = Validation(argument);

        if (errors.Count > 0)
            return Results.UnprocessableEntity(errors);

        return await next(context);
    }

    private IDictionary<string, IEnumerable<string>> Validation(T argument)
    {
        var validationResults = new List<ValidationResult>();

        var validationContext = new ValidationContext(argument);

        Validator.TryValidateObject(argument, validationContext, validationResults, true);

        return validationResults
            .GroupBy(vr => vr.MemberNames.FirstOrDefault() ?? string.Empty)
            .ToDictionary(g => g.Key, g => g.Select(vr => vr.ErrorMessage ?? string.Empty));
    }
}
