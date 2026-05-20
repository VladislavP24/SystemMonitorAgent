using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using SystemMonitorAgent.Models;

namespace SystemMonitorAgent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppSettings _config;
        private readonly SystemInfoCollector _collector;
        private readonly ApiSender _apiSender;

        public Worker(ILogger<Worker> logger, IOptions<AppSettings> options, SystemInfoCollector collector, ApiSender apiSender)
        {
            _logger = logger;
            _config = options.Value;
            _collector = collector;
            _apiSender = apiSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Start collecting parameters...");
                    var parameters = await _collector.CollectorAsync();
                    await _apiSender.SendSystemParametersAsync(parameters, stoppingToken);
                    _logger.LogInformation("The monitoring cycle has been successfully completed.");
                }
                catch (Exception ex) 
                { 
                    _logger.LogError("Error during data collection or sending: {msg}", ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(_config.IntervalSeconds), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stopping...");
            await base.StopAsync(cancellationToken);
        }
    }
}
