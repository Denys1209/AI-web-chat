using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LLM_Test.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user) 
    {
        var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new UnauthorizedAccessException("Token missing sub claims.");

        return Guid.Parse(sub);
    }
}
