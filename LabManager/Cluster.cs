using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace LabManager
{
    public partial class Cluster : Form
    {


        
        IList<MainWindow.CandidatePoint> source = new List<MainWindow.CandidatePoint>();

        public Cluster(IList<MainWindow.CandidatePoint> candidatePointsList)
        {
            InitializeComponent();
            source = candidatePointsList;
            CreateDistribution(source);
        }


        public void SaveClusterPoints(IList<MainWindow.CandidatePoint> source)
        {
            int iLength = source.Count();

            string filedir = Directory.GetCurrentDirectory();
            filedir = filedir + @"\CandidatesPoints_Cluster_" + source[0].ObjectID + ".csv";
            StreamWriter file = new StreamWriter(filedir);

            for (int i = 0; i < iLength; i++)
            {
                file.Write((i + 1) + "," + "\t" + source[i].ObjectID + "," + source[i].TimeStamp + "," + source[i].Coordinates.X + "," + source[i].Coordinates.Y + "," + source[i].ClusterNum);
                file.Write("\n");
            }
            file.Close();

            return;
        }


        public void CreateDistribution(IList<MainWindow.CandidatePoint> source)
        {
            double AxisXmax = source.Max(r => r.Coordinates.X);
            double AxisXmin = source.Min(r => r.Coordinates.X);
            double AxisYmax = source.Max(r => r.Coordinates.Y);
            double AxisYmin = source.Min(r => r.Coordinates.Y);

            distribution.ChartAreas[0].AxisX.Maximum = AxisXmax;
            distribution.ChartAreas[0].AxisX.Minimum = AxisXmin;
            distribution.ChartAreas[0].AxisY.Maximum = AxisYmax;
            distribution.ChartAreas[0].AxisY.Minimum = AxisYmin;

            distribution.Series.Clear();

            int numOfCluster = source.Max(r => r.ClusterNum);

            for (int i = 0; i < numOfCluster; i++)
            {
                string seriesNum = "Series" + (i + 1).ToString();
                distribution.Series.Add(seriesNum);
            }
            
            for (int i = 0; i < source.Count(); i++)
            {
                int clusterNum = source[i].ClusterNum;
                string seriesNum = "Series" + clusterNum.ToString();

                distribution.Series[seriesNum].Points.AddXY(source[i].Coordinates.X, source[i].Coordinates.Y);
                distribution.Series[seriesNum].Points[0].Color = Color.FromArgb(clusterNum);
                distribution.Series[seriesNum].Points[0].MarkerSize = 10;
                distribution.Series[seriesNum].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveClusterPoints(source);

            string message = "Finish saving cluster points!";
            string caption = "Post-processing";
            System.Windows.MessageBox.Show(message, caption);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
