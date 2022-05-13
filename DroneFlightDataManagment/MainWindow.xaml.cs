using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Net;
using OxyPlot;
using OxyPlot.Series;
using System.Reflection;
using System.Windows.Threading;

namespace DroneFlightDataManagment
{
    public partial class MainWindow : Window
    {
        private string[] allLinesOfCSVFile;

        public MapProviders mapProvider = MapProviders.GoogleTerrainMap;

        public Statistics flightsStatistics { get; set; } = new Statistics();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Initialize VideoControls
            grdVideo.Visibility = Visibility.Hidden;

            //Initialize MapControls
            try
            {
                System.Net.IPHostEntry h = System.Net.Dns.GetHostEntry("www.google.com");
                mapControl1.Manager.Mode = AccessMode.ServerAndCache;
            }
            catch
            {
                mapControl1.Manager.Mode = AccessMode.CacheOnly;

                MessageBox.Show("No Internet connection available, going to CacheOnly mode.", "GMap.NET Demo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            mapControl1.MapProvider = GMapProviders.GoogleTerrainMap;
            mapControl1.ShowCenter = false;
            mapControl1.DragButton = MouseButton.Left;
            mapControl1.SetPositionByKeywords("Athens , Greece");
        }

        #region FILE HANDLING
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            List<Flight> lf = new List<Flight>();
            lf = FileOperation.OpenBinaryObject<List<Flight>>();
            if (lf != null)
            {
                foreach (Flight item in lf)
                {
                    if (listBoxFlights.Items.Contains(item) == false)
                    {
                        listBoxFlights.Items.Add(item);
                        //item.VideoFiles = new List<string>();
                    }
                }
            }
            btnRefreshStats_Click(this, null);
            DrawAllMapMarkers();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            List<Flight> lf = new List<Flight>();
            foreach (var item in listBoxFlights.Items)
            {
                lf.Add((Flight)item);
            }
            FileOperation.SaveObjectToBinary<List<Flight>>(lf);
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            //Read the CSV File
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true && openFileDialog.FileNames != null)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    allLinesOfCSVFile = File.ReadAllLines(file);

                    //Create a new list of logs
                    List<Log> listOfLogs = new List<Log>();

                    //Map data to a log instance 
                    string[] firstLine = allLinesOfCSVFile[1].Split(',');

