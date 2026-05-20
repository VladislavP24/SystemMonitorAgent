using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SystemMonitorAgent.Models;

namespace SystemMonitorAgent
{
    public class SystemInfoCollector
    {
        private readonly AppSettings _config;

        public SystemInfoCollector(IOptions<AppSettings> options) => _config = options.Value;

        public async Task<SystemParameters> CollectorAsync()
        {
            SystemParameters parameters = new SystemParameters
            {
                Hostname = Environment.MachineName,
                WindowsVersion = RuntimeInformation.OSDescription,
                IpAddresses = GetIpAddresses(),
                Uptime = GetUptime().ToString(@"dd\.hh\:mm\;ss"),
                CpuLoad = await GetCpuLoadAsync(),
                RamUsedMb = GetRamUsed(),
                Disks = GetDiskInfo(),
                RunningProcesses = GetRunningProcesses(),
                ProcessesStatus = GetProcessesStatus(),
                Timestamp = DateTime.UtcNow
            };

            return parameters;
        }

        private List<string> GetIpAddresses() =>
            Dns.GetHostEntry(Dns.GetHostName()).AddressList
               .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
               .Select(x => x.ToString()).ToList();

        private TimeSpan GetUptime() => TimeSpan.FromMilliseconds(Environment.TickCount64);

        private async Task<double> GetCpuLoadAsync()
        {
            var counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            counter.NextValue();
            await Task.Delay(1000);
            return Math.Round(counter.NextValue(), 2);
        }

        private long GetRamUsed()
        {
            var counter = new PerformanceCounter("Memory", "Available MBytes");
            float freeMb = counter.NextValue();
            long totalMb = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / 1024 / 1024;

            return totalMb - (long)freeMb;
        }

        private List<DiskInfo> GetDiskInfo() =>
            DriveInfo.GetDrives().Where(x => x.IsReady).Select(x => new DiskInfo
            {
                DiskName = x.Name,
                FreeSpaceGb = x.AvailableFreeSpace / 1024 / 1024 / 1024
            }).ToList();

        private List<string> GetRunningProcesses() =>
            Process.GetProcesses().Select(x => x.ProcessName).Distinct().ToList();

        private Dictionary<string, bool> GetProcessesStatus()
        {
            var running = GetRunningProcesses();
            return _config.MonitoredProcesses.ToDictionary(x => x, x => running.Contains(x));
        }
    }
}
