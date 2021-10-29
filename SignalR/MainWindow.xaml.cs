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


namespace SignalR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static System.Net.Http.HttpClient client;
        public HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();
            korigang(); //SignalR
        }

        public async void korigang()
        {
            //client = new System.Net.Http.HttpClient();
            //client.DefaultRequestHeaders.Add("Accept", "application/x-www-form-urlencoded");
            //client.DefaultRequestHeaders.Add("X-Authenticate-User", "per.astrom@hd-wireless.se");
            //client.DefaultRequestHeaders.Add("X-Authenticate-Password", "!Test4All");

            //Microsoft.AspNet.SignalR.Client.ConnectionState _hub_state = await PrenPos("p186-geps-production-api.hd-rtls.com", "per.astrom@hd-wireless.se", "!Test4All", "2600000000009d40");

            string Token = await login("p186-geps-production-api.hd-rtls.com", "KTH", "!Test4KTH");

            //var q = "?X-Authenticate-Token=" + Token;
            //var hubConnection = new HubConnection("https://p186-geps-production-api.hd-rtls.com");
            //connection = new HubConnectionBuilder()
            //   .WithUrl("https://p174-geps-production-api.hd-rtls.com" + "/signalr/objectPosition" + q)
            //   .Build();

            //string Token = await login("p186-geps-production-api.hd-rtls.com", "fill in your user", "your passw");
            connection = new HubConnectionBuilder()
               .WithUrl("https://p186-geps-production-api.hd-rtls.com/signalr/beaconPosition", options =>
               {

                   options.Headers.Add("X-Authenticate-Token", Token);

               })
               .Build();

            connection.On<pos>("onEvent", Data =>
            {
                Poskommer("kkK", Data);
            });

            await connection.StartAsync();
            await connection.InvokeAsync("subscribe", null);

            //System.Net.Http.HttpResponseMessage responseGet = await client.GetAsync("https://p178-geps-production-metrics.hd-rtls.com/api/v1/labels");
            //var response = await responseGet.Content.ReadAsStringAsync();

        }

        public static void Poskommer(string server, pos p)
        {

            MessageBox.Show("Server: " + server + "  kalle hoppar");            
            //Debug.WriteLine("Server: " + server + "  kalle hoppar");
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
    }

    public class login_cred
    {
        public string ID { get; set; }
        public string AuthenticateToken { get; set; }
        public bool Isadmin { get; set; }
    }

    public class pos
    {
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Map { get; set; }
        public int Zone { get; set; }
        public string Beacon { get; set; }
        public object Object { get; set; }
        public DateTime Timestamp { get; set; }
        public object Data { get; set; }
        public object Frames { get; set; }
        public string Type { get; set; }
        public string Radio { get; set; }

    }

}