                    for (int i = 1; i < allLinesOfCSVFile.Length; i++)
                    {
                        Log log = new Log();
                        string[] currentLineOfCSVFile = allLinesOfCSVFile[i].Split(',');
                        #region Mapping
                        log.LogID = i;
                        log.Latitude = float.Parse(currentLineOfCSVFile[0]);
                        log.Longitude = float.Parse(currentLineOfCSVFile[1]);
                        log.Altitude = int.Parse(currentLineOfCSVFile[2]);
                        log.Ascent = int.Parse(currentLineOfCSVFile[3]);
                        log.Speed = float.Parse(currentLineOfCSVFile[4]);
                        log.Distance = int.Parse(currentLineOfCSVFile[5]);
                        log.MaxAltitude = int.Parse(currentLineOfCSVFile[6]);
                        log.MaxAscent = int.Parse(currentLineOfCSVFile[7]);
                        log.MaxSpeed = float.Parse(currentLineOfCSVFile[8]);
                        log.MaxDistance = int.Parse(currentLineOfCSVFile[9]);
                        log.Time = int.Parse(currentLineOfCSVFile[10]);
                        log.UTCDateTime = DateTime.Parse(currentLineOfCSVFile[11]);
                        log.LocalDateTime = DateTime.Parse(currentLineOfCSVFile[12]);
                        log.Satellites = int.Parse(currentLineOfCSVFile[13]);
                        //log.Pressure = float.Parse(currentLineOfCSVFile[14]);
                        //log.Temperature = float.Parse(currentLineOfCSVFile[15]);
                        //log.Voltage = float.Parse(currentLineOfCSVFile[16]);
                        log.HomeLatitude = float.Parse(currentLineOfCSVFile[17]);
                        log.HomeLongitude = float.Parse(currentLineOfCSVFile[18]);
                        log.VelocityX = float.Parse(currentLineOfCSVFile[19]);
                        log.VelocityY = float.Parse(currentLineOfCSVFile[20]);
                        log.VelocityZ = float.Parse(currentLineOfCSVFile[21]);
                        log.Pitch = int.Parse(currentLineOfCSVFile[22]);
                        log.Roll = int.Parse(currentLineOfCSVFile[23]);
                        log.Yaw = int.Parse(currentLineOfCSVFile[24]);
                        //log.PowerLevel = int.Parse(currentLineOfCSVFile[25]);
                        log.IsFlying = bool.Parse(int.Parse(currentLineOfCSVFile[26]) == 1 ? "True" : "False");
                        log.IsTakingPhoto = bool.Parse(int.Parse(currentLineOfCSVFile[27]) == 1 ? "True" : "False");
                        log.RemainPowerPercent = byte.Parse(currentLineOfCSVFile[28]);
                        log.RemainLifePercent = byte.Parse(currentLineOfCSVFile[29]);
                        log.CurrentCurrent = int.Parse(currentLineOfCSVFile[30]);
                        log.CurrentElectricity = int.Parse(currentLineOfCSVFile[31]);
                        log.CurrentVoltage = int.Parse(currentLineOfCSVFile[32]);
                        log.BatteryTemperature = float.Parse(currentLineOfCSVFile[33]);
                        log.DischargeCount = byte.Parse(currentLineOfCSVFile[34]);
                        log.Flightmode = currentLineOfCSVFile[35];
                        log.IsMotorsOn = bool.Parse(int.Parse(currentLineOfCSVFile[36]) == 1 ? "True" : "False");
                        log.IsTakingVideo = bool.Parse(int.Parse(currentLineOfCSVFile[37]) == 1 ? "True" : "False");
                        log.RcElevator = int.Parse(currentLineOfCSVFile[38]);
                        log.RcAileron = int.Parse(currentLineOfCSVFile[39]);
                        log.RcThrottle = int.Parse(currentLineOfCSVFile[40]);
                        log.RcRudder = int.Parse(currentLineOfCSVFile[41]);
                        log.RcGyro = int.Parse(currentLineOfCSVFile[42]);
                        log.TimeStamp = long.Parse(currentLineOfCSVFile[43]);
                        log.BatteryCell1 = int.Parse(currentLineOfCSVFile[44]);
                        log.BatteryCell2 = int.Parse(currentLineOfCSVFile[45]);
                        log.BatteryCell3 = int.Parse(currentLineOfCSVFile[46]);
                        log.BatteryCell4 = int.Parse(currentLineOfCSVFile[47]);
                        log.BatteryCell5 = int.Parse(currentLineOfCSVFile[48]);
                        log.BatteryCell6 = int.Parse(currentLineOfCSVFile[49]);
                        log.DroneType = int.Parse(currentLineOfCSVFile[50]);
                        log.AppVersion = firstLine[51];
                        log.PlaneName = firstLine[52];
                        log.FlyControllerSerialNumber = firstLine[53];
                        log.RemoteSerialNumber = firstLine[54];
                        //log.BatterySerialNumber = firstLine[55];
                        log.CenterBatteryProductDate = firstLine[56];
                        log.CenterBatterySerialNo = firstLine[57];
                        log.CenterBatteryFullCapacity = int.Parse(firstLine[58]);
                        log.CenterBatteryProductDateRaw = int.Parse(firstLine[59]);
                        log.PitchRaw = int.Parse(currentLineOfCSVFile[60]);
                        log.RollRaw = int.Parse(currentLineOfCSVFile[61]);
                        log.YawRaw = int.Parse(currentLineOfCSVFile[62]);
                        log.GimbalPitchRaw = int.Parse(currentLineOfCSVFile[63]);
                        log.GimbalRollRaw = int.Parse(currentLineOfCSVFile[64]);
                        log.GimbalYawRaw = int.Parse(currentLineOfCSVFile[65]);
                        log.FlyState = int.Parse(currentLineOfCSVFile[66]);
                        log.AltitudeRaw = int.Parse(currentLineOfCSVFile[67]);
                        log.SpeedRaw = float.Parse(currentLineOfCSVFile[68]);
                        log.DistanceRaw = float.Parse(currentLineOfCSVFile[69]);
                        log.VelocityXRaw = int.Parse(currentLineOfCSVFile[70]);
                        log.VelocityYRaw = int.Parse(currentLineOfCSVFile[71]);
                        log.VelocityZRaw = int.Parse(currentLineOfCSVFile[72]);
                        //log.DataReuse = byte.Parse(currentLineOfCSVFile[73]);
                        log.AppTip = currentLineOfCSVFile[74];
                        log.AppWarning = currentLineOfCSVFile[75];
                        log.DownlinkSignalQuality = int.Parse(currentLineOfCSVFile[76]);
                        log.UplinkSignalQuality = int.Parse(currentLineOfCSVFile[77]);
                        log.TransmissionChannel = int.Parse(currentLineOfCSVFile[78]);
                        #endregion
                        // Add log to list of logs.
                        listOfLogs.Add(log);
                    }

