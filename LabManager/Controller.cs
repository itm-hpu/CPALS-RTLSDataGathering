using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabManager
{
    class Controller
    {
        public string[] RequestServer_AGV(string agvURI)
        {
            string[] responseResult = new string[3];

            RESTClinet rClient = new RESTClinet();

            rClient.agvAddress = agvURI;

            responseResult = rClient.MakeAGVRequest();

            return responseResult;
        }


        public string[] RequestServer_RTLS(string rtlsURI, string username, string password)
        {
            string[] responseResult;

            RESTClinet rClient = new RESTClinet();

            rClient.rtlsAddress = rtlsURI;
            rClient.userName = username;
            rClient.userPassword = password;

            responseResult = rClient.MakeRTLSRequest();

            return responseResult;
        }

        public string[] RequestServer_RTLS_pos(string rtlsURI, string username, string password)
        {
            string[] responseResult;

            RESTClinet rClient = new RESTClinet();

            rClient.rtlsAddress = rtlsURI;
            rClient.userName = username;
            rClient.userPassword = password;

            responseResult = rClient.MakeRTLSposRequest();

            return responseResult;
        }

        public List<string[]> RequestServer_RTLS_pos(string rtlsURI, string username, string password, int requiringNumofPoints)
        {
            List<string[]> responseResult = new List<string[]>(requiringNumofPoints);

            RESTClinet rClient = new RESTClinet();

            rClient.rtlsAddress = rtlsURI;
            rClient.userName = username;
            rClient.userPassword = password;

            responseResult = rClient.MakeRTLSposRequest(requiringNumofPoints); //requiringNumofPoints

            return responseResult;
        }


        public string GetID(string rtlsURI, string userName, string password)
        {
            string result_IDs = "";

            RESTClinet rClient = new RESTClinet();

            rClient.rtlsAddress = rtlsURI;
            rClient.userName = userName;
            rClient.userPassword = password;

            string[] responseResult = rClient.GetIDs();

            for (int i = 0; i < responseResult.Count(); i++)
            {
                result_IDs = result_IDs + responseResult[i] + "\n";
            }

            return result_IDs;
        }

        public void WriteCSVfile(List<List<PositionData>> positionDatas)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            int iLength = positionDatas.Count();
            int jLength = positionDatas[0].Count();

            string[] filedir = new string[jLength];
            StreamWriter[] file = new StreamWriter[jLength];

            for (int j = 0; j < jLength; j++)
            {

                filedir[j] = Directory.GetCurrentDirectory();
                filedir[j] = filedir[j] + @"\PositionData_" + timeStamp + "_" + positionDatas[0][j].Type + "_" + positionDatas[0][j].ObjectID + ".csv";
                file[j] = new StreamWriter(filedir[j]);

                for (int i = 0; i < iLength; i++)
                {
                    file[j].Write(positionDatas[i][j].Type + "," + "\t" + positionDatas[i][j].ObjectID + "," + positionDatas[i][j].TimeStamp + "," + positionDatas[i][j].Coordinates.X + "," + positionDatas[i][j].Coordinates.Y + "," + ConvertZoneIDtoStationName(positionDatas[i][j].Zone) + "," + positionDatas[i][j].Longitude + "," + positionDatas[i][j].Latitude);
                    file[j].Write("\n");
                }
                file[j].Close();
            }
            return;
        }

        public static Color Rainbow(float progress)
        {
            float div = (Math.Abs(progress % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;

            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, Convert.ToByte(ascending), 0);
                case 1:
                    return Color.FromArgb(255, Convert.ToByte(descending), 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, Convert.ToByte(ascending));
                case 3:
                    return Color.FromArgb(255, 0, Convert.ToByte(descending), 255);
                case 4:
                    return Color.FromArgb(255, Convert.ToByte(ascending), 0, 255);
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, Convert.ToByte(descending));
            }
        }

        public string ConvertZoneIDtoStationName(string ZoneID)
        {
            if (ZoneID == "10") return "W1";
            else if (ZoneID == "11") return "A";
            else if (ZoneID == "12") return "C";
            else if (ZoneID == "13") return "B";
            else if (ZoneID == "14") return "W3";
            else if (ZoneID == "15") return "W2";
            else if (ZoneID == "16") return "FH1";
            else if (ZoneID == "17") return "B";
            else if (ZoneID == "18") return "D";
            else if (ZoneID == "19") return "FH2";
            else if (ZoneID == "20") return "F";
            else if (ZoneID == "21") return "FH3";
            else return ZoneID;

        }

        public Ellipse CreateDotofCoordinates(int dotsize, Color dotcolor, double X, double Y)
        {
            Ellipse dot = new Ellipse();
            Color color = new Color();
            color = dotcolor;
            dot.Height = dotsize;
            dot.Width = dotsize;
            dot.Fill = new SolidColorBrush(color);
            dot.Margin = new Thickness(X, Y, 0, 0);
            return dot;
        }


        public string PrintPositionData(List<List<PositionData>> result, int i, int j, string type, string objectID)
        {
            if (result[i][j].Type == type && result[i][j].ObjectID == objectID)
            {
                if (!double.IsNaN(result[i][j].Coordinates.X))
                {
                    string result_RTLS =
                        i.ToString() + ", " +
                        result[i][j].TimeStamp + ", " +
                        result[i][j].Type + ", " +
                        result[i][j].ObjectID + ", " +
                        result[i][j].Coordinates.X + ", " +
                        result[i][j].Coordinates.Y + ", " +
                        ConvertZoneIDtoStationName(result[i][j].Zone) + ", " +
                        result[i][j].Longitude + ", " +
                        result[i][j].Latitude;

                    return result_RTLS;
                }
                else
                {
                    string result_RTLS =
                        i.ToString() + ", " +
                       "NaN" + ", " +
                       result[i][j].Type + ", " +
                       result[i][j].ObjectID + ", " +
                       "NaN" + ", " +
                       "NaN" + ", " +
                       "NaN" + ", " +
                       "NaN" + ", " +
                       "NaN";

                    return result_RTLS;
                }
            }
            else return null;
        }
    }
}
