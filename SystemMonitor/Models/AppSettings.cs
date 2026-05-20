using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitorAgent.Models
{
    public class AppSettings
    {
        public string ApiUrl { get; set; } = string.Empty;
        public int IntervalSeconds { get; set; }
        public int ApiTimeoutSeconds { get; set; }
        public string LogPath { get; set; } = string.Empty;
        public List<string> MonitoredProcesses { get; set; } = new();
    }
}
