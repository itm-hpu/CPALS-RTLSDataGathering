using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSignalR
{
    public class ObservableCollision : Notifier
    {
        private int index;
        private object objectId;
        private DateTime collisionTime;
        private int zone;
        private object counterpart;
        private string level;
        private double distance;

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

        public DateTime CollisionTime
        {
            get
            {
                return collisionTime;
            }
            set
            {
                collisionTime = value;
                OnPropertyChanged("CollisionTime");
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

        public object Counterpart
        {
            get
            {
                return counterpart;
            }
            set
            {
                counterpart = value;
                OnPropertyChanged("Counterpart");
            }
        }

        public string Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                OnPropertyChanged("Level");
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
    }
}
