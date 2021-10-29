using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.IO;
using System.Globalization;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Reflection;

namespace NewSignalR
{
    
    class Controller
    {
        RESTfulClient rClient = new RESTfulClient();

        public string GetID(string uriAddress, string userName, string password)
        {
            string result_IDs = "";
            rClient.uriAddress = uriAddress;
            rClient.userName = userName;
            rClient.userPassword = password;
            string[] responseResult = rClient.GetIDs();
            for (int i = 0; i < responseResult.Count(); i++)
            {
                result_IDs = result_IDs + responseResult[i] + "\n";
            }
            return result_IDs;
        }

        public string[] DivideIDs(string tagIDs)
        {
            string objectIDs = tagIDs;
            string[] objectIDsArray = objectIDs.Split(new char[] { '\n' });
            Array.Resize(ref objectIDsArray, objectIDsArray.Length - 1);
            return objectIDsArray;
        }

        private object GetValueByProperty(object obj, string propertyName)
        {
            // get the type:
            var objType = obj.GetType();

            // iterate the properties
            var prop = (from property in objType.GetProperties()
                            // filter on the name
                        where property.Name == propertyName
                        // select the propertyInfo
                        select property).FirstOrDefault();

            // use the propertyinfo to get the instance->property value
            return prop?.GetValue(obj);
        }

