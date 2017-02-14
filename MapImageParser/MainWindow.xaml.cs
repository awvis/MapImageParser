using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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

namespace MapImageParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            label.Content = "...";
            // Load dialog
            OpenFileDialog Dialog = new OpenFileDialog();
            Dialog.Filter = "LattitudeLongitude File (*.csv)|*.csv|All files (*.*)|*.*";
            Dialog.Multiselect = false;
            if (!(bool)Dialog.ShowDialog()) return;
            string fileName = Dialog.FileName;
            
            GetValues(fileName);

            
        }

        private void ParseUrl(List<double> lattitude, List<double> longitude, string path)
        {
            for(int i=0;i< lattitude.Count;i++)
            {
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(@"https://maps.googleapis.com/maps/api/staticmap?center=" + lattitude[i].ToString("F7") + "," + longitude[i].ToString("F7") + "&zoom=12&size=1920x1080&maptype=roadmap&key=AIzaSyAPuCX2Rxw8zU5StR9elSpvizDq_AtPYM4", path+i+".png");

                }
                catch (System.ArgumentOutOfRangeException e)
                {

                }
            }
               }

        private void GetValues(string filename)
        {
            List<double> LattitudeList = new List<double>();

            List<double> LongitudeList = new List<double>();
            LattitudeList.Clear();
            var Lines = File.ReadAllLines(filename);
            foreach (var Line in Lines)
            {
                var Segments = Line.Split(',');

                double lat = 0;
                double lon = 0;
                // Lattitude 
                bool res = double.TryParse(Segments[0], NumberStyles.Number, CultureInfo.InvariantCulture,  out lat);

                LattitudeList.Add(lat);
                if (!res) continue;

                // Longitude 
                res = double.TryParse(Segments[1], NumberStyles.Number, CultureInfo.InvariantCulture, out lon);

                LongitudeList.Add(lon);
                if (!res) continue;
            }
            label.Content = "Done Parsing";
            string directoryname = System.IO.Path.GetDirectoryName(filename);

            ParseUrl(LattitudeList, LongitudeList, filename);

        }

     
    }
}
