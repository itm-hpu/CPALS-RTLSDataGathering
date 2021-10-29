using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LabManager
{
    class PositionData
    {
        public string TimeStamp { get; set; }
        public Point Coordinates { get; set; }
        public string ObjectID { get; set; }
        public string Type { get; set; }
        public string Zone { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