        public void SaveDataToTextFile(ObservableCollection<ObservablePosition> positionList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
            
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(ObservablePosition).GetProperties();
            string[] propertyNames = new string[typeof(ObservablePosition).GetProperties().Count()];
            int propertyIndex = 0;
            foreach (PropertyInfo p in propertyInfos)
            {
                propertyNames[propertyIndex] = p.Name;
                propertyIndex++;
            }

            int iLength = positionList.Count();
            int jLength = typeof(ObservablePosition).GetProperties().Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_PositionData_" + timeStamp + "_" + positionList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    string tempResult = string.Empty;
                    for (int j = 0; j < jLength; j++)
                    {
                        tempResult = tempResult + GetValueByProperty(positionList[i], propertyNames[j]).ToString() + ",";
                    }
                    tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                    file.Write(tempResult);
                    file.Write("\n");
                }
                file.Close();
                return;
            }
            else
            {
                return;
            }
        }

        public void SaveDataToTextFile(ObservableCollection<ObservableMovementType> distanceList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(ObservableMovementType).GetProperties();
            string[] propertyNames = new string[typeof(ObservableMovementType).GetProperties().Count()];
            int propertyIndex = 0;
            foreach (PropertyInfo p in propertyInfos)
            {
                propertyNames[propertyIndex] = p.Name;
                propertyIndex++;
            }

            int iLength = distanceList.Count();
            int jLength = typeof(ObservableMovementType).GetProperties().Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_DistanceDataFromSignalR_" + timeStamp + "_" + distanceList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    string tempResult = string.Empty;
                    for (int j = 0; j < jLength; j++)
                    {
                        tempResult = tempResult + GetValueByProperty(distanceList[i], propertyNames[j]).ToString() + ",";
                    }
                    tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                    file.Write(tempResult);
                    file.Write("\n");
                }
                file.Close();
                return;
            }
            else
            {
                return;
            }
        }

        public void SaveDataToTextFile(List<Distance> distanceList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(Distance).GetProperties();
            string[] propertyNames = new string[typeof(Distance).GetProperties().Count()];
            int propertyIndex = 0;
            foreach (PropertyInfo p in propertyInfos)
            {
                propertyNames[propertyIndex] = p.Name;
                propertyIndex++;
            }

            int iLength = distanceList.Count();
            int jLength = typeof(Distance).GetProperties().Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_DistanceDataFromRESTful_" + timeStamp + "_" + distanceList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    string tempResult = string.Empty;
                    for (int j = 0; j < jLength; j++)
                    {
                        tempResult = tempResult + GetValueByProperty(distanceList[i], propertyNames[j]).ToString() + ",";
                    }
                    tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                    file.Write(tempResult);
                    file.Write("\n");
                }
                file.Close();
                return;
            }
            else
            {
                return;
            }
        }

        public void SaveDataToTextFile(ObservableCollection<ObservableMovement> movementList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(ObservableMovement).GetProperties();
            string[] propertyNames = new string[typeof(ObservableMovement).GetProperties().Count()];
            int propertyIndex = 0;
            foreach (PropertyInfo p in propertyInfos)
            {
                propertyNames[propertyIndex] = p.Name;
                propertyIndex++;
            }

            int iLength = movementList.Count();
            int jLength = typeof(ObservableMovement).GetProperties().Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_Movements_" + timeStamp + "_" + movementList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    string tempResult = string.Empty;
                    for (int j = 0; j < jLength; j++)
                    {
                        tempResult = tempResult + GetValueByProperty(movementList[i], propertyNames[j]).ToString() + ",";
                    }
                    tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                    file.Write(tempResult);
                    file.Write("\n");
                }
                file.Close();
                return;
            }
            else
            {
                return;
            }
        }

        public List<Distance> GetDistance(string uriAddress, string userName, string password, string objectID, int max_age, string aggregation)
        {
            rClient.uriAddress = uriAddress + "&object=" + objectID + "&max_age=" + max_age.ToString() + "&aggregation=" + aggregation;
            rClient.userName = userName;
            rClient.userPassword = password;

            List<string[]> responseResult = rClient.GetDistance();

            List<Distance> distances = new List<Distance>(responseResult.Count);

            for (int i = 0; i < responseResult.Count; i++)
            {
                distances.Add
                    (
                    new Distance() {
                        ObjectId = objectID,
                        Timestamp = DateTime.ParseExact(responseResult[i][0], "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                        Value = double.Parse(responseResult[i][1], CultureInfo.InvariantCulture)
            });
            }

            return distances;
        }

        public static double CalculateDistances(ObservableCollection<ObservablePosition> positionlist) // m
        {
            int i = positionlist.Count - 1;

            double distX = positionlist[i].X - positionlist[i - 1].X;
            double distY = positionlist[i].Y - positionlist[i - 1].Y;
            double dist = Math.Sqrt(distX * distX + distY * distY) / 1000.0; // to change scale of distance (after division dist scale = m)

            return dist;
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static double CalculateMovementTime(ObservableCollection<ObservablePosition> positionlist)
        {
            int i = positionlist.Count - 1;

            DateTime startTime = positionlist[i - 1].Timestamp;
            DateTime endTime = positionlist[i].Timestamp;

            double startTimeSecond = ConvertToUnixTimestamp(startTime);
            double endTimeSecond = ConvertToUnixTimestamp(endTime);

            double timeSpan = endTimeSecond - startTimeSecond;

            return timeSpan;
        }

        public static double CalculateVelocity(ObservableCollection<ObservablePosition> positionlist) // second
        {
            int i = positionlist.Count - 1;

            DateTime startTimeSpan = positionlist[i - 1].Timestamp;
            DateTime endTimeSpan = positionlist[i].Timestamp;

            double startTimeSecond = ConvertToUnixTimestamp(startTimeSpan);
            double endTimeSecond = ConvertToUnixTimestamp(endTimeSpan);

            double timeSpan = endTimeSecond - startTimeSecond;

            double dist = CalculateDistances(positionlist);

            double velocity = dist / timeSpan;

            return velocity;
        }

        public static string CheckMovementType(ObservableCollection<ObservablePosition> positionlist, double velocityValue) // m/s
        {
            double velocity = CalculateVelocity(positionlist);

            if (velocity < velocityValue) // Velocity criteria (m/s)
            {
                return "Stop";
            }
            else
            {
                return "Move";
            }
        }
        
        public static double CalculateTheLastMovement(ObservableCollection<ObservableMovementType> distanceList, ObservableCollection<ObservableMovement> movementList)
        {
            double tempDistResult = 0.0;

            if (movementList[movementList.Count - 1].Type == "Move")
            {
                for (int i = movementList[movementList.Count - 2].Index; i <= movementList[movementList.Count - 1].Index; i++)
                {
                    tempDistResult = tempDistResult + distanceList[i - 1].Distance;
                }
                return tempDistResult;
            }
            else 
            {
                return tempDistResult;
            }
        }

        public List<ZoneInfo> GetZoneInfo(string uriAddress, string userName, string password)
        {
            rClient.uriAddress = uriAddress;
            rClient.userName = userName;
            rClient.userPassword = password;

            List<ZoneInfo> responseResult = rClient.GetZoneInfo();
            List<ZoneInfo> zoneInfos = new List<ZoneInfo>(responseResult.Count);
            
            for (int i = 0; i < responseResult.Count; i++)
            {
                zoneInfos.Add
                    (
                    new ZoneInfo()
                    {
                        zoneId = Convert.ToInt32(responseResult[i].zoneId),
                        zoneName = responseResult[i].zoneName,
                        zoneBoundary = responseResult[i].zoneBoundary
                    });
            }
            return zoneInfos;
        }

        public double Percentile(double[] sequence, double excelPercentile)
        {
            Array.Sort(sequence);
            int N = sequence.Length;
            double n = (N - 1) * excelPercentile / 100.0 + 1;
            if (n == 1d) return sequence[0];
            else if (n == N) return sequence[N - 1];
            else
            {
                int k = (int)n;
                double d = n - k;
                return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
            }
        }

        public List<VelocityOyZone> CalcPercentileVelocityOfZones(ObservableCollection<ObservableMovementType> listOfMovementType, double percentage)
        {
            int zoneCount = listOfMovementType.Select(t => t.Zone).Distinct().Count();
            int[] zoneId = listOfMovementType.Select(t => t.Zone).Distinct().ToArray();
            string[] zoneName = listOfMovementType.Select(t => t.ZoneName).Distinct().ToArray();

            List<VelocityOyZone> velocityOfZones = new List<VelocityOyZone>();

            foreach (int zone in zoneId)
            {
                double[] velocityOfZone = listOfMovementType.Where(t => t.Zone == zone).Select(t => t.Velocity).ToArray();

                int i = zoneId.findIndex(zone);
                velocityOfZones.Add(new VelocityOyZone
                {
                    Zone = zoneId[i],
                    ZoneName = zoneName[i],
                    Velocity = Percentile(velocityOfZone, percentage)
                });
            }

            return velocityOfZones;
        }

        public ObservableCollection<ObservableRedefinedMovementType> CreateRedefinedMovementList(ObservableCollection<ObservableMovementType> listOfMovementType, List<VelocityOyZone> listOfVelocityByZone, double percentage)
        {
            ObservableCollection<ObservableRedefinedMovementType> redefinedMovement = new ObservableCollection<ObservableRedefinedMovementType>();

            double averageVelocityInFactory = 0.0;
            if (listOfVelocityByZone.Count == 1 && listOfVelocityByZone[listOfVelocityByZone.Count - 1].Zone == 0)
            {
                double[] velocityInZoneZero = listOfVelocityByZone.Where(t => t.Zone == 0).Select(t => t.Velocity).ToArray();
                averageVelocityInFactory = Percentile(velocityInZoneZero, percentage);

            }
            else if (listOfVelocityByZone.Count == 1 && listOfVelocityByZone[listOfVelocityByZone.Count - 1].Zone != 0)
            {
                double[] velocityExceptZoneZero = listOfVelocityByZone.Where(t => t.Zone != 0).Select(t => t.Velocity).ToArray();
                averageVelocityInFactory = Percentile(velocityExceptZoneZero, percentage);
            }
            else if (listOfVelocityByZone.Count > 1)
            {
                double[] velocityExceptZoneZero = listOfVelocityByZone.Where(t => t.Zone != 0).Select(t => t.Velocity).ToArray();
                averageVelocityInFactory = Percentile(velocityExceptZoneZero, percentage);
            }
            
            for (int i = 0; i < listOfMovementType.Count; i++)
            {
                string redefinedType;
                if (listOfMovementType[i].Velocity < averageVelocityInFactory)
                {
                    redefinedType = "Stop";
                }
                else
                {
                    redefinedType = "Move";
                }

                redefinedMovement.Add(new ObservableRedefinedMovementType
                {
                    Index = listOfMovementType[i].Index,
                    ObjectId = listOfMovementType[i].ObjectId,
                    StartTime = listOfMovementType[i].StartTime,
                    EndTime = listOfMovementType[i].EndTime,
                    MovementTime = listOfMovementType[i].MovementTime,
                    Distance = listOfMovementType[i].Distance,
                    Velocity = listOfMovementType[i].Velocity,
                    Zone = listOfMovementType[i].Zone,
                    ZoneName = listOfMovementType[i].ZoneName,
                    RedefinedType = redefinedType
                });
            }

            return redefinedMovement;
        }
        
    }

    public static class Extensions
    {
        public static int findIndex<T>(this T[] array, T item)
        {
            return Array.IndexOf(array, item);
        }
    }
}
