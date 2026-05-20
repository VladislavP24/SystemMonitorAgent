using SystemMonitorAgent;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SystemMonitorAgent.Models;
using Microsoft.Extensions.Http;

namespace SystemMonitorAgent;

public class Program
{
    public static void Main(string[] args)
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        var initialConfig = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var bootstrapLogPath = initialConfig.GetValue<string>("Serilog:WriteTo:File:Args:path")
                               ?? "Logs/boot.log";

        if (!Path.IsPathRooted(bootstrapLogPath))
            bootstrapLogPath = Path.Combine(AppContext.BaseDirectory, bootstrapLogPath);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: bootstrapLogPath,
                rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                shared: true
            )
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Start System Monitor Agent...");

            var builder = Host.CreateDefaultBuilder(args);

            builder.UseWindowsService(options => {
                options.ServiceName = "SystemMonitorAgent";
            });


            builder.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Debug()
            );

            builder.ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                services.AddSingleton<SystemInfoCollector>();
                services.AddHttpClient<ApiSender>();
                services.AddHostedService<Worker>();
            });

            var host = builder.Build();
            host.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "System Monitor Agent crashed on startup.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}