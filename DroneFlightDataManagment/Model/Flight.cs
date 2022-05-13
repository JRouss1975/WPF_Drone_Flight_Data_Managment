using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace DroneFlightDataManagment
{
    [Serializable]
    public class Flight : Observable
    {
        public Flight()
        {
        }

        public LengthUnits LengthUnit
        {
            get { return _LengthUnit; }
            set
            {
                _LengthUnit = value;
                NotifyChange("");
            }
        }
        public LengthUnits _LengthUnit;

        public TemperatureUnits TemperatureUnit
        {
            get { return _TemperatureUnit; }
            set
            {
                _TemperatureUnit = value;
                NotifyChange("");
            }
        }
        public TemperatureUnits _TemperatureUnit;

        public float LengthConverter
        {
            get
            {
                return (this.LengthUnit == LengthUnits.Metric) ? 0.3048F : 1;
            }
        }

        public float SpeedConverter
        {
            get
            {
                return (this.LengthUnit == LengthUnits.Metric) ? 1.609344F : 1;
            }
        }

        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
                NotifyChange();
            }
        }
        private bool _IsChecked = true;

        public DateTime FlightDate
        {
            get
            {
                return Logs[0].LocalDateTime;
            }
        }

        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                NotifyChange();
            }
        }
        private string _Title;

        public string Description { get; set; }

        public List<Log> Logs { get; set; } = new List<Log>();

        public float MaxAltitude
        {
            get
            {
                int last = Logs.Count - 1;
                if (LengthUnit == LengthUnits.Imperial)
                    return Logs[last].MaxAltitude;
                return Logs[last].MaxAltitude * 0.3048F;
            }
        }

        public float MaxSpeed
        {
            get
            {
                int last = Logs.Count - 1;
                if (LengthUnit == LengthUnits.Imperial)
                    return Logs[last].MaxSpeed;
                return Logs[last].MaxSpeed * 1.609344F;
            }
        }

        public float MaxDistance
        {
            get
            {
                int last = Logs.Count - 1;
                if (LengthUnit == LengthUnits.Imperial)
                    return Logs[last].MaxDistance;
                return Logs[last].MaxDistance * 0.3048F;
            }
        }

        public TimeSpan FlightDuration
        {
            get
            {
                int min = Logs.Where(l => l.IsFlying == true).Select(l => l.Time).Min();
                int max = Logs.Where(l => l.IsFlying == true).Select(l => l.Time).Max();
                int dt = max - min;
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, dt);
                return ts;
            }
        }

        public double PathLength
        {
            get
            {
                int logsLength = Logs.Count();
                double totalDistance = 0;
                for (int i = 0, j = 1; i < logsLength - 1; i++, j++)
                {
                    if (Logs[i].IsFlying == true && Logs[j].IsFlying == true)
                    {
                        GeoCoordinate s = new GeoCoordinate(Logs[i].Latitude, Logs[i].Longitude, Logs[i].Altitude * 0.3048) { HorizontalAccuracy = 1, VerticalAccuracy = 1 };
                        GeoCoordinate e = new GeoCoordinate(Logs[j].Latitude, Logs[j].Longitude, Logs[j].Altitude * 0.3048) { HorizontalAccuracy = 1, VerticalAccuracy = 1 };
                        double dx = Math.Abs(s.GetDistanceTo(e));
                        double dy = Math.Abs(e.Altitude - s.Altitude);
                        double d = Math.Sqrt(dx * dx + dy * dy);
                        totalDistance += d;
                    }
                }
                if (LengthUnit == LengthUnits.Imperial)
                    return totalDistance / 0.3048F;
                return totalDistance;
            }
        }

        public double PathLengthAt(int index)
        {
            int logsLength = Logs.Count();
            double totalDistance = 0;
            for (int i = 0, j = 1; i < index - 1; i++, j++)
            {
                if (Logs[i].IsFlying == true && Logs[j].IsFlying == true)
                {
                    GeoCoordinate s = new GeoCoordinate(Logs[i].Latitude, Logs[i].Longitude, Logs[i].Altitude * 0.3048) { HorizontalAccuracy = 1, VerticalAccuracy = 1 };
                    GeoCoordinate e = new GeoCoordinate(Logs[j].Latitude, Logs[j].Longitude, Logs[j].Altitude * 0.3048) { HorizontalAccuracy = 1, VerticalAccuracy = 1 };
                    double dx = Math.Abs(s.GetDistanceTo(e));
                    double dy = Math.Abs(e.Altitude - s.Altitude);
                    double d = Math.Sqrt(dx * dx + dy * dy);
                    totalDistance += d;
                }
            }
            if (LengthUnit == LengthUnits.Imperial)
                return totalDistance / 0.3048F;
            return totalDistance;
        }


        public double AverageSpeed
        {
            get
            {
                if (LengthUnit == LengthUnits.Imperial)
                    return (PathLength / FlightDuration.TotalSeconds) * 0.681818;
                return (PathLength / FlightDuration.TotalSeconds) * 3.600;
            }
        }

        public byte TakeOffBattety
        {
            get
            {
                return Logs.First(f => f.IsFlying).RemainPowerPercent;
            }
        }

        public byte LandingBattety
        {
            get
            {
                return Logs.Last(f => f.IsFlying).RemainPowerPercent;
            }
        }

        public byte BatteryConsumption
        {
            get
            {
                return (byte)(TakeOffBattety - LandingBattety);
            }
        }

        public float AverageAltitude
        {
            get
            {
                if (LengthUnit == LengthUnits.Imperial)
                    return (float)Logs.Where(f => f.IsFlying).Average(f => f.Altitude);
                return (float)Logs.Where(f => f.IsFlying).Average(f => f.Altitude) * 0.3048F;
            }
        }

        public float DistancePerConsumptionUnit
        {
            get
            {
                return (float)PathLength / BatteryConsumption;
            }
        }

        public IEnumerable<Log> GetAppTips
        {
            get
            {
                return Logs.Where(f => f.AppTip != "").Select(f => f);
            }
        }

        public IEnumerable<Log> GetAppWarning
        {
            get
            {
                return Logs.Where(f => f.AppWarning != "").Select(f => f);
            }
        }

        public int NumberOfTips
        {
            get
            {
                return Logs.Count(f => f.AppTip != "");
            }
        }

        public int NumberOfWarnings
        {
            get
            {
                return Logs.Count(f => f.AppWarning != "");
            }
        }

        public List<Log> LogsWhereVideoWasTaken { get; set; }

        public int NumberOfVideos
        {
            get
            {
                int numberOfVideos = 0;
                bool isVideoButtonPressed = false;
                LogsWhereVideoWasTaken = new List<Log>();
                foreach (Log l in Logs)
                {
                    if (l.IsTakingVideo && isVideoButtonPressed == false)
                    {
                        isVideoButtonPressed = true;
                        numberOfVideos++;
                        LogsWhereVideoWasTaken.Add(l);
                    }
                    if (!l.IsTakingVideo && isVideoButtonPressed == true)
                        isVideoButtonPressed = false;
                }
                return numberOfVideos;
            }
        }

        public List<Log> LogsWherePhotoWasTaken { get; set; }

        public int NumberOfPhotos
        {
            get
            {
                int numberOfPhotos = 0;
                bool isPhotoButtonPressed = false;
                LogsWherePhotoWasTaken = new List<Log>();
                foreach (Log l in Logs)
                {
                    if (l.IsTakingPhoto && isPhotoButtonPressed == false)
                    {
                        isPhotoButtonPressed = true;
                        numberOfPhotos++;
                        LogsWherePhotoWasTaken.Add(l);
                    }
                    if (!l.IsTakingPhoto && isPhotoButtonPressed == true)
                        isPhotoButtonPressed = false;
                }
                return numberOfPhotos;
            }
        }

        public List<string> VideoFiles
        {
            get { return _videoFiles; }
            set
            {
                _videoFiles = value;
                NotifyChange();
            }
        }
        private List<string> _videoFiles = new List<string>();

        public override string ToString()
        {
            return $"{Logs[0].LocalDateTime.ToShortDateString()} - {Logs[0].LocalDateTime.ToShortTimeString()} : {Title}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Flight && obj != null)
                return this.FlightDate.Equals((obj as Flight).FlightDate);
            return false;
        }
    }
}