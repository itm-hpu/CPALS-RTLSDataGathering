using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSignalR
{
    public class ObservablePosition : Notifier
    {
        private int index;
        private object objectId;
        private DateTime timestamp;
        private int x;
        private int y;
        private int zone;
        private string zoneName;
        private float longitude;
        private float latitude;

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                OnPropertyChanged("Index");
            }
        }
        public object ObjectId
        {
            get
            {
                return objectId;
            }
            set
            {
                objectId = value;
                OnPropertyChanged("ObjectId");
            }
        }
        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
                OnPropertyChanged("Timestamp");
            }
        }
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }
        public int Zone
        {
            get
            {
                return zone;
            }
            set
            {
                zone = value;
                OnPropertyChanged("Zone");
            }
        }
        public string ZoneName
        {
            get
            {
                return zoneName;
            }
            set
            {
                zoneName = value;
                OnPropertyChanged("ZoneName");
            }
        }
        public float Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }
        public float Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }
    }
}
