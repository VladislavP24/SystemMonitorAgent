using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SystemMonitorAgent.Models;

namespace SystemMonitorAgent
{
    public class ApiSender
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiSender> _logger;
        private readonly AppSettings _config;

        public ApiSender(HttpClient httpClient, ILogger<ApiSender> logger, IOptions<AppSettings> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = options.Value;
            _httpClient.Timeout = TimeSpan.FromSeconds(_config.ApiTimeoutSeconds);
        }

        public async Task SendSystemParametersAsync(SystemParameters parameters, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_config.ApiUrl, parameters, cancellationToken);

                if (response.IsSuccessStatusCode)
                    _logger.LogInformation("Data successfully sent to API.");
                else
                    _logger.LogWarning("API returned error: {code}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during data collection or sending: {msg}", ex.Message);
            }
        }
    }
}
