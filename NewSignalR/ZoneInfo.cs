using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace NewSignalR
{
    public class ZoneInfo
    {
        public int zoneId { get; set; }
        public string zoneName { get; set; }
        public Point3D zoneBoundary { get; set; }
    }
}