                    //Add flight to list
                    Flight item = new Flight() { Logs = listOfLogs };
                    if (listBoxFlights.Items.Contains(item) == false)
                    {
                        listBoxFlights.Items.Add(item);
                    }
                    //Sort List
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listBoxFlights.Items);
                    view.SortDescriptions.Add(new SortDescription("FlightDate", ListSortDirection.Descending));
                }
                btnRefreshStats_Click(this, null);
            }
            else
            {
                MessageBox.Show("No file selected");
                return;
            }
        }
        #endregion

        #region listBoxFlights HANDLING
        private void btnCheckUnCheck_Click(object sender, RoutedEventArgs e)
        {
            CollectionView list = (CollectionView)CollectionViewSource.GetDefaultView(listBoxFlights.Items);
            if (list != null)
                foreach (Flight fl in list)
                {
                    if ((sender as Button).Name == "btnCheckAll")
                    {
                        fl.IsChecked = true;
                    }
                    else
                    {
                        fl.IsChecked = false;
                    }
                }
            btnRefreshStats_Click(this, null);
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            listBoxFlights.Items.Clear();
            btnRefreshStats_Click(this, null);
        }

        private void btnRemoveChecked_Click(object sender, RoutedEventArgs e)
        {
            CollectionView list = (CollectionView)CollectionViewSource.GetDefaultView(listBoxFlights.Items);

            List<Flight> newList = new List<Flight>();

            if (list != null)
                foreach (Flight fl in list)
                {
                    if (fl.IsChecked != true)
                        newList.Add(fl);
                }
            listBoxFlights.Items.Clear();

            foreach (Flight fl in newList)
            {
                listBoxFlights.Items.Add(fl);
            }
            listBoxFlights.Items.Refresh();

            btnRefreshStats_Click(this, null);
        }

        private void listBoxFlights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Flight fl = (Flight)(listBoxFlights.SelectedItem);
            if (fl != null)
            {
                PointLatLng point = new PointLatLng(fl.Logs.First().HomeLatitude, fl.Logs.First().HomeLongitude);
                mapControl1.Position = point;
                DrawPath();
                DrawGraph();
            }
        }

        private void menuItem_SortByProperty_Click(object sender, RoutedEventArgs e)
        {
            string propertyName = (sender as MenuItem).Name.ToString().Remove(0, 8); //Remove the "menuItem" from sender name.
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(listBoxFlights.Items);
            cv.SortDescriptions.Clear();
            cv.SortDescriptions.Add(new SortDescription("IsChecked", ListSortDirection.Descending));
            cv.SortDescriptions.Add(new SortDescription(propertyName, ListSortDirection.Descending));

            foreach (FrameworkElement item in contextMenuSort.Items)
            {
                item.Style = (Style)TryFindResource("contextMenuNormalStyle");
            }
             (sender as FrameworkElement).Style = (Style)TryFindResource("contextMenuSelectedStyle");
        }

        private void textBoxTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            listBoxFlights.Items.Refresh();
        }
        #endregion

        #region listBoxMessage HANDLING
        private void listBoxMessage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            Log log = (Log)(listBox.SelectedItem);
            if (log != null)
            {
                PointLatLng point = new PointLatLng(log.Latitude, log.Longitude);
                mapControl1.Position = point;
                GMapMarker marker = new GMapMarker(point);
                Image img = new Image();

                switch (listBox.Name)
                {
                    case "listBoxAppTips":
                        img.Source = new BitmapImage(new Uri("/Resources/TipIcon.png", UriKind.Relative));
                        break;
                    case "listBoxAppWarnings":
                        img.Source = new BitmapImage(new Uri("/Resources/WarningIcon.png", UriKind.Relative));
                        break;
                    case "listBoxVideos":
                        img.Source = new BitmapImage(new Uri("/Resources/VideoIcon.png", UriKind.Relative));
                        break;
                    case "listBoxPhotos":
                        img.Source = new BitmapImage(new Uri("/Resources/PhotoIcon.png", UriKind.Relative));
                        break;
                    default:
                        break;
                }
                marker.Shape = img;
                mapControl1.Markers.Add(marker);
            }
        }

        private void listBoxMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            listBox.SelectedIndex = -1;
        }
        #endregion

        #region UNITS HANDLING
        private void RadioButtonLengthUnits_Click(object sender, RoutedEventArgs e)
        {
            CollectionView list = (CollectionView)CollectionViewSource.GetDefaultView(listBoxFlights.Items);
            RadioButton rb = sender as RadioButton;
            foreach (Flight item in list)
            {
                if (rb.Name == "Metric")
                    item.LengthUnit = LengthUnits.Metric;
                else item.LengthUnit = LengthUnits.Imperial;
            }
            btnRefreshStats_Click(this, null);
        }

        private void RadioButtonTemperatureUnits_Click(object sender, RoutedEventArgs e)
        {
            CollectionView list = (CollectionView)CollectionViewSource.GetDefaultView(listBoxFlights.Items);
            RadioButton rb = sender as RadioButton;
            foreach (Flight item in list)
            {
                if (rb.Name == "Celsius")
                    item.TemperatureUnit = TemperatureUnits.Celsious;
                else item.TemperatureUnit = TemperatureUnits.Fahrenheit;
            }
        }
        #endregion

        #region MAP HANDLING
        private void cbMapsProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            switch ((MapProviders)cb.SelectedItem)
            {
                case MapProviders.GoogleMap:
                    mapControl1.MapProvider = GMapProviders.GoogleMap;
                    break;
                case MapProviders.GoogleTerrainMap:
                    mapControl1.MapProvider = GMapProviders.GoogleTerrainMap;
                    break;
                case MapProviders.Bing:
                    mapControl1.MapProvider = GMapProviders.BingMap;
                    break;
                default:
                    break;
            }
        }

        private void btnApplyWEBProxy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBoxWEBAddress.Text != String.Empty && textBoxPort.Text != String.Empty)
                    GMapProvider.WebProxy = new WebProxy(textBoxWEBAddress.Text.Replace(" ", ""), int.Parse(textBoxPort.Text)); //("10.77.0.7", 8080);
                mapControl1.ReloadMap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClearWEBProxy_Click(object sender, RoutedEventArgs e)
        {
            GMapProvider.WebProxy = null;
            textBoxWEBAddress.Text = "";
            textBoxPort.Text = "";
        }

        public void DrawAllMapMarkers()
        {
            foreach (Flight fl in listBoxFlights.Items.Cast<Flight>())
            {
                PointLatLng point = new PointLatLng(fl.Logs.First().HomeLatitude, fl.Logs.First().HomeLongitude);
                GMapMarker marker = new GMapMarker(point);
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("/Resources/HomeIcon.png", UriKind.Relative));
                img.ToolTip = fl;
                marker.Shape = img;
                mapControl1.Markers.Add(marker);
            }
        }

        public void DrawPath()
        {
            Flight fl = (Flight)(listBoxFlights.SelectedItem);
            if (fl == null)
                return;

            PointLatLng point = new PointLatLng(fl.Logs[0].HomeLatitude, fl.Logs[0].HomeLongitude);

            IEnumerable<PointLatLng> pts = fl.Logs.Where(f => f.LogID % 1 == 0).Select(p => new PointLatLng() { Lat = p.Latitude, Lng = p.Longitude });
            GMapPolygon gmapPolygon = new GMapPolygon(pts);
            gmapPolygon.RegenerateShape(mapControl1);

            Shape shp = (Shape)gmapPolygon.Shape;
            shp.StrokeThickness = 2;
            shp.Stroke = Brushes.Red;
            shp.Fill = null;
            shp.Opacity = 1;

            GMapMarker marker = new GMapMarker(point);
            marker.Shape = gmapPolygon.Shape;
            marker.ZIndex = -1;

            //Remove Current Path from markers.
            int pos = 0;
            foreach (GMapMarker m in mapControl1.Markers)
            {
                if (m.Shape is System.Windows.Shapes.Path)
                {
                    mapControl1.Markers.RemoveAt(pos);
                    break;
                }
                pos++;
            }
            mapControl1.Markers.Add(marker);
        }

        public void DrawAllPaths()
        {
            foreach (Flight fl in listBoxFlights.Items.Cast<Flight>())
            {
                PointLatLng point = new PointLatLng(fl.Logs[0].HomeLatitude, fl.Logs[0].HomeLongitude);

                IEnumerable<PointLatLng> pts = fl.Logs.Where(f => f.LogID % 1 == 0).Select(p => new PointLatLng() { Lat = p.Latitude, Lng = p.Longitude });
                GMapPolygon gmapPolygon = new GMapPolygon(pts);
                gmapPolygon.RegenerateShape(mapControl1);

                Shape shp = (Shape)gmapPolygon.Shape;
                shp.StrokeThickness = 2;
                shp.Stroke = Brushes.Red;
                shp.Fill = null;
                shp.Opacity = 1;

                GMapMarker marker = new GMapMarker(point);
                marker.Shape = gmapPolygon.Shape;
                mapControl1.Markers.Add(marker);
            }
        }

        private void mapControl1_OnMapZoomChanged()
        {
            mapControl1.Markers.Clear();
            DrawAllMapMarkers();
            DrawPath();
        }

        private void btnRefreshMap_Click(object sender, RoutedEventArgs e)
        {
            mapControl1.ReloadMap();
        }

        private void btnDrawAllMarkers_Click(object sender, RoutedEventArgs e)
        {
            DrawAllMapMarkers();
        }

        private void btnDrawAllPaths_Click(object sender, RoutedEventArgs e)
        {
            DrawAllPaths();
        }

        private void btnClearMarkers_Click(object sender, RoutedEventArgs e)
        {
            mapControl1.Markers.Clear();
        }
        #endregion

        #region GRAPH HANDLING
        private void DrawGraph()
        {
            Flight fl = (Flight)(listBoxFlights.SelectedItem);
            if (fl == null)
                return;

            List<DataPoint> seriesData1 = fl.Logs.Select(l => new DataPoint() { X = l.LogID, Y = l.Altitude * fl.LengthConverter }).ToList();

            PlotModel myModel = new PlotModel() { Title = fl.Title };

            myModel.Series.Add(new LineSeries() { ItemsSource = seriesData1 });

            plotView1.Model = myModel;
        }

        private void btnAddSeries_Click(object sender, RoutedEventArgs e)
        {
            Flight fl = (Flight)(listBoxFlights.SelectedItem);
            if (fl == null)
                return;
            try
            {
                string[] x = cmbXItems.SelectedValue.ToString().Split(new char[] { ':' });
                PropertyInfo Xprop = typeof(Log).GetProperty(x.Last().Trim());

                string[] y = cmbYItems.SelectedValue.ToString().Split(new char[] { ':' });
                PropertyInfo Yprop = typeof(Log).GetProperty(y.Last().Trim());

                double XConvertUnit = 1;
                switch (Xprop.Name)
                {
                    case "Speed":
                    case "MaxSpeed":
                        XConvertUnit = fl.SpeedConverter;
                        break;
                    case "Altitude":
                    case "MaxAltitude":
                    case "MaxDistance":
                        XConvertUnit = fl.LengthConverter;
                        break;
                    default:
                        break;
                }

                double YConvertUnit = 1;
                switch (Yprop.Name)
                {
                    case "Speed":
                    case "MaxSpeed":
                        YConvertUnit = fl.SpeedConverter;
                        break;
                    case "Altitude":
                    case "MaxAltitude":
                    case "MaxDistance":
                        YConvertUnit = fl.LengthConverter;
                        break;
                    default:
                        break;
                }


                List<DataPoint> seriesData1 = fl.Logs.Select(l => new DataPoint() { X = Double.Parse(Xprop.GetValue(l).ToString()) * XConvertUnit, Y = Double.Parse(Yprop.GetValue(l).ToString()) * YConvertUnit }).ToList();

                listBoxSeries.Items.Add(new { Name = $"{fl.Title}: {Xprop.Name} vs {Yprop.Name}", Data = seriesData1 });

                PlotModel myModel = new PlotModel();

                if (listBoxSeries.Items != null)
                {
                    foreach (dynamic item in listBoxSeries.Items)
                    {
                        myModel.Series.Add(new LineSeries()
                        {
                            ItemsSource = item.Data,
                            Title = item.Name
                        });
                    }
                }
                plotView1.Model = myModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnRemoveSeries_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxSeries.Items != null)
            {
                listBoxSeries.Items.Remove(listBoxSeries.SelectedItem);

                PlotModel myModel = new PlotModel();

                if (listBoxSeries.Items != null)
                {
                    foreach (dynamic item in listBoxSeries.Items)
                    {
                        myModel.Series.Add(new LineSeries()
                        {
                            ItemsSource = item.Data,
                            Title = item.Name
                        });
                    }
                }
                plotView1.Model = myModel;
            }
        }

        private void btnClearSeries_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxSeries.Items != null)
            {
                listBoxSeries.Items.Clear();
                plotView1.Model = new PlotModel();
            }
        }

        private void btnDrawSeries_Click(object sender, RoutedEventArgs e)
        {
            PlotModel myModel = new PlotModel();

            if (listBoxSeries.Items != null)
            {
                foreach (dynamic item in listBoxSeries.Items)
                {
                    myModel.Series.Add(new LineSeries()
                    {
                        ItemsSource = item.Data,
                        Title = item.Name
                    });
                }
            }
            plotView1.Model = myModel;
        }

        #endregion

        #region VIDEO HANDLING
        private void btnAddVideoFiles_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choose Media";
            if (dialog.ShowDialog() == true)
            {
                mediaPlayer1.Source = new Uri(dialog.FileName);

                if (listBoxFlights.SelectedItem != null)
                {
                    ((Flight)(listBoxFlights.SelectedItem)).VideoFiles.Add(mediaPlayer1.Source.ToString());
                    listBoxVideoFiles.Items.Refresh();
                }
            }
        }

        private void listBoxVideoFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxVideoFiles.SelectedItem != null)
            {
                mediaPlayer1.Source = new Uri(listBoxVideoFiles.SelectedItem.ToString());
                mediaPlayer1.Play();
            }
        }

        private void btnRemoveVideoFiles_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxVideoFiles.HasItems)
            {
                mediaPlayer1.Stop();
                if (listBoxFlights.SelectedItem != null)
                {
                    ((Flight)(listBoxFlights.SelectedItem)).VideoFiles.Remove(listBoxVideoFiles.SelectedItem.ToString());
                    listBoxVideoFiles.Items.Refresh();
                }
            }
        }

        private void btnClearVideoFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnScreenShot_Click(object sender, RoutedEventArgs e)
        {
            Size dpi = new Size(300, 300);
            RenderTargetBitmap bmp = new RenderTargetBitmap(640, 480, dpi.Width, dpi.Height, PixelFormats.Pbgra32);
            bmp.Render(mediaPlayer1);

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            string filename = Guid.NewGuid().ToString() + ".jpg";
            FileStream fs = new FileStream(filename, FileMode.Create);
            encoder.Save(fs);
            fs.Close();

            System.Diagnostics.Process.Start(filename);
        }

        #region VIDEO SLIDER
        private bool userIsDraggingSlider = false;

        private void mediaPlayer1_MediaOpened(object sender, RoutedEventArgs e)
        {
            //Initialize timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer1.Source != null) && (mediaPlayer1.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                videoSlider.Minimum = 0;
                videoSlider.Maximum = mediaPlayer1.NaturalDuration.TimeSpan.TotalSeconds;
                videoSlider.Value = mediaPlayer1.Position.TotalSeconds;
            }
        }

        private void videoSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void videoSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaPlayer1.Position = TimeSpan.FromSeconds(videoSlider.Value);
        }
        #endregion

        #endregion

        #region VARIOUS
        private void btnRefreshStats_Click(object sender, RoutedEventArgs e)
        {
            flightsStatistics.FlightsStats.Clear();
            foreach (Flight item in listBoxFlights.Items)
            {
                if (item.IsChecked)
                    flightsStatistics.FlightsStats.Add(new StatisticsData
                    {
                        MaxDistance = item.MaxDistance,
                        PathLength = item.PathLength,
                        MaxSpeed = item.MaxSpeed,
                        FlightDate = item.FlightDate,
                        Duration = item.FlightDuration,
                        DistancePerConsumptionUnit = item.DistancePerConsumptionUnit,
                        LengthUnit = item.LengthUnit,
                        MaxAltitude = item.MaxAltitude,
                        Title = item.Title
                    });
            }
            flightsStatistics.InitializeLINQProperties();
            flightsStatistics.NotifyChange("");
            textBlockSelectedItems.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }

        private void TabControl_GotFocus(object sender, RoutedEventArgs e)
        {
           
            TabItem ti = e.Source as TabItem;
            if (ti == null)
                return;

            switch (((TabItem)e.Source).Header.ToString())
            {
                case "Graphs":
                    mapControl1.Visibility = Visibility.Hidden;
                    plotView1.Visibility = Visibility.Visible;
                    grdVideo.Visibility = Visibility.Hidden;
                    break;
                case "Video":
                    mapControl1.Visibility = Visibility.Hidden;
                    plotView1.Visibility = Visibility.Hidden;
                    grdVideo.Visibility = Visibility.Visible;
                    break;
                default:
                    mapControl1.Visibility = Visibility.Visible;
                    plotView1.Visibility = Visibility.Hidden;
                    grdVideo.Visibility = Visibility.Hidden;

                    break;
            }
        }

        #endregion

        ///WIP///
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            //List<Log> myList = ((Flight)(listBoxFlights.SelectedItem)).Logs;
            //string csv = ((Flight)(listBoxFlights.SelectedItem)).Logs.ToDelimitedText<Log>(",");
            //File.WriteAllText(@"D:\path.txt", csv);

            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(listBoxFlights.Items);
            List<Flight> list = new List<Flight>();
            foreach (Flight item in cv)
            {
                list.Add(item);
            }
            int n = list.Select(f => f.Logs.Count).Sum();
            MessageBox.Show(n.ToString());
        }
    }
}