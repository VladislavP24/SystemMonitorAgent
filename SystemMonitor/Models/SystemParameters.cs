using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitorAgent.Models
{
    public class SystemParameters
    {
        public string Hostname { get; set; } = string.Empty; // Hostname
        public List<string> IpAddresses { get; set; } = new(); // IP-адреса
        public string WindowsVersion { get; set; } = string.Empty; // Версия Windows
        public string Uptime { get; set; } = string.Empty; // Uptime
        public double CpuLoad { get; set; } // Текущая загрузка CPU
        public long RamUsedMb { get; set; } // Использование RAM
        public List<DiskInfo> Disks { get; set; } = new(); // Свободное место на дисках
        public List<string> RunningProcesses { get; set; } = new(); // Список запущенных процессов
        public Dictionary<string, bool> ProcessesStatus { get; set; } = new(); // Наличие заданных процессов
        public DateTime Timestamp { get; set; } // Время
    }
}
