using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using System;
using System.Net.Http;

namespace Spaceport.API;

public static class Startup
{
    public static WebApplication ConfigureApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline
        ConfigurePipeline(app);

        return app;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Add controllers
        builder.Services.AddControllers();
        
        // Add OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Add Serilog
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());
        
        // Add OpenTelemetry
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource("Spaceport.API")
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Spaceport.API"))
                    .AddAspNetCoreInstrumentation();
            });
        
        // Register domain services
        RegisterDomainServices(builder.Services);
        
        // Register infrastructure services
        RegisterInfrastructureServices(builder.Services);
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }

    private static void RegisterDomainServices(IServiceCollection services)
    {
        // Register domain services here
    }

    private static void RegisterInfrastructureServices(IServiceCollection services)
    {
        // Register infrastructure services here
        services.AddSingleton<Spaceport.Domain.Interfaces.IContextService, Spaceport.Infrastructure.Services.MockContextService>();
        
        // Configure HttpClient for external APIs
        services.AddHttpClient("AzureDevOps", client =>
        {
            client.BaseAddress = new Uri("https://dev.azure.com/");
            // Add default headers, etc.
        }).AddPolicyHandler(GetRetryPolicy());
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Polly.Extensions.Http.HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}