using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSignalR
{
    public class ObservableMovement : Notifier
    {
        private int index;
        private object objectId;
        private DateTime startTime;
        //private DateTime endTime;
        //private double movementTime;
        private string type;
        private int zone;
        private double distance;
        //private DateTime endTime;
        //private DateTime timespan;

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

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        /*
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        public double MovementTime
        {
            get
            {
                return movementTime;
            }
            set
            {
                movementTime = value;
                OnPropertyChanged("MovementTime");
            }
        }
        */
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged("Type");
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

        public double Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
                OnPropertyChanged("Distance");
            }
        }

        /*
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }
        public DateTime Timespan
        {
            get
            {
                return timespan;
            }
            set
            {
                timespan = value;
                OnPropertyChanged("Timespan");
            }
        }
        */
    }
}
