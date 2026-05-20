using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitorAgent.Models
{
    public class DiskInfo
    {
        public string DiskName { get; set; } = string.Empty;
        public double FreeSpaceGb { get; set; }
    }
}
