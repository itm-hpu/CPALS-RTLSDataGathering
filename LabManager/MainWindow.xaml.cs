using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Threading;


namespace LabManager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller = new Controller();

        public MainWindow()
        {
            InitializeComponent();
        }

        // #######################################################################################
        // ############################### 1. Acquire raw data ###################################
        // #######################################################################################

        List<List<PositionData>> result = new List<List<PositionData>>();
        List<List<List<PositionData>>> result_pos = new List<List<List<PositionData>>>();

        private void BtnGetID_Click(object sender, RoutedEventArgs e)
        {
            // URI, userName, password for objectID
            string rtlsURI = "https://p186-geps-production-api.hd-rtls.com/objects"; 
            string userName = "KTH"; 
            string password = "!Test4KTH";

            string result_IDs = controller.GetID(rtlsURI, userName, password);
            
            txtTAGIDs.Text = result_IDs;
        }
        
        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            string objectIDs = txtTAGIDs.Text;
            string[] objectIDsArray = objectIDs.Split(new char[] { '\n' });
            Array.Resize(ref objectIDsArray, objectIDsArray.Length - 1);

            string agvAddress = "http://192.168.128.36/api/v2.0.0/status";
            txtAGVuri.Text = agvAddress;

            string[] rtlsAddressArray = new string[objectIDsArray.Length];
            for (int i = 0; i < objectIDsArray.Length; i++)
            {
                // for RTLS coordinate URI
                rtlsAddressArray[i] = "https://p186-geps-production-api.hd-rtls.com/objects/" + objectIDsArray[i] + "/pos?max_age=" + txtCheckSeconds.Text;
                txtRTLSuri.Text = txtRTLSuri.Text + rtlsAddressArray[i] + '\n';
            }

            string[] rtlsPosAddressArray = new string[objectIDsArray.Length];
            for (int i = 0; i < objectIDsArray.Length; i++)
            {
                rtlsPosAddressArray[i] = "https://p186-geps-production-api.hd-rtls.com/positions?object=" + objectIDsArray[i] + "&count=" + txtNumofPoints.Text;
                txtRTLSuri_Pos.Text = txtRTLSuri_Pos.Text + rtlsPosAddressArray[i] + '\n';
            }

            // defined userName and password
            string userName = "KTH";
            string password = "!Test4KTH";
            txtUserName.Text = userName;
            txtPassword.Text = password;
        }

        
        private async void ButtonAcquire_Click(object sender, RoutedEventArgs e)
        {
            string agvURI = txtAGVuri.Text;

            string objectIDs = txtTAGIDs.Text;
            string[] objectIDsArray = objectIDs.Split(new char[] { '\n' });
            Array.Resize(ref objectIDsArray, objectIDsArray.Length - 1);

            string rtlsURI = txtRTLSuri.Text;
            string[] rtlsURIArray = rtlsURI.Split(new char[] { '\n' });
            Array.Resize(ref rtlsURIArray, rtlsURIArray.Length - 1);

            string rtlsURI_Pos = txtRTLSuri_Pos.Text;
            string[] rtlsURIPosArray = rtlsURI_Pos.Split(new char[] { '\n' });
            Array.Resize(ref rtlsURIPosArray, rtlsURIPosArray.Length - 1);

            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            int numofpoints = Convert.ToInt32(txtNumofPoints.Text);

            int iterNum = Convert.ToInt32(txtIterationNum.Text); // iteration number
            double intervalTime = Convert.ToDouble(txtIntervalTime.Text); // interval time

            // Show origin point on canvas
            Ellipse dot = controller.CreateDotofCoordinates(7, Colors.Black, 0, 0);
            myCanvas.Children.Add(dot);

            string temp = "SystemTime, Seq, TimeStamp, Type, ObjectID, CoordinateX, CoordinateY, Zone, Longitude, Latitude" + "\r\n";
            txtResponseObj11.Text = temp;
            txtResponseObj12.Text = temp;
            txtResponseObj13.Text = temp;
            txtResponsePos11.Text = temp;
            txtResponsePos12.Text = temp;
            txtResponsePos13.Text = temp;

            List<string> temp00000011 = new List<string>();
            List<string> temp00000012 = new List<string>();
            List<string> temp00000013 = new List<string>();
            List<string> tempPos00000011 = new List<string>();
            List<string> tempPos00000012 = new List<string>();
            List<string> tempPos00000013 = new List<string>();

            for (int i = 0; i < iterNum; i++)
            {
                // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
                //string timeStamp = System.DateTime.Now.ToString("M/dd/yyyy");
                string timeStamp = System.DateTime.Now.ToString("M/dd/yyyy HH:mm:ss.fff");

                // Get coordinates of HDW tags - /object/(objectID)/pos
                List<string[]> tempReuslt_RTLS_list = new List<string[]>();
                for (int j = 0; j < rtlsURIArray.Length; j++)
                {
                    var rtlsTask = Task.Run(
                        () => controller.RequestServer_RTLS(rtlsURIArray[j], userName, password)
                        );
                    tempReuslt_RTLS_list.Add(await rtlsTask);
                }

                /*
                // Get coordinates of HDW tags - /position
                List<string[]> tempReuslt_RTLS_Pos_list = new List<string[]>();
                for (int j = 0; j < rtlsURIPosArray.Length; j++)
                {
                    var rtlsTask = Task.Run(
                        () => controller.RequestServer_RTLS_pos(rtlsURIPosArray[j], userName, password)
                        );
                    tempReuslt_RTLS_Pos_list.Add(await rtlsTask);
                }
                */

                // Get coordinates of HDW tags - /position
                List<List<string[]>> tempReuslt_RTLS_Pos_list = new List<List<string[]>>();
                for (int j = 0; j < rtlsURIPosArray.Length; j++)
                {
                    var rtlsTask = Task.Run(
                        () => controller.RequestServer_RTLS_pos(rtlsURIPosArray[j], userName, password, numofpoints)
                        );
                    tempReuslt_RTLS_Pos_list.Add(await rtlsTask);
                }

                // under processing: success with get the count(5) data, but not to assign in "tempResult_RTLS_Pos_list"
                // list of list of list for count(5) uri from /position API


                /*
                // Get coordinates of AGV
                string[] tempReuslt_AGV = new string[3];
                var agvTask = Task.Run(() => controller.RequestServer_AGV(agvURI));
                tempReuslt_AGV = await agvTask;
                */

                // Create list type of RTLS PositionData for several TAGs
                List<PositionData> subResult = new List<PositionData>();

                for (int j = 0; j < tempReuslt_RTLS_list.Count(); j++)
                {
                    subResult.Add(new PositionData());

                    if (tempReuslt_RTLS_list[j][0] == "") subResult[j].TimeStamp = null;
                    else subResult[j].TimeStamp = (tempReuslt_RTLS_list[j][2]).ToString();

                    if (tempReuslt_RTLS_list[j][1] != "" && tempReuslt_RTLS_list[j][2] != "")
                    {
                        subResult[j].Coordinates = new Point(-Convert.ToDouble(tempReuslt_RTLS_list[j][1]), Convert.ToDouble(tempReuslt_RTLS_list[j][0]));
                    }
                    else
                    {
                        subResult[j].Coordinates = new Point(double.NaN, double.NaN);
                    }
                    subResult[j].ObjectID = tempReuslt_RTLS_list[j][3]; // TAG ID
                    subResult[j].Type = "RTLS";
                    subResult[j].Zone = tempReuslt_RTLS_list[j][4]; // Zone
                    subResult[j].Longitude = tempReuslt_RTLS_list[j][5];
                    subResult[j].Latitude = tempReuslt_RTLS_list[j][6];
                }

                // Create list type of RTLS PositionData for several TAGs
                List<List<PositionData>> subPosResult = new List<List<PositionData>>();
                List<PositionData> tempSubPosResult = new List<PositionData>();

                for (int j = 0; j < tempReuslt_RTLS_Pos_list.Count(); j++)
                {
                    subPosResult.Add(new List<PositionData>());

                    for (int k = 0; k < numofpoints; k++)
                    {
                        if (tempReuslt_RTLS_Pos_list[j][k][0] == "") subPosResult[j][k].TimeStamp = null;
                        else subPosResult[j][k].TimeStamp = (tempReuslt_RTLS_Pos_list[j][k][2]);

                        if (tempReuslt_RTLS_Pos_list[j][k][1] != "" && tempReuslt_RTLS_Pos_list[j][k][2] != "")
                        {
                            subPosResult[j][k].Coordinates = new Point(-Convert.ToDouble(tempReuslt_RTLS_Pos_list[j][k][1]), Convert.ToDouble(tempReuslt_RTLS_Pos_list[j][k][0]));
                        }
                        else
                        {
                            subPosResult[j][k].Coordinates = new Point(double.NaN, double.NaN);
                        }
                        subPosResult[j][k].ObjectID = tempReuslt_RTLS_Pos_list[j][k][3]; // TAG ID
                        subPosResult[j][k].Type = "RTLS";
                        subPosResult[j][k].Zone = tempReuslt_RTLS_Pos_list[j][k][4]; // Zone
                        subPosResult[j][k].Longitude = tempReuslt_RTLS_Pos_list[j][k][5];
                        subPosResult[j][k].Latitude = tempReuslt_RTLS_Pos_list[j][k][6];
                    }
                }

                /*
                // Create list type of AGV PositionData to append "subResult" list
                List<PositionData> subResult_AGV = new List<PositionData>()
                {
                    new PositionData()
                    {
                        TimeStamp = Convert.ToDateTime(timeStamp),
                        Coordinates = new Point(Convert.ToDouble(tempReuslt_AGV[1]), Convert.ToDouble(tempReuslt_AGV[2])),
                        ObjectID = "0", // AGV object ID?
                        Type = "AGV", // Type AGV
                        Zone = "0" // AGV default zone = 0
                    }
                };
                
                // Append PositionData of AGV into "subReslut" list
                subResult.AddRange(subResult_AGV);
                */

                // Store PositionData of RTLS tag and AGV into "reslut" list
                result.Add(subResult);
                result_pos.Add(subPosResult);


                // Show coordinates of RTLS tag and AGV
                for (int j = 0; j < result[i].Count(); j++)
                {
                    //string result_RTLS = controller.PrintPositionData(result, i, j, "RTLS", "00000011");
                    //txtResponseObj11.Text = txtResponseObj11.Text + result_RTLS + "\r\n";
                    //temp00000011.Add(result_RTLS);
                    
                    if (result[i][j].Type == "RTLS" && result[i][j].ObjectID == "00000011")
                    {
                        if ( !double.IsNaN(result[i][j].Coordinates.X))
                        {
                            string result_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                                result[i][j].TimeStamp + ", " +
                                result[i][j].Type + ", " +
                                result[i][j].ObjectID + ", " +
                                result[i][j].Coordinates.X + ", " +
                                result[i][j].Coordinates.Y + ", " +
                                controller.ConvertZoneIDtoStationName(result[i][j].Zone) + ", " +
                                result[i][j].Longitude + ", " +
                                result[i][j].Latitude;

                            txtResponseObj11.Text = txtResponseObj11.Text + result_RTLS + "\r\n";
                            temp00000011.Add(result_RTLS);
                        }
                        else
                        {
                            string result_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                               "NaN" + ", " +
                               result[i][j].Type + ", " +
                               result[i][j].ObjectID + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN";

                            txtResponseObj11.Text = txtResponseObj11.Text + result_RTLS + "\r\n";
                            temp00000011.Add(result_RTLS);
                        }
                    }
                    txtResponseObj11.ScrollToEnd();
                }
                

                for (int j = 0; j < result[i].Count(); j++)
                {
                    if (result[i][j].Type == "RTLS" && result[i][j].ObjectID == "00000012")
                    {
                        if (!double.IsNaN(result[i][j].Coordinates.X))
                        {
                            string result_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                                result[i][j].TimeStamp + ", " +
                                result[i][j].Type + ", " +
                                result[i][j].ObjectID + ", " +
                                result[i][j].Coordinates.X + ", " +
                                result[i][j].Coordinates.Y + ", " +
                                controller.ConvertZoneIDtoStationName(result[i][j].Zone) + ", " +
                                result[i][j].Longitude + ", " +
                                result[i][j].Latitude;

                            txtResponseObj12.Text = txtResponseObj12.Text + result_RTLS + "\r\n";
                            temp00000012.Add(result_RTLS);
                        }
                        else
                        {
                            string result_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                               "NaN" + ", " +
                               result[i][j].Type + ", " +
                               result[i][j].ObjectID + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN";

                            txtResponseObj12.Text = txtResponseObj12.Text + result_RTLS + "\r\n";
                            temp00000012.Add(result_RTLS);
                        }
                    }
                    txtResponseObj12.ScrollToEnd();
                }


                for (int j = 0; j < result[i].Count(); j++)
                {
                    if (result[i][j].Type == "RTLS" && result[i][j].ObjectID == "00000013")
                    {
                        if (!double.IsNaN(result[i][j].Coordinates.X))
                        {
                            string result_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                                result[i][j].TimeStamp + ", " +
                                result[i][j].Type + ", " +
                                result[i][j].ObjectID + ", " +
                                result[i][j].Coordinates.X + ", " +
                                result[i][j].Coordinates.Y + ", " +
                                controller.ConvertZoneIDtoStationName(result[i][j].Zone) + ", " +
                                result[i][j].Longitude + ", " +
                                result[i][j].Latitude;

                            txtResponseObj13.Text = txtResponseObj13.Text + result_RTLS + "\r\n";
                            temp00000013.Add(result_RTLS);
                        }
                        else
                        {
                            string result_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                               "NaN" + ", " +
                               result[i][j].Type + ", " +
                               result[i][j].ObjectID + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN";

                            txtResponseObj13.Text = txtResponseObj13.Text + result_RTLS + "\r\n";
                            temp00000013.Add(result_RTLS);
                        }
                    }
                    /*
                    else if (result[i][j].Type == "AGV")
                    {
                        string result_AGV =
                            i.ToString() + ", " +
                            result[i][j].TimeStamp + ", " +
                            result[i][j].Type + ", " +
                            result[i][j].ObjectID + ", " +
                            result[i][j].Coordinates.X + ", " +
                            result[i][j].Coordinates.Y + ", " +
                            result[i][j].Zone;

                        txtResponse.Text = txtResponse.Text + result_AGV + "\r\n";
                    }
                    */
                    txtResponseObj13.ScrollToEnd();
                }


                for (int j = 0; j < result_pos[i].Count(); j++)
                {
                    for (int k = 0; k < numofpoints; k++)
                    {
                        if (result_pos[i][j][k].Type == "RTLS" && result_pos[i][j][k].ObjectID == "00000011")
                        {
                            if (!double.IsNaN(result_pos[i][j][k].Coordinates.X))
                            {
                                string result_pos_RTLS = timeStamp.ToString() + ", " +
                                    i.ToString() + ", " +
                                    result_pos[i][j][k].TimeStamp + ", " +
                                    result_pos[i][j][k].Type + ", " +
                                    result_pos[i][j][k].ObjectID + ", " +
                                    result_pos[i][j][k].Coordinates.X + ", " +
                                    result_pos[i][j][k].Coordinates.Y + ", " +
                                    controller.ConvertZoneIDtoStationName(result_pos[i][j][k].Zone) + ", " +
                                    result_pos[i][j][k].Longitude + ", " +
                                    result_pos[i][j][k].Latitude;

                                txtResponsePos11.Text = txtResponsePos11.Text + result_pos_RTLS + "\r\n";
                                tempPos00000011.Add(result_pos_RTLS);
                            }
                            else
                            {
                                string result_pos_RTLS = timeStamp.ToString() + ", " +
                                    i.ToString() + ", " +
                                   "NaN" + ", " +
                                   result_pos[i][j][k].Type + ", " +
                                   result_pos[i][j][k].ObjectID + ", " +
                                   "NaN" + ", " +
                                   "NaN" + ", " +
                                   "NaN" + ", " +
                                   "NaN" + ", " +
                                   "NaN";

                                txtResponsePos11.Text = txtResponsePos11.Text + result_pos_RTLS + "\r\n";
                                tempPos00000011.Add(result_pos_RTLS);
                            }
                        }
                    }

                    txtResponsePos11.ScrollToEnd();
                }
                /*
                for (int j = 0; j < result_pos[i].Count(); j++)
                {
                    if (result_pos[i][j].Type == "RTLS" && result_pos[i][j].ObjectID == "00000012")
                    {
                        if (!double.IsNaN(result_pos[i][j].Coordinates.X))
                        {
                            string result_pos_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                                result_pos[i][j].TimeStamp + ", " +
                                result_pos[i][j].Type + ", " +
                                result_pos[i][j].ObjectID + ", " +
                                result_pos[i][j].Coordinates.X + ", " +
                                result_pos[i][j].Coordinates.Y + ", " +
                                controller.ConvertZoneIDtoStationName(result_pos[i][j].Zone) + ", " +
                                result_pos[i][j].Longitude + ", " +
                                result_pos[i][j].Latitude;

                            txtResponsePos12.Text = txtResponsePos12.Text + result_pos_RTLS + "\r\n";
                            tempPos00000012.Add(result_pos_RTLS);
                        }
                        else
                        {
                            string result_pos_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                               "NaN" + ", " +
                               result_pos[i][j].Type + ", " +
                               result_pos[i][j].ObjectID + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN";

                            txtResponsePos12.Text = txtResponsePos12.Text + result_pos_RTLS + "\r\n";
                            tempPos00000012.Add(result_pos_RTLS);
                        }
                    }
                    txtResponsePos12.ScrollToEnd();
                }

                for (int j = 0; j < result_pos[i].Count(); j++)
                {
                    if (result_pos[i][j].Type == "RTLS" && result_pos[i][j].ObjectID == "00000013")
                    {
                        if (!double.IsNaN(result_pos[i][j].Coordinates.X))
                        {
                            string result_pos_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                                result_pos[i][j].TimeStamp + ", " +
                                result_pos[i][j].Type + ", " +
                                result_pos[i][j].ObjectID + ", " +
                                result_pos[i][j].Coordinates.X + ", " +
                                result_pos[i][j].Coordinates.Y + ", " +
                                controller.ConvertZoneIDtoStationName(result_pos[i][j].Zone) + ", " +
                                result_pos[i][j].Longitude + ", " +
                                result_pos[i][j].Latitude;

                            txtResponsePos13.Text = txtResponsePos13.Text + result_pos_RTLS + "\r\n";
                            tempPos00000013.Add(result_pos_RTLS);
                        }
                        else
                        {
                            string result_pos_RTLS = timeStamp.ToString() + ", " +
                                i.ToString() + ", " +
                               "NaN" + ", " +
                               result_pos[i][j].Type + ", " +
                               result_pos[i][j].ObjectID + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN" + ", " +
                               "NaN";

                            txtResponsePos13.Text = txtResponsePos13.Text + result_pos_RTLS + "\r\n";
                            tempPos00000013.Add(result_pos_RTLS);
                        }
                    }
                    txtResponsePos13.ScrollToEnd();
                }
                */

                // Visualize where the RTLS tag and AGV have been
                for (int j = 0; j < result[i].Count(); j++)
                {
                    if (result[i][j].Type == "RTLS")
                    {
                        if (Double.IsNaN(result[i][j].Coordinates.X))
                        {
                            continue;
                        }
                        else
                        {
                            Ellipse dotRTLS = controller.CreateDotofCoordinates(5, Colors.Red, result[i][j].Coordinates.X * -0.5, result[i][j].Coordinates.Y * 0.5);
                            myCanvas.Children.Add(dotRTLS);
                        }
                    }
                    else if (result[i][j].Type == "AGV")
                    {
                        Ellipse dotAGV = controller.CreateDotofCoordinates(3, Colors.Blue, result[i][j].Coordinates.X * -15.0, result[i][j].Coordinates.Y * 15.0);
                        myCanvas.Children.Add(dotAGV);
                    }
                }
                await Task.Delay(TimeSpan.FromMilliseconds(intervalTime * 1000));
            }

            string message = "Finish acquiring position data!";
            string caption = "Acquire module";
            System.Windows.MessageBox.Show(message, caption);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            controller.WriteCSVfile(result);

            string message = "Finish saving position data!";
            string caption = "Acquire module";
            System.Windows.MessageBox.Show(message, caption);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtTAGIDs.Clear();
            txtCheckSeconds.Clear();
            txtIterationNum.Clear();
            txtIntervalTime.Clear();
            txtUserName.Clear();
            txtPassword.Clear();

            txtRTLSuri.Clear();
            txtAGVuri.Clear();

            txtResponseObj11.Clear();
            txtResponseObj12.Clear();
            txtResponseObj13.Clear();
            txtResponsePos11.Clear();
            txtResponsePos12.Clear();
            txtResponsePos13.Clear();

            myCanvas.Children.Clear();
        }
        
        // #######################################################################################
        // ############################### 2. Post-processing ####################################
        // #######################################################################################

        public class ParetoFreqTable 
        {
            public double RangeValue { get; set; }
            public int Freq { get; set; }
            public int CumulFreq{ get; set; }
            public double CumulPercent { get; set; }
            public string Section { get; set; }
        }

        public class DistancePoint
        {
            public DateTime TimeStamp { get; set; }
            public Point Coordinates { get; set; }
            public string ObjectID { get; set; }
            public double Distance { get; set; }
        }

        public class CandidatePoint
        {
            public int ClusterNum { get; set; }
            public DateTime TimeStamp { get; set; }
            public Point Coordinates { get; set; }
            public string ObjectID { get; set; }
        }

        /// <summary>
        /// Read CSV rawdata file
        /// </summary>
        /// <param name="CSVdir"></param>
        /// <returns></returns>
        public static string[,] ReadCSVfile(string CSVdir)
        {
            string whole_file = System.IO.File.ReadAllText(CSVdir);

            whole_file = whole_file.Replace('\n', '\r');

            string[] lines = whole_file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int numRows = lines.Length;

            int numCols = lines[0].Split(',').Length;

            string[,] tempValues = new string[numRows, numCols];

            for (int r = 0; r < numRows; r++)
            {
                string[] line_r = lines[r].Split(',');

                for (int c = 0; c < numCols; c++)
                {
                    tempValues[r, c] = line_r[c];
                }
            }
            return tempValues;
        }

        /// <summary>
        /// Remove row which has a missing value, *** Need to adjust into more efficient way to find missing value ***
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[,] RemoveEmptyRows(string[,] array)
        {
            // Find how many rows have an " NaN" value
            int rowsToRemove = 0;
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                if (array[i, 2] == " NaN") // NaN value of X_coordinate
                {
                    rowsToRemove++;
                }
            }

            // Reinitialize an array minus the number of empty rows
            string[,] results = new string[array.GetUpperBound(0) + 1 - rowsToRemove, array.GetUpperBound(1) + 1];

            int row = 0;
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                int col = 0;
                if (array[i, 2] != " NaN") // NaN value of X_coordinate
                {
                    for (int j = 0; j <= array.GetUpperBound(1); j++)
                    {
                        results[row, col] = array[i, j];
                        col++;
                    }
                    row++;
                }
            }
            return results;
        }

        /// <summary>
        /// Remove duplicate row
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public string[,] RemoveDuplicateRows(string[,] array)
        {
            // Find how many rows have an " NaN" value
            int rowsToRemove = 0;
            for (int i = 0; i <= array.GetUpperBound(0) - 1; i++)
            {
                if (array[i, 1] == array[i + 1, 1]) // compare time with previous row
                {
                    rowsToRemove++;
                }
            }

            // Reinitialize an array minus the number of duplicate rows
            string[,] results = new string[array.GetUpperBound(0) - rowsToRemove, array.GetUpperBound(1) + 1];

            int row = 0;
            for (int i = 0; i <= array.GetUpperBound(0) - 1; i++)
            {
                int col = 0;
                if (array[i, 1] != array[i + 1, 1]) // compare time with previous row
                {
                    for (int j = 0; j <= array.GetUpperBound(1); j++)
                    {
                        results[row, col] = array[i, j];
                        col++;
                    }
                    row++;
                }
            }

            string[] added = new string[array.GetUpperBound(1) + 1];

            if (array[array.GetUpperBound(0) - 1, 1] != array[array.GetUpperBound(0), 1])
            {
                for (int i = 0; i <= array.GetUpperBound(1); i++)
                {
                    added[i] = array[array.GetUpperBound(0), i];
                }
                results = AddRow(results, added);
            }

            return results;
        }

        /// <summary>
        /// Add 1-D array into 2-D array as a row to reinitialize 2-D array
        /// </summary>
        /// <param name="original"></param>
        /// <param name="added"></param>
        /// <returns></returns>
        static string[,] AddRow(string[,] original, string[] added)
        {
            int lastRow = original.GetUpperBound(0);
            int lastColumn = original.GetUpperBound(1);
            // Create new array.
            string[,] result = new string[lastRow + 2, lastColumn + 1];
            // Copy existing array into the new array.
            for (int i = 0; i <= lastRow; i++)
            {
                for (int x = 0; x <= lastColumn; x++)
                {
                    result[i, x] = original[i, x];
                }
            }
            // Add the new row.
            for (int i = 0; i < added.Length; i++)
            {
                result[lastRow + 1, i] = added[i];
            }
            return result;
        }

        /// <summary>
        /// Calculate euclidean distance between two points
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public double GetDistance(Point P1, Point P2) // string
        {
            double dist = 0.0;

            double dist1 = P1.X - P2.X;
            double dist2 = P1.Y - P2.Y;
            dist = Math.Sqrt(dist1 * dist1 + dist2 * dist2);

            return dist;
        }

        /// <summary>
        /// Calculate percentile of a sorted data set (Q1, Q3, etc.)
        /// </summary>
        /// <param name="sortedData"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static double Percentile(double[] sortedData, double p)
        {
            // algo derived from Aczel pg 15 bottom
            if (p >= 100.0d) return sortedData[sortedData.Length - 1];

            double position = (sortedData.Length + 1) * p / 100.0;
            double leftNumber = 0.0d, rightNumber = 0.0d;

            double n = p / 100.0d * (sortedData.Length - 1) + 1.0d;

            if (position >= 1)
            {
                leftNumber = sortedData[(int)Math.Floor(n) - 1];
                rightNumber = sortedData[(int)Math.Floor(n)];
            }
            else
            {
                leftNumber = sortedData[0]; // first data
                rightNumber = sortedData[1]; // first data
            }

            //if (leftNumber == rightNumber)
            if (Equals(leftNumber, rightNumber))
                return leftNumber;
            double part = n - Math.Floor(n);
            return leftNumber + part * (rightNumber - leftNumber);
        } // end of internal function percentile
        
        /// <summary>
        /// Create 1-D distance array to manipulate other function more easily
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public double[] CreateDistArray(string[,] array)
        {
            Point[] coordinates = new Point[array.GetUpperBound(0) + 1]; // extract coordinates from rawdata

            for (int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = new Point(Convert.ToDouble(array[i, 2]), Convert.ToDouble(array[i, 3]));
            }

            double[] distArray = new double[array.GetUpperBound(0) + 1]; // calculate distance between current and previous points 

            distArray[0] = 0;
            for (int i = 1; i < coordinates.Length; i++)
            {
                distArray[i] = GetDistance(coordinates[i - 1], coordinates[i]);
            }

            return distArray;
        }
        
        /// <summary>
        /// Create list data having "ObjectID", "TimeStamp", "Coordinates", "Distance" attributes
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public IList<DistancePoint> CreateDistPointsList(string[,] array)
        {
            IList<DistancePoint> distancePointsList = new List<DistancePoint>();

            double[] distArray = CreateDistArray(array);

            for (int i = 0; i < distArray.Length; i++)
            {
                distancePointsList.Add(new DistancePoint());
            }

            for (int i = 0; i < distArray.Length; i++)
            {
                distancePointsList[i].ObjectID = array[i, 0];
                distancePointsList[i].TimeStamp = Convert.ToDateTime(array[i, 1]);
                distancePointsList[i].Coordinates = new Point(Convert.ToDouble(array[i, 2]), Convert.ToDouble(array[i, 3]));
                distancePointsList[i].Distance = distArray[i];
            }

            return distancePointsList;
        }

        /// <summary>
        /// Create frequency table for pareto chart
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public IList<ParetoFreqTable> CreateFreqTable(string[,] array)
        {
            double[] distArray = CreateDistArray(array);

            Array.Sort(distArray);
            double q1 = Percentile(distArray, 25.0);
            double q3 = Percentile(distArray, 75.0);
            double IQR = q3 - q1;
            double binWidth = 2 * IQR / Math.Pow(distArray.Length, 1.0 / 3.0); // Freedman–Diaconis rule
            int binCount = Convert.ToInt32(Math.Ceiling((distArray.Max() - distArray.Min()) / binWidth));

            // Create frequency distribution table, (binWidth, frequency, cumulative frequency, cumulative percentage)
            IList<ParetoFreqTable> freqTable = new List<ParetoFreqTable>();

            for (int i = 0; i < binCount; i++)
            {
                freqTable.Add(new ParetoFreqTable());
            }

            // set section name
            freqTable[0].Section = "[0, " + binWidth.ToString("F2") + ")";
            for (int i = 1; i < freqTable.Count(); i++)
            {
                freqTable[i].Section = "[" + (binWidth * i).ToString("F2") + ", " + (binWidth * (i+1)).ToString("F2") + ")";
            }

            // set range value
            for (int i = 0; i < freqTable.Count(); i++)
            {
                freqTable[i].RangeValue = binWidth * (i + 1);
            }

            // set frequency
            for (int i = 0; i < distArray.Length; i++)
            {
                for (int j = 1; j < freqTable.Count(); j++)
                {
                    if (0 <= distArray[i] && distArray[i] < freqTable[j - 1].RangeValue)
                    {
                        freqTable[0].Freq = freqTable[0].Freq + 1;
                        break;
                    }
                    else if (freqTable[j - 1].RangeValue <= distArray[i] && distArray[i] < freqTable[j].RangeValue)
                    {
                        freqTable[j].Freq = freqTable[j].Freq + 1;
                        break;
                    }
                }
            }

            // calculate cumulative frequency
            freqTable[0].CumulFreq = freqTable[0].Freq;
            for (int i = 1; i < freqTable.Count(); i++)
            {
                freqTable[i].CumulFreq = freqTable[i - 1].CumulFreq + freqTable[i].Freq;
            }

            // calculate cumulative percentage
            for (int i = 0; i < freqTable.Count(); i++)
            {
                freqTable[i].CumulPercent = freqTable[i].CumulFreq / Convert.ToDouble(distArray.Length);
            }

            return freqTable;
        }

        /// <summary>
        /// Calculate distance filter value in order to find destination candidates points
        /// </summary>
        /// <param name="array"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public double FindFilter(IList<ParetoFreqTable> freqTable, double p)
        {
            double filter = freqTable[0].RangeValue;

            // find filter value according to TOP percengtage
            for (int i = 0; i < freqTable.Count(); i++)
            {
                if (freqTable[i].CumulPercent < (p / 100.0) && (p / 100.0) < freqTable[i + 1].CumulPercent)
                {
                    filter = freqTable[i + 1].RangeValue;
                }
            }
            
            return filter;
        }

        /// <summary>
        /// Find destination candidates points
        /// </summary>
        /// <param name="distancePointsList"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<CandidatePoint> CreateCandidatesPointsList(IList<DistancePoint> distancePointsList, double filter)
        {
            IList<CandidatePoint> candidatesPointsList = new List<CandidatePoint>();
            
            for (int i = 0; i < distancePointsList.Count(); i++)
            {
                if (distancePointsList[i].Distance < filter)
                {
                    candidatesPointsList.Add(new CandidatePoint()
                    {
                        ObjectID = distancePointsList[i].ObjectID,
                        TimeStamp = distancePointsList[i].TimeStamp,
                        Coordinates = distancePointsList[i].Coordinates
                    });
                    
                }
            }

            return candidatesPointsList;
        }

        


        // Need to change for selecting input file by user
        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            string fileDir = Directory.GetCurrentDirectory();
            txtInputPath.Text = fileDir + @"\experiment\PositionData_RTLS_2020-05-19 14-50-25_00000013.csv"; // need to change in the code
        }



        private void BtnPareto_Click(object sender, RoutedEventArgs e)
        {
            string[,] rawData = ReadCSVfile(txtInputPath.Text); // Read input CSV data
            string[,] rawData_v2 = RemoveEmptyRows(rawData); // Remove missing value rows
            string[,] rawData_v3 = RemoveDuplicateRows(rawData_v2); // Remove duplicate rows

            IList<DistancePoint> distancePointsList = CreateDistPointsList(rawData_v3); // Create coordinates data with distance 
            IList<ParetoFreqTable> freqTable = CreateFreqTable(rawData_v3);

            FilterBefore pareto = new FilterBefore(distancePointsList, freqTable);
            pareto.WindowState = FormWindowState.Maximized;
            pareto.Show();
            
        }

        IList<CandidatePoint> candidatePointsList = new List<CandidatePoint>();

        private void BtnCandidates_Click(object sender, RoutedEventArgs e)
        {
            string[,] rawData = ReadCSVfile(txtInputPath.Text); // Read input CSV data
            string[,] rawData_v2 = RemoveEmptyRows(rawData); // Remove missing value rows
            string[,] rawData_v3 = RemoveDuplicateRows(rawData_v2); // Remove duplicate rows

            IList<DistancePoint> distancePointsList = CreateDistPointsList(rawData_v3); // Create coordinates data with distance 
            IList<ParetoFreqTable> freqTable = CreateFreqTable(rawData_v3);

            double filterDistance = FindFilter(freqTable, Convert.ToDouble(txtPercent.Text)); // Find distance filter value for setting cadidates of destination
            //IList<CandidatePoint> candidatePointsList = CreateCandidatesPointsList(distancePointsList, filterDistance);
            candidatePointsList = CreateCandidatesPointsList(distancePointsList, filterDistance);

            FilterAfter distribution = new FilterAfter(candidatePointsList);
            distribution.WindowState = FormWindowState.Maximized;
            distribution.Show();
        }

        // ########################################################################################
        // #################################### For clustering ####################################
        // ########################################################################################

        public class CoordinatesData
        {
            [LoadColumn(0)]
            public int ObjectID;

            [LoadColumn(1)]
            public DateTime TimeStamp;

            [LoadColumn(0)]
            public Single X;

            [LoadColumn(1)]
            public Single Y;
        }

        public class ClusterPrediction
        {
            [ColumnName("PredictedLabel")]
            public uint PredictedClusterId;

            [ColumnName("Score")]
            public float[] Distances;
        }
        

        public IList<CandidatePoint> CreateClusterPointsList(IList<CandidatePoint> source, int numOfCluster)
        {
            IList<CandidatePoint> clusterPointsList = source;

            string filedir = Directory.GetCurrentDirectory();
            filedir = filedir + @"\CandidatesPoints_" + source[0].ObjectID + ".csv";

            var mlContext = new MLContext(seed: 0);
            
            var textLoader = mlContext.Data.CreateTextLoader(new[]
            {
                new TextLoader.Column("ObjectID", DataKind.Single, 0),
                new TextLoader.Column("TimeStamp", DataKind.Single, 1),
                new TextLoader.Column("X", DataKind.Single, 2),
                new TextLoader.Column("Y", DataKind.Single, 3),
            },
            hasHeader: false,
            separatorChar: ',');

            IDataView dataView = textLoader.Load(filedir);
            
            string featuresColumnName = "Features";

            var pipeline = (mlContext.Transforms.Concatenate(featuresColumnName, "X", "Y"))
            .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: numOfCluster));
            
            var model = pipeline.Fit(dataView);

            var predictor = mlContext.Model.CreatePredictionEngine<CoordinatesData, ClusterPrediction>(model);

            int iLength = source.Count();

            for (int i = 0; i < iLength; i++)
            {
                CoordinatesData input = new CoordinatesData();
                input.ObjectID = Convert.ToInt32(source[i].ObjectID);
                input.TimeStamp = source[i].TimeStamp;
                input.X = Convert.ToSingle(source[i].Coordinates.X);
                input.Y = Convert.ToSingle(source[i].Coordinates.Y);

                var prediction = predictor.Predict(input);
                clusterPointsList[i].ClusterNum = Convert.ToInt32(prediction.PredictedClusterId);
            }
            return clusterPointsList;
        }

        IList<CandidatePoint> clusterPointsList = new List<CandidatePoint>();

        private void BtnCluster_Click(object sender, RoutedEventArgs e)
        {
            int numOfCluster = Convert.ToInt32(txtCluster.Text);
            clusterPointsList = CreateClusterPointsList(candidatePointsList, numOfCluster);

            Cluster distribution = new Cluster(clusterPointsList);
            distribution.WindowState = FormWindowState.Maximized;
            distribution.Show();
        }

        
    }
}