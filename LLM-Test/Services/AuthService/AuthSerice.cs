using LLM_Test.Data;
using LLM_Test.Data.Entities;
using LLM_Test.Dtos.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Thread = LLM_Test.Data.Entities.Thread;

namespace LLM_Test.Services.AuthService;

public class AuthSerice : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _config;


    public AuthSerice(AppDbContext db, IPasswordHasher<User> passwordHasher, IConfiguration config)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _config = config;
    }

    public async Task<AuthResponse> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken)
    {

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Gmail == loginUserDto.Gmail, cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid Gmail or Password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid Gmail or Password");

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, loginUserDto.Password);
            await _db.SaveChangesAsync(cancellationToken);
        }

        var token = GenerateToken(user);

        return new AuthResponse
        {
            DisplayedName = user.DisplayedName,
            Gmail = user.Gmail,
            Token = token,
            Id = user.Id
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        var alreadyExists = await _db.Users.AnyAsync(u => u.Gmail == registerUserDto.Gmail, cancellationToken);

        if (alreadyExists)
            throw new InvalidOperationException("User with this Gmail already exists");

        var user = new User
        {
            DisplayedName = registerUserDto.DisplayedName,
            Gmail = registerUserDto.Gmail,
            PasswordHash = "",
            Threads = new List<Thread>()
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);

        try 
        {
            await _db.Users.AddAsync(user, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while creating the user account.", ex);
        }

        var token = GenerateToken(user);

        return new AuthResponse
        {
            DisplayedName = user.DisplayedName,
            Gmail = user.Gmail,
            Token = token,
            Id = user.Id
        };



    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Gmail),
            new Claim("displayedName", user.DisplayedName),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
