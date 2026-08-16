using LLM_Test.Dtos.Users;

namespace LLM_Test.Services.AuthService;

public interface IAuthService
{
    public Task<AuthResponse> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    public Task<AuthResponse> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken);

}
