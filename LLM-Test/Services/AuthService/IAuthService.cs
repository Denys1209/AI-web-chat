using LLM_Test.Dtos.User;

namespace LLM_Test.Services.AuthService;

public interface IAuthService
{
    public Task<AuthResponse> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken);

    public AuthResponse Login(LoginUserDto loginUserDto);

    public Task<AuthResponse> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    public AuthResponse Register(RegisterUserDto registerUserDto);
}
