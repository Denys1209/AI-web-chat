using Chat;
using Grpc.Net.Client;
using LLM_Test.Data;
using LLM_Test.Services.AuthService;
using LLM_Test.Services.GrpcChatService;
using LLM_Test.Services.ImageAttachmentServices;
using LLM_Test.Services.ImageServices;
using LLM_Test.Services.ThreadService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        using var channel = GrpcChannel.ForAddress("http://localhost:50051");
        var client = new Gemma4Server.Gemma4ServerClient(channel);

        services.AddKeyedSingleton<Gemma4Server.Gemma4ServerClient>(client);

        services.AddScoped<IImageStorageService, LocalImageStorageService>();
        services.AddScoped<IImageAttachmentService, ImageAttachmentService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IGrpcChatService, GrpcChatService>();

        services.AddScoped<IThreadService, ThreadService>();



    }



}
