using Chat;
using Grpc.Net.Client;
using LLM_Test.Data;
using LLM_Test.Services.AuthService;
using LLM_Test.Services.GrpcChatService;
using LLM_Test.Services.ImageAttachmentServices;
using LLM_Test.Services.ImageServices;
using LLM_Test.Services.ThreadService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using LLM_Test.Data.Entities;

namespace LLM_Test.Extensions;
public static class DependencyInjectionExtensions
{

    public static void AddApplication(this IServiceCollection services, IConfiguration configuration) 
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<AppDbContext>(options =>
        {
            options
                .UseNpgsql(dataSource)
                .UseLazyLoadingProxies();
        });

        var channel = GrpcChannel.ForAddress("http://localhost:50051");
        var client = new Gemma4Server.Gemma4ServerClient(channel);

        services.AddSingleton<Gemma4Server.Gemma4ServerClient>(client);

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped<IImageStorageService, LocalImageStorageService>();
        services.AddScoped<IImageAttachmentService, ImageAttachmentService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IGrpcChatService, GrpcChatService>();

        services.AddScoped<IThreadService, ThreadService>();



    }



}
