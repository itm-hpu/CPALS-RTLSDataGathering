using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;

namespace LabManager
{
    public partial class FilterBefore : Form
    {
        public FilterBefore(IList<MainWindow.DistancePoint> distancePointsList, IList<MainWindow.ParetoFreqTable> freqTable)
        {
            InitializeComponent();
            CreateDistribution(distancePointsList);
            CreatePareto(freqTable);
        }

        public void CreateDistribution(IList<MainWindow.DistancePoint> distancePointsList)
        {
            //distribution.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            //distribution.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;

            double AxisXmax = distancePointsList.Max(r => r.Coordinates.X);
            double AxisXmin = distancePointsList.Min(r => r.Coordinates.X);
            double AxisYmax = distancePointsList.Max(r => r.Coordinates.Y);
            double AxisYmin = distancePointsList.Min(r => r.Coordinates.Y);

            distribution.ChartAreas[0].AxisX.Maximum = AxisXmax;
            distribution.ChartAreas[0].AxisX.Minimum = AxisXmin;
            distribution.ChartAreas[0].AxisY.Maximum = AxisYmax;
            distribution.ChartAreas[0].AxisY.Minimum = AxisYmin;

            for (int i = 0; i < distancePointsList.Count(); i++)
            {
                distribution.Series["Series1"].Points.AddXY(distancePointsList[i].Coordinates.X, distancePointsList[i].Coordinates.Y);
            }
        }

        public void CreatePareto(IList<MainWindow.ParetoFreqTable> freqTable)
        {
            pareto.ChartAreas[0].AxisX.LabelStyle.Interval = 1;

            for (int i = 0; i < freqTable.Count(); i++)
            {
                pareto.Series["Frequency"].Points.AddXY(freqTable[i].Section, freqTable[i].Freq);
                pareto.Series["Percent"].Points.AddXY(freqTable[i].Section, freqTable[i].CumulPercent * 100.0);
            }
        }
    }
}
