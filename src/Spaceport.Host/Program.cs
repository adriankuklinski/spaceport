using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Spaceport.Host;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting Spaceport API");
            var builder = WebApplication.CreateBuilder(args);

            // Add services from API project
            var app = API.Startup.ConfigureApp(args);
            
            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Application terminated unexpectedly: {ex.Message}");
        }
    }
}