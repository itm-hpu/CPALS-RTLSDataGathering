using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace NewSignalR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static System.Net.Http.HttpClient client;
        public HubConnection connection;
        Controller controller = new Controller();
       
        public static ViewModel vm;

        public MainWindow()
        {
            InitializeComponent();

            vm = new ViewModel();
            DataContext = vm;

            txtServer.Text = "p184-geps-production-api.hd-rtls.com";
            txtUserName.Text = "cpal";
            txtPassword.Text = "cpal";
            txtmax_age.Text = "10";
            txtMovementVelocity.Text = "0.1";
            txtPercentage.Text = "80.0";
        }

        public async void ConnectSignalR(string option)
        {   
            string server = txtServer.Text;
            string userName = txtUserName.Text;
            string password = txtPassword.Text;

            string[] id4require = new string[3];
            id4require[0] = cmbTagID1.Text;
            id4require[1] = cmbTagID2.Text;
            id4require[2] = cmbTagID3.Text;

            double movementVelocity = Double.Parse(txtMovementVelocity.Text, CultureInfo.InvariantCulture.NumberFormat); 

            string Token = await login(server, userName, password);

            connection = new HubConnectionBuilder()
               .WithUrl("https://" + server + "/signalr/position", options =>
               {
                   options.Headers.Add("X-Authenticate-Token", Token);
               })
               .Build();

            connection.On<pos>("onPosition", Data =>
            {
                Poskommer(Data, id4require, movementVelocity);
                
            });

            await connection.StartAsync();
            await connection.InvokeAsync("subscribe");
        }


        public async void DisconnectSignalR()
        {
            if (vm.movementList1.Count > 1)
            {
                Controller.CalculateTheLastMovement(vm.movementType1, vm.movementList1);
            }
            if (vm.movementList2.Count > 1)
            {
                Controller.CalculateTheLastMovement(vm.movementType2, vm.movementList2);
            }
            if (vm.movementList3.Count > 1)
            {
                Controller.CalculateTheLastMovement(vm.movementType3, vm.movementList3);
            }
            
            await connection.StopAsync();
        }
        
        public static void Poskommer(pos p, string[] id4require, double movementVelocity)
        {
            int index = vm.zoneInfoList.FindIndex(a => a.zoneId == p.Zone);
            ObservablePosition inputforlist = new ObservablePosition
            {
                ObjectId = p.Object,
                X = p.X,
                Y = p.Y,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Timestamp = Convert.ToDateTime(p.Timestamp),
                Zone = p.Zone,
                ZoneName = (index == (-1)) ? "Transfer" : vm.zoneInfoList[index].zoneName
            };

            int dotsize = 3;
            double XAdjustCons = 10.0;
            double YAdjustCons = 150.0;
            double velocityValue = movementVelocity; // criteria value to judge movement of tag, m/s

            
            if (inputforlist.ObjectId.ToString() == id4require[0])
            {
                Application.Current.Dispatcher.Invoke(new Action(delegate 
                {
                    inputforlist.Index = vm.positionList1.Count();
                    vm.positionList1.Add(inputforlist);
                    // distance in real-time
                    if (vm.positionList1.Count > 1)
                    {
                        double dist = Controller.CalculateDistances(vm.positionList1); 
                        double velocity = Controller.CalculateVelocity(vm.positionList1);
                        string type = Controller.CheckMovementType(vm.positionList1, velocityValue);
                        double movementTime = Controller.CalculateMovementTime(vm.positionList1);
                        ObservableMovementType inputfordistlist = new ObservableMovementType {
                            ObjectId = id4require[0],
                            StartTime = vm.positionList1[vm.positionList1.Count - 2].Timestamp,
                            EndTime = vm.positionList1[vm.positionList1.Count - 1].Timestamp,
                            Distance = dist,
                            MovementTime = movementTime,
                            Velocity = velocity,
                            Type = type,
                            Zone = vm.positionList1[vm.positionList1.Count - 1].Zone,
                            ZoneName = vm.positionList1[vm.positionList1.Count - 1].ZoneName
                        };
                        inputfordistlist.Index = vm.positionList1[vm.positionList1.Count - 2].Index;
                        vm.movementType1.Add(inputfordistlist);
                    }
                    // visualization in real-time
                    if (vm.positionList1.Count > 0)
                    {
                        SolidColorBrush FillColor1 = new SolidColorBrush(Colors.Red); //FillColor1
                        vm.EllipseNodes.Add(new EllipseNode1
                        {
                            Left = (vm.positionList1[vm.positionList1.Count - 1].X + 435) / XAdjustCons, // normal (+) x axis
                            Top = (vm.positionList1[vm.positionList1.Count - 1].Y + 170) / YAdjustCons, // normal (-) y axis
                            LeftGPS = (vm.positionList1[vm.positionList1.Count - 1].Longitude - 17.633f) * 50000, // normal (+) x axis 
                            TopGPS = (vm.positionList1[vm.positionList1.Count - 1].Latitude - 59.175f) * 50000, // normal (-) y axis
                            FillColor = FillColor1,
                            Height = dotsize,
                            Width = dotsize,
                            Opacity = 1.0
                        });
                    }

                    // movement in real-time: need to modify velocity criteria, define data when "vm.movementType1.Count == 1"
                    double tempDistResult = 0.0;

                    if (vm.positionList1.Count > 1)
                    {
                        if (vm.movementType1.Count == 1 && vm.movementType1[vm.movementType1.Count - 1].Type == "Stop")
                        {
                            vm.movementList1.Add(new ObservableMovement
                            {
                                Index = vm.positionList1[vm.movementType1.Count - 1].Index,
                                ObjectId = vm.positionList1[vm.movementType1.Count - 1].ObjectId,
                                Zone = vm.positionList1[vm.movementType1.Count - 1].Zone,
                                StartTime = vm.positionList1[vm.movementType1.Count - 1].Timestamp,
                                Type = vm.movementType1[vm.movementType1.Count - 1].Type,
                                Distance = 0.0
                            });
                        }
                        else if (vm.movementType1.Count == 1 && vm.movementType1[vm.movementType1.Count - 1].Type == "Move")
                        {
                            vm.movementList1.Add(new ObservableMovement
                            {
                                Index = vm.positionList1[vm.movementType1.Count - 1].Index,
                                ObjectId = vm.positionList1[vm.movementType1.Count - 1].ObjectId,
                                Zone = vm.positionList1[vm.movementType1.Count - 1].Zone,
                                StartTime = vm.positionList1[vm.movementType1.Count - 1].Timestamp,
                                Type = vm.movementType1[vm.movementType1.Count - 1].Type,
                                Distance = vm.movementType1[vm.movementType1.Count - 1].Distance
                            });
                        }
                        
                        if (vm.movementType1.Count > 1)
                        {
                            if (vm.movementType1[vm.movementType1.Count - 2].Type != vm.movementType1[vm.movementType1.Count - 1].Type)
                            {
                                vm.movementList1.Add(new ObservableMovement
                                {
                                    Index = vm.positionList1[vm.movementType1.Count - 1].Index,
                                    ObjectId = vm.positionList1[vm.movementType1.Count - 1].ObjectId,
                                    Zone = vm.positionList1[vm.movementType1.Count - 1].Zone,
                                    StartTime = vm.positionList1[vm.movementType1.Count - 1].Timestamp,
                                    Type = vm.movementType1[vm.movementType1.Count - 1].Type,
                                    Distance = 0.0
                                });

                                if (vm.movementType1[vm.movementType1.Count - 1].Type == "Move")
                                {
                                    tempDistResult = vm.movementList1[vm.movementList1.Count - 1].Distance;
                                    tempDistResult = tempDistResult + vm.movementType1[vm.movementType1.Count - 1].Distance;
                                    vm.movementList1[vm.movementList1.Count - 1].Distance = tempDistResult;
                                }
                                else if (vm.movementType1[vm.movementType1.Count - 1].Type == "Stop")
                                {
                                    vm.movementList1[vm.movementList1.Count - 1].Distance = tempDistResult;
                                }
                            }
                            else if (vm.movementType1[vm.movementType1.Count - 2].Type == vm.movementType1[vm.movementType1.Count - 1].Type)
                            {
                                if (vm.movementType1[vm.movementType1.Count - 1].Type == "Move")
                                {
                                    tempDistResult = vm.movementList1[vm.movementList1.Count - 1].Distance;
                                    tempDistResult = tempDistResult + vm.movementType1[vm.movementType1.Count - 1].Distance;
                                    vm.movementList1[vm.movementList1.Count - 1].Distance = tempDistResult;
                                }
                                else if (vm.movementType1[vm.movementType1.Count - 1].Type == "Stop")
                                {
                                    vm.movementList1[vm.movementList1.Count - 1].Distance = tempDistResult;
                                }
                            }
                        }
                    }
                }));
            }
            else if (inputforlist.ObjectId.ToString() == id4require[1])
            {
                Application.Current.Dispatcher.Invoke(new Action(delegate
                {
                    inputforlist.Index = vm.positionList2.Count();
                    vm.positionList2.Add(inputforlist);
                    // distance in real-time
                    if (vm.positionList2.Count > 1)
                    {
                        double dist = Controller.CalculateDistances(vm.positionList2);
                        double velocity = Controller.CalculateVelocity(vm.positionList2);
                        string type = Controller.CheckMovementType(vm.positionList2, velocityValue);
                        double movementTime = Controller.CalculateMovementTime(vm.positionList2);
                        ObservableMovementType inputfordistlist = new ObservableMovementType
                        {
                            ObjectId = id4require[1],
                            StartTime = vm.positionList2[vm.positionList2.Count - 2].Timestamp,
                            EndTime = vm.positionList2[vm.positionList2.Count - 1].Timestamp,
                            Distance = dist,
                            MovementTime = movementTime,
                            Velocity = velocity,
                            Type = type,
                            Zone = vm.positionList1[vm.positionList1.Count - 1].Zone,
                            ZoneName = vm.positionList1[vm.positionList1.Count - 1].ZoneName
                        };
                        inputfordistlist.Index = vm.positionList2[vm.positionList2.Count - 2].Index;
                        vm.movementType2.Add(inputfordistlist);
                    }
                    // visualization in real-time
                    if (vm.positionList2.Count > 0)
                    {
                        SolidColorBrush FillColor2 = new SolidColorBrush(Colors.Blue); //FillColor2
                        vm.EllipseNodes.Add(new EllipseNode2
                        {
                            Left = (vm.positionList2[vm.positionList2.Count - 1].X + 435) / XAdjustCons, // normal (+) x axis
                            Top = (vm.positionList2[vm.positionList2.Count - 1].Y + 170) / YAdjustCons, // normal (-) y axis
                            LeftGPS = (vm.positionList2[vm.positionList2.Count - 1].Longitude - 17.633f) * 50000, // normal (+) x axis 
                            TopGPS = (vm.positionList2[vm.positionList2.Count - 1].Latitude - 59.175f) * 50000, // normal (-) y axis
                            FillColor = FillColor2,
                            Height = dotsize,
                            Width = dotsize,
                            Opacity = 1.0
                        });
                    }

                    // movement in real-time: need to modify velocity criteria, define data when "vm.movementType2.Count == 1"
                    double tempDistResult = 0.0;

                    if (vm.positionList2.Count > 1)
                    {
                        if (vm.movementType2.Count == 1 && vm.movementType2[vm.movementType2.Count - 1].Type == "Stop")
                        {
                            vm.movementList2.Add(new ObservableMovement
                            {
                                Index = vm.positionList2[vm.movementType2.Count - 1].Index,
                                ObjectId = vm.positionList2[vm.movementType2.Count - 1].ObjectId,
                                Zone = vm.positionList2[vm.movementType2.Count - 1].Zone,
                                StartTime = vm.positionList2[vm.movementType2.Count - 1].Timestamp,
                                Type = vm.movementType2[vm.movementType2.Count - 1].Type,
                                Distance = 0.0
                            });
                        }
                        else if (vm.movementType2.Count == 1 && vm.movementType2[vm.movementType2.Count - 1].Type == "Move")
                        {
                            vm.movementList2.Add(new ObservableMovement
                            {
                                Index = vm.positionList2[vm.movementType2.Count - 1].Index,
                                ObjectId = vm.positionList2[vm.movementType2.Count - 1].ObjectId,
                                Zone = vm.positionList2[vm.movementType2.Count - 1].Zone,
                                StartTime = vm.positionList2[vm.movementType2.Count - 1].Timestamp,
                                Type = vm.movementType2[vm.movementType2.Count - 1].Type,
                                Distance = vm.movementType2[vm.movementType2.Count - 1].Distance
                            });
                        }

                        if (vm.movementType2.Count > 1)
                        {
                            if (vm.movementType2[vm.movementType2.Count - 2].Type != vm.movementType2[vm.movementType2.Count - 1].Type)
                            {
                                vm.movementList2.Add(new ObservableMovement
                                {
                                    Index = vm.positionList2[vm.movementType2.Count - 1].Index,
                                    ObjectId = vm.positionList2[vm.movementType2.Count - 1].ObjectId,
                                    Zone = vm.positionList2[vm.movementType2.Count - 1].Zone,
                                    StartTime = vm.positionList2[vm.movementType2.Count - 1].Timestamp,
                                    Type = vm.movementType2[vm.movementType2.Count - 1].Type,
                                    Distance = 0.0
                                });

                                if (vm.movementType2[vm.movementType2.Count - 1].Type == "Move")
                                {
                                    tempDistResult = vm.movementList2[vm.movementList2.Count - 1].Distance;
                                    tempDistResult = tempDistResult + vm.movementType2[vm.movementType2.Count - 1].Distance;
                                    vm.movementList2[vm.movementList2.Count - 1].Distance = tempDistResult;
                                }
                                else if (vm.movementType2[vm.movementType2.Count - 1].Type == "Stop")
                                {
                                    vm.movementList2[vm.movementList2.Count - 1].Distance = tempDistResult;
                                }
                            }
                            else if (vm.movementType2[vm.movementType2.Count - 2].Type == vm.movementType2[vm.movementType2.Count - 1].Type)
                            {
                                if (vm.movementType2[vm.movementType2.Count - 1].Type == "Move")
                                {
                                    tempDistResult = vm.movementList2[vm.movementList2.Count - 1].Distance;
                                    tempDistResult = tempDistResult + vm.movementType2[vm.movementType2.Count - 1].Distance;
                                    vm.movementList2[vm.movementList2.Count - 1].Distance = tempDistResult;
                                }
                                else if (vm.movementType2[vm.movementType2.Count - 1].Type == "Stop")
                                {
                                    vm.movementList2[vm.movementList2.Count - 1].Distance = tempDistResult;
                                }
                            }
                        }
                    }
                }));
            }
            else if (inputforlist.ObjectId.ToString() == id4require[2])
            {
                Application.Current.Dispatcher.Invoke(new Action(delegate
                {
                    inputforlist.Index = vm.positionList3.Count();
                    vm.positionList3.Add(inputforlist);
                    // distance in real-time
                    if (vm.positionList3.Count > 1)
                    {
                        double dist = Controller.CalculateDistances(vm.positionList3);
                        double velocity = Controller.CalculateVelocity(vm.positionList3);
                        string type = Controller.CheckMovementType(vm.positionList3, velocityValue);
                        double movementTime = Controller.CalculateMovementTime(vm.positionList3);
                        ObservableMovementType inputfordistlist = new ObservableMovementType
                        {
                            ObjectId = id4require[2],
                            StartTime = vm.positionList3[vm.positionList3.Count - 2].Timestamp,
                            EndTime = vm.positionList3[vm.positionList3.Count - 1].Timestamp,
                            Distance = dist,
                            MovementTime = movementTime,
                            Velocity = velocity,
                            Type = type,
                            Zone = vm.positionList3[vm.positionList3.Count - 1].Zone,
                            ZoneName = vm.positionList3[vm.positionList3.Count - 1].ZoneName
                        };
                        inputfordistlist.Index = vm.positionList3[vm.positionList3.Count - 2].Index;
                        vm.movementType3.Add(inputfordistlist);
                    }
                    // visualization in real-time
                    if (vm.positionList3.Count > 0)
                    {
                        SolidColorBrush FillColor3 = new SolidColorBrush(Colors.Green); //FillColor2
                        vm.EllipseNodes.Add(new EllipseNode3
                        {
                            Left = (vm.positionList3[vm.positionList3.Count - 1].X + 435) / XAdjustCons, // normal (+) x axis
                            Top = (vm.positionList3[vm.positionList3.Count - 1].Y + 170) / YAdjustCons, // normal (-) y axis
                            LeftGPS = (vm.positionList3[vm.positionList3.Count - 1].Longitude - 17.633f) * 50000, // normal (+) x axis 
                            TopGPS = (vm.positionList3[vm.positionList3.Count - 1].Latitude - 59.175f) * 50000, // normal (-) y axis
                            FillColor = FillColor3,
                            Height = dotsize,
                            Width = dotsize,
                            Opacity = 1.0
                        });
                    }

                    // movement in real-time: need to modify velocity criteria, define data when "vm.movementType2.Count == 1"
                    double tempDistResult = 0.0;

                    if (vm.positionList3.Count > 1)
                    {
                        if (vm.movementType3.Count == 1 && vm.movementType3[vm.movementType3.Count - 1].Type == "Stop")
                        {
                            vm.movementList3.Add(new ObservableMovement
                            {
                                Index = vm.positionList3[vm.movementType3.Count - 1].Index,
                                ObjectId = vm.positionList3[vm.movementType3.Count - 1].ObjectId,
                                Zone = vm.positionList3[vm.movementType3.Count - 1].Zone,
                                StartTime = vm.positionList3[vm.movementType3.Count - 1].Timestamp,
                                Type = vm.movementType3[vm.movementType3.Count - 1].Type,
                                Distance = 0.0
                            });
                        }
                        else if (vm.movementType3.Count == 1 && vm.movementType3[vm.movementType3.Count - 1].Type == "Move")
                        {
                            vm.movementList3.Add(new ObservableMovement
                            {
                                Index = vm.positionList3[vm.movementType3.Count - 1].Index,
                                ObjectId = vm.positionList3[vm.movementType3.Count - 1].ObjectId,
                                Zone = vm.positionList3[vm.movementType3.Count - 1].Zone,
                                StartTime = vm.positionList3[vm.movementType3.Count - 1].Timestamp,
                                Type = vm.movementType3[vm.movementType3.Count - 1].Type,
                                Distance = vm.movementType3[vm.movementType3.Count - 1].Distance
                            });
                        }

                        if (vm.movementType3.Count > 1)
                        {
                            if (vm.movementType3[vm.movementType3.Count - 2].Type != vm.movementType3[vm.movementType3.Count - 1].Type)
                            {
                                vm.movementList3.Add(new ObservableMovement
                                {
                                    Index = vm.positionList3[vm.movementType3.Count - 1].Index,
                                    ObjectId = vm.positionList3[vm.movementType3.Count - 1].ObjectId,
                                    Zone = vm.positionList3[vm.movementType3.Count - 1].Zone,
                                    StartTime = vm.positionList3[vm.movementType3.Count - 1].Timestamp,
                                    Type = vm.movementType3[vm.movementType3.Count - 1].Type,
                                    Distance = 0.0
                                });

                                if (vm.movementType3[vm.movementType3.Count - 1].Type == "Move")
                                {
                                    tempDistResult = vm.movementList3[vm.movementList3.Count - 1].Distance;
                                    tempDistResult = tempDistResult + vm.movementType3[vm.movementType3.Count - 1].Distance;
                                    vm.movementList3[vm.movementList3.Count - 1].Distance = tempDistResult;
                                }
                                else if (vm.movementType3[vm.movementType3.Count - 1].Type == "Stop")
                                {
                                    vm.movementList3[vm.movementList3.Count - 1].Distance = tempDistResult;
                                }
                            }
                            else if (vm.movementType3[vm.movementType3.Count - 2].Type == vm.movementType3[vm.movementType3.Count - 1].Type)
                            {
                                if (vm.movementType3[vm.movementType3.Count - 1].Type == "Move")
                                {
                                    tempDistResult = vm.movementList3[vm.movementList3.Count - 1].Distance;
                                    tempDistResult = tempDistResult + vm.movementType3[vm.movementType3.Count - 1].Distance;
                                    vm.movementList3[vm.movementList3.Count - 1].Distance = tempDistResult;
                                }
                                else if (vm.movementType3[vm.movementType3.Count - 1].Type == "Stop")
                                {
                                    vm.movementList3[vm.movementList3.Count - 1].Distance = tempDistResult;
                                }
                            }
                        }
                    }
                }));
            }
        }


        public async Task<string> login(string server, string user, string passw)
        {
            client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("X-Authenticate-User", user);
            client.DefaultRequestHeaders.Add("X-Authenticate-Password", passw);
            StringContent content = new System.Net.Http.StringContent("{\"Id\": \"" + user + "\", \"Password\": \"" + passw + "\",\"IsAdmin\": true}", Encoding.UTF8, "text/json");
            HttpResponseMessage response = await client.PostAsync("https://" + server + "/login/", content);
            login_cred result = JsonConvert.DeserializeObject<login_cred>(await response.Content.ReadAsStringAsync());
            return result.AuthenticateToken;
        }


        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            string objectIDsAddress = "https://" + txtServer.Text + "/objects";
            txtTagID.Text = controller.GetID(objectIDsAddress, txtUserName.Text, txtPassword.Text);

            string[] objectIDs = controller.DivideIDs(txtTagID.Text);
            cmbTagID1.ItemsSource = objectIDs;
            cmbTagID2.ItemsSource = objectIDs;
            cmbTagID3.ItemsSource = objectIDs;

            cmbObjectForDistance1.ItemsSource = objectIDs;
            cmbObjectForDistance2.ItemsSource = objectIDs;
            cmbObjectForDistance3.ItemsSource = objectIDs;
            cmbAggregation.ItemsSource = new string[] { "None", "Sum" };

            cmbinCollisionTagID1.ItemsSource = objectIDs;
            cmbinCollisionTagID2.ItemsSource = objectIDs;
            cmbinCollisionTagID3.ItemsSource = objectIDs;

            string zoneIDsAddress = "https://" + txtServer.Text + "/locations/zones";
            List<ZoneInfo> zoneInfos = controller.GetZoneInfo(zoneIDsAddress, txtUserName.Text, txtPassword.Text);
            for (int i = 0; i < zoneInfos.Count; i++)
            {
                vm.zoneInfoList.Add(new ZoneInfo
                {
                    zoneId = zoneInfos[i].zoneId,
                    zoneName = zoneInfos[i].zoneName,
                    zoneBoundary = zoneInfos[i].zoneBoundary
                });
            };

            string message = "Complete to check \"" + txtServer.Text +"\" server infos!";
            MessageBox.Show(message);
        }

        private void BtnDebbug_Click(object sender, RoutedEventArgs e)
        {

        }

        //-----------------------------
        // # Tab Position
        //-----------------------------

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ConnectSignalR("subscribe");
            lbObjectID1.Content = cmbTagID1.Text;
            lbObjectID2.Content = cmbTagID2.Text;
            lbObjectID3.Content = cmbTagID3.Text;
            lbObjectIDinRM1.Content = cmbTagID1.Text;
            lbObjectIDinRM2.Content = cmbTagID2.Text;
            lbObjectIDinRM3.Content = cmbTagID3.Text;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            DisconnectSignalR();

            string message = "Stop acquiring data!";
            MessageBox.Show(message);
        }
        
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            controller.SaveDataToTextFile(vm.positionList1);
            controller.SaveDataToTextFile(vm.positionList2);
            controller.SaveDataToTextFile(vm.positionList3);
            controller.SaveDataToTextFile(vm.movementType1);
            controller.SaveDataToTextFile(vm.movementType2);
            controller.SaveDataToTextFile(vm.movementType3);

            string message = "Save a acquired data!";
            MessageBox.Show(message);
        }

        //-----------------------------
        // # Tab Real-time Movement
        //-----------------------------

        private void BtnSaveInMovement_Click(object sender, RoutedEventArgs e)
        {
            controller.SaveDataToTextFile(vm.movementList1);
            controller.SaveDataToTextFile(vm.movementList2);
            controller.SaveDataToTextFile(vm.movementList3);

            string message = "Save a acquired data!";
            MessageBox.Show(message);
        }

        //-----------------------------
        // # Tab Refined Movement
        //-----------------------------

        private void BtnAnalyzeInRedefinedMovement_Click(object sender, RoutedEventArgs e)
        {
            if (connection.State == 0) // Disconnected
            {
                double p = Double.Parse(txtPercentage.Text, CultureInfo.InvariantCulture.NumberFormat);

                vm.velocityOfZones1 = controller.CalcPercentileVelocityOfZones(vm.movementType1, p);
                vm.velocityOfZones2 = controller.CalcPercentileVelocityOfZones(vm.movementType2, p);
                vm.velocityOfZones3 = controller.CalcPercentileVelocityOfZones(vm.movementType3, p);

                vm.redefinedMovementType1 = controller.CreateRedefinedMovementList(vm.movementType1, vm.velocityOfZones1, p);
                vm.redefinedMovementType2 = controller.CreateRedefinedMovementList(vm.movementType2, vm.velocityOfZones2, p);
                vm.redefinedMovementType3 = controller.CreateRedefinedMovementList(vm.movementType3, vm.velocityOfZones3, p);

                lbxRedefinedMovementType1.ItemsSource = vm.redefinedMovementType1;
                lbxRedefinedMovementType2.ItemsSource = vm.redefinedMovementType2;
                lbxRedefinedMovementType3.ItemsSource = vm.redefinedMovementType3;
            }
            else
            {
                string message = "Please disconnect signalR first";
                MessageBox.Show(message);
            }

        }

        private void BtnSaveInRedefinedMovement_Click(object sender, RoutedEventArgs e)
        {

        }

        //-----------------------------
        // # Collisions - under progressing
        //-----------------------------
        public static void DetectCollisions(string objectID)
        {

        }
        
        private void BtnStartInCollision_Click(object sender, RoutedEventArgs e)
        {
            string targetID = cmbinCollisionTagID1.Text;

            DetectCollisions(targetID);
        }

        private void BtnStopInCollision_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveInCollision_Click(object sender, RoutedEventArgs e)
        {

        }

        //-----------------------------
        // # Distance (historical) 
        //-----------------------------

        private void BtnDistance_Click(object sender, RoutedEventArgs e)
        {
            string distanceAddress = "https://" + txtServer.Text + "/time-series/distance/points?order_by=Id&order=Ascending";

            int max_age = Convert.ToInt32(txtmax_age.Text);
            string aggregation = cmbAggregation.Text;

            string objectID1 = cmbObjectForDistance1.Text;
            string objectID2 = cmbObjectForDistance2.Text;
            string objectID3 = cmbObjectForDistance3.Text;

            vm.distances1 = controller.GetDistance(distanceAddress, txtUserName.Text, txtPassword.Text, objectID1, max_age, aggregation);
            vm.distances2 = controller.GetDistance(distanceAddress, txtUserName.Text, txtPassword.Text, objectID2, max_age, aggregation);
            vm.distances3 = controller.GetDistance(distanceAddress, txtUserName.Text, txtPassword.Text, objectID3, max_age, aggregation);

            txtObjectForDistance1.Text = "Object, Timestamp, Distance" + "\n";
            txtObjectForDistance2.Text = "Object, Timestamp, Distance" + "\n";
            txtObjectForDistance3.Text = "Object, Timestamp, Distance" + "\n";

            for (int i = 0; i < vm.distances1.Count(); i++)
            {
                txtObjectForDistance1.Text = txtObjectForDistance1.Text + vm.distances1[i].ObjectId + ", " + vm.distances1[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " + vm.distances1[i].Value + "\n";
                txtObjectForDistance1.ScrollToEnd();
            }
            for (int i = 0; i < vm.distances2.Count(); i++)
            {
                txtObjectForDistance2.Text = txtObjectForDistance2.Text + vm.distances2[i].ObjectId + ", " + vm.distances2[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " + vm.distances2[i].Value + "\n";
                txtObjectForDistance2.ScrollToEnd();
            }
            for (int i = 0; i < vm.distances3.Count(); i++)
            {
                txtObjectForDistance3.Text = txtObjectForDistance3.Text + vm.distances3[i].ObjectId + ", " + vm.distances3[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " + vm.distances3[i].Value + "\n";
                txtObjectForDistance3.ScrollToEnd();
            }
        }

        private void BtnSaveDistance_Click(object sender, RoutedEventArgs e)
        {
            controller.SaveDataToTextFile(vm.distances1);
            controller.SaveDataToTextFile(vm.distances2);
            controller.SaveDataToTextFile(vm.distances3);

            string message = "Save a acquired data!";
            MessageBox.Show(message);
        }


        // under progressing for visualization
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


    }


    public class login_cred
    {
        public string ID { get; set; }
        public string AuthenticateToken { get; set; }
        public bool Isadmin { get; set; }
    }

    public class pos
    {
        public string Beacon { get; set; }
        public string Box { get; set; }
        public string Object { get; set; }
        public string Device { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Map { get; set; }
        public int Building { get; set; }
        public int Zone { get; set; }
        public int Site { get; set; }
        public string Timestamp { get; set; }
        public string ServerTimestamp { get; set; }
        public int Flags { get; set; }
        public List<object> Frames { get; set; }
        public string Type { get; set; }
        public string Radio { get; set; }
        public string Container { get; set; }
        public Data Data { get; set; }
        public int State { get; set; }
        public Object Events { get; set; }
        public Velocity Velocity { get; set; }
        public float Distance { get; set; }
        public int Quality { get; set; }
    }
    public class Data
    {

        public Acceleration Acceleration { get; set; }
        public Temperature Temperature { get; set; }
        public Battery Battery { get; set; }
        public Compass Compass { get; set; }
    }
    public class Velocity
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Value { get; set; }
    }
    public class Acceleration
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string Timestamp { get; set; }
    }
    public class Temperature
    {
        public float Celsius { get; set; }
        public string Timestamp { get; set; }
    }
    public class Battery
    {
        public float level { get; set; }
        public float Voltage { get; set; }
        public string Timestamp { get; set; }
    }
    public class Compass
    {
        public float Degrees { get; set; }
        public string Timestamp { get; set; }
    }

    public class EllipseNode1 : ObservablePosition
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double LeftGPS { get; set; }
        public double TopGPS { get; set; }
        public SolidColorBrush FillColor { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public double Opacity { get; set; }
    }

    public class EllipseNode2 : ObservablePosition
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double LeftGPS { get; set; }
        public double TopGPS { get; set; }
        public SolidColorBrush FillColor { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public double Opacity { get; set; }
    }

    public class EllipseNode3 : ObservablePosition
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double LeftGPS { get; set; }
        public double TopGPS { get; set; }
        public SolidColorBrush FillColor { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public double Opacity { get; set; }
    }

    public class VelocityOyZone
    {
        public int Zone { get; set; }
        public string ZoneName { get; set; }
        public double Velocity { get; set; }
    }


    public class ViewModel
    {
        public ObservableCollection<ObservablePosition> positionList1 { get; set; } = new ObservableCollection<ObservablePosition>();
        public ObservableCollection<ObservablePosition> positionList2 { get; set; } = new ObservableCollection<ObservablePosition>();
        public ObservableCollection<ObservablePosition> positionList3 { get; set; } = new ObservableCollection<ObservablePosition>();

        public ObservableCollection<ObservableMovementType> movementType1 { get; set; } = new ObservableCollection<ObservableMovementType>();
        public ObservableCollection<ObservableMovementType> movementType2 { get; set; } = new ObservableCollection<ObservableMovementType>();
        public ObservableCollection<ObservableMovementType> movementType3 { get; set; } = new ObservableCollection<ObservableMovementType>();

        public ObservableCollection<ObservableMovement> movementList1 { get; set; } = new ObservableCollection<ObservableMovement>();
        public ObservableCollection<ObservableMovement> movementList2 { get; set; } = new ObservableCollection<ObservableMovement>();
        public ObservableCollection<ObservableMovement> movementList3 { get; set; } = new ObservableCollection<ObservableMovement>();
        
        public List<ZoneInfo> zoneInfoList { get; set; } = new List<ZoneInfo>();

        public List<VelocityOyZone> velocityOfZones1 { get; set; } = new List<VelocityOyZone>();
        public List<VelocityOyZone> velocityOfZones2 { get; set; } = new List<VelocityOyZone>();
        public List<VelocityOyZone> velocityOfZones3 { get; set; } = new List<VelocityOyZone>();

        public ObservableCollection<ObservableRedefinedMovementType> redefinedMovementType1 { get; set; } = new ObservableCollection<ObservableRedefinedMovementType>();
        public ObservableCollection<ObservableRedefinedMovementType> redefinedMovementType2 { get; set; } = new ObservableCollection<ObservableRedefinedMovementType>();
        public ObservableCollection<ObservableRedefinedMovementType> redefinedMovementType3 { get; set; } = new ObservableCollection<ObservableRedefinedMovementType>();

        public ObservableCollection<ObservableRedefinedMovement> redefinedMovement1 { get; set; } = new ObservableCollection<ObservableRedefinedMovement>();
        public ObservableCollection<ObservableRedefinedMovement> redefinedMovement2 { get; set; } = new ObservableCollection<ObservableRedefinedMovement>();
        public ObservableCollection<ObservableRedefinedMovement> redefinedMovement3 { get; set; } = new ObservableCollection<ObservableRedefinedMovement>();

        public ObservableCollection<ObservablePosition> EllipseNodes { get; set; } = new ObservableCollection<ObservablePosition>();

        public ObservableCollection<ObservableCollision> collisionList1 { get; set; } = new ObservableCollection<ObservableCollision>();

        public List<Distance> distances1 { get; set; } = new List<Distance>();
        public List<Distance> distances2 { get; set; } = new List<Distance>();
        public List<Distance> distances3 { get; set; } = new List<Distance>();
    }
}
