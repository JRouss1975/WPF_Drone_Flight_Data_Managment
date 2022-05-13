using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace DroneFlightDataManagment
{
    public class Statistics : Observable
    {
        public ObservableCollection<StatisticsData> FlightsStats { get; set; } = new ObservableCollection<StatisticsData>();

        public int Counter { get; set; }

        #region LENGTH
        public double SPathLength { get; set; }
        public double SAveragePathLength
        {
            get
            {
                return Counter > 0 ? SPathLength / Counter : 0;
            }
        }
        //-----------------------------------------
        public double SMaxPathLength
        {
            get
            {
                double sMaxPathLength = Counter > 0 ? FlightsStats.Max(f => f.PathLength) : 0;
                var q = FlightsStats.Where(f => f.PathLength == sMaxPathLength).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMaxPathLengthFlightTitle = q[0].Title;
                    SMaxPathLengthFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMaxPathLength : 0;
            }
        }
        public string SMaxPathLengthFlightTitle
        {
            get
            {
                return _SMaxPathLengthFlightTitle;
            }
            set
            {
                _SMaxPathLengthFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMaxPathLengthFlightTitle;
        public string SMaxPathLengthFlightDate
        {
            get
            {
                return _SMaxPathLengthFlightDate;
            }
            set
            {
                _SMaxPathLengthFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMaxPathLengthFlightDate;
        //-----------------------------------------
        public double SMinPathLength
        {
            get
            {
                double sMinPathLength = Counter > 0 ? FlightsStats.Min(f => f.PathLength) : 0;
                var q = FlightsStats.Where(f => f.PathLength == sMinPathLength).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMinPathLengthFlightTitle = q[0].Title;
                    SMinPathLengthFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMinPathLength : 0;
            }
        }
        public string SMinPathLengthFlightTitle
        {
            get
            {
                return _SMinPathLengthFlightTitle;
            }
            set
            {
                _SMinPathLengthFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMinPathLengthFlightTitle;
        public string SMinPathLengthFlightDate
        {
            get
            {
                return _SMinPathLengthFlightDate;
            }
            set
            {
                _SMinPathLengthFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMinPathLengthFlightDate;
        #endregion

        #region DURATION
        public TimeSpan SDuration { get; set; }
        public TimeSpan SAverageDuration
        {
            get
            {
                return Counter > 0 ? new TimeSpan(0, 0, 0, (int)(SDuration.TotalSeconds / Counter)) : new TimeSpan();
            }
        }
        //-----------------------------------------
        public TimeSpan SMaxDuration
        {
            get
            {
                TimeSpan sMaxDuration = Counter > 0 ? FlightsStats.Max(f => f.Duration) : new TimeSpan();
                var q = FlightsStats.Where(f => f.Duration == sMaxDuration).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMaxDurationFlightTitle = q[0].Title;
                    SMaxDurationFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMaxDuration : new TimeSpan();
            }
        }
        public string SMaxDurationFlightTitle
        {
            get
            {
                return _SMaxDurationFlightTitle;
            }
            set
            {
                _SMaxDurationFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMaxDurationFlightTitle;
        public string SMaxDurationFlightDate
        {
            get
            {
                return _SMaxDurationFlightDate;
            }
            set
            {
                _SMaxDurationFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMaxDurationFlightDate;
        //-----------------------------------------
        public TimeSpan SMinDuration
        {
            get
            {
                TimeSpan sMinDuration = Counter > 0 ? FlightsStats.Min(f => f.Duration) : new TimeSpan();
                var q = FlightsStats.Where(f => f.Duration == sMinDuration).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMinDurationFlightTitle = q[0].Title;
                    SMinDurationFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMinDuration : new TimeSpan();
            }
        }
        public string SMinDurationFlightTitle
        {
            get
            {
                return _SMinDurationFlightTitle;
            }
            set
            {
                _SMinDurationFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMinDurationFlightTitle;
        public string SMinDurationFlightDate
        {
            get
            {
                return _SMinDurationFlightDate;
            }
            set
            {
                _SMinDurationFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMinDurationFlightDate;
        #endregion

        #region SPEED
        public double SAverageSpeed
        {
            get
            {
                if (Counter > 0 && SDuration.Seconds > 0)
                {
                    if (FlightsStats.First().LengthUnit == LengthUnits.Imperial)
                        return SPathLength / SDuration.TotalSeconds * 0.681818;
                    return SPathLength / SDuration.TotalSeconds * 3.6000;
                }
                return 0;
            }
        }
        //-----------------------------------------
        public double SMaxSpeed
        {
            get
            {
                double sMaxSpeed = Counter > 0 ? FlightsStats.Max(f => f.MaxSpeed) : 0;
                var q = FlightsStats.Where(f => f.MaxSpeed == sMaxSpeed).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMaxSpeedFlightTitle = q[0].Title;
                    SMaxSpeedFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMaxSpeed : 0;
            }
        }
        public string SMaxSpeedFlightTitle
        {
            get
            {
                return _SMaxSpeedFlightTitle;
            }
            set
            {
                _SMaxSpeedFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMaxSpeedFlightTitle;
        public string SMaxSpeedFlightDate
        {
            get
            {
                return _SMaxSpeedFlightDate;
            }
            set
            {
                _SMaxSpeedFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMaxSpeedFlightDate;
        //-----------------------------------------
        public double SMinSpeed
        {
            get
            {
                double sMinSpeed = Counter > 0 ? FlightsStats.Min(f => f.MaxSpeed) : 0;
                var q = FlightsStats.Where(f => f.MaxSpeed == sMinSpeed).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMinSpeedFlightTitle = q[0].Title;
                    SMinSpeedFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMinSpeed : 0;
            }
        }
        public string SMinSpeedFlightTitle
        {
            get
            {
                return _SMinSpeedFlightTitle;
            }
            set
            {
                _SMinSpeedFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMinSpeedFlightTitle;
        public string SMinSpeedFlightDate
        {
            get
            {
                return _SMinSpeedFlightDate;
            }
            set
            {
                _SMinSpeedFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMinSpeedFlightDate;
        #endregion

        #region DISTANCE
        public double SMaxDistance
        {
            get
            {
                double sMaxDistance = Counter > 0 ? FlightsStats.Max(f => f.MaxDistance) : 0;
                var q = FlightsStats.Where(f => f.MaxDistance == sMaxDistance).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMaxDistanceFlightTitle = q[0].Title;
                    SMaxDistanceFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMaxDistance : 0;
            }
        }
        public string SMaxDistanceFlightTitle
        {
            get
            {
                return _SMaxDistanceFlightTitle;
            }
            set
            {
                _SMaxDistanceFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMaxDistanceFlightTitle;
        public string SMaxDistanceFlightDate
        {
            get
            {
                return _SMaxDistanceFlightDate;
            }
            set
            {
                _SMaxDistanceFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMaxDistanceFlightDate;
        //-----------------------------------------
        public double SMinDistance
        {
            get
            {
                double sMinDistance = Counter > 0 ? FlightsStats.Min(f => f.MaxDistance) : 0;
                var q = FlightsStats.Where(f => f.MaxDistance == sMinDistance).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMinDistanceFlightTitle = q[0].Title;
                    SMinDistanceFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMinDistance : 0;
            }
        }
        public string SMinDistanceFlightTitle
        {
            get
            {
                return _SMinDistanceFlightTitle;
            }
            set
            {
                _SMinDistanceFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMinDistanceFlightTitle;
        public string SMinDistanceFlightDate
        {
            get
            {
                return _SMinDistanceFlightDate;
            }
            set
            {
                _SMinDistanceFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMinDistanceFlightDate;
        #endregion

        #region ALTITUDE
        public double SMaxAltitude
        {
            get
            {
                double sMaxAltitude = Counter > 0 ? FlightsStats.Max(f => f.MaxAltitude) : 0;
                var q = FlightsStats.Where(f => f.MaxAltitude == sMaxAltitude).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMaxAltitudeFlightTitle = q[0].Title;
                    SMaxAltitudeFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMaxAltitude : 0;
            }
        }
        public string SMaxAltitudeFlightTitle
        {
            get
            {
                return _SMaxAltitudeFlightTitle;
            }
            set
            {
                _SMaxAltitudeFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMaxAltitudeFlightTitle;
        public string SMaxAltitudeFlightDate
        {
            get
            {
                return _SMaxAltitudeFlightDate;
            }
            set
            {
                _SMaxAltitudeFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMaxAltitudeFlightDate;
        //-----------------------------------------
        public double SMinAltitude
        {
            get
            {
                double sMinAltitude = Counter > 0 ? FlightsStats.Min(f => f.MaxAltitude) : 0;
                var q = FlightsStats.Where(f => f.MaxAltitude == sMinAltitude).Select(s => s).ToList();
                if (q.Count > 0)
                {
                    SMinAltitudeFlightTitle = q[0].Title;
                    SMinAltitudeFlightDate = q[0].FlightDate.Date.ToShortDateString();
                }
                return Counter > 0 ? sMinAltitude : 0;
            }
        }
        public string SMinAltitudeFlightTitle
        {
            get
            {
                return _SMinAltitudeFlightTitle;
            }
            set
            {
                _SMinAltitudeFlightTitle = value;
                NotifyChange();
            }
        }
        private string _SMinAltitudeFlightTitle;
        public string SMinAltitudeFlightDate
        {
            get
            {
                return _SMinAltitudeFlightDate;
            }
            set
            {
                _SMinAltitudeFlightDate = value;
                NotifyChange();
            }
        }
        private string _SMinAltitudeFlightDate;
        #endregion

        #region CONSUMPTION
        public double SMeanConsumption
        {
            get
            {
                if (Counter > 0 && FlightsStats.Select(f => f.PathLength).Sum() > 0)
                {
                    return FlightsStats.Select(f => f.DistancePerConsumptionUnit * f.PathLength).Sum()
                    / FlightsStats.Select(f => f.PathLength).Sum();
                }
                return 0;
            }
        }
        #endregion

        public Statistics()
        {

        }

        public void InitializeLINQProperties()
        {
            Counter = FlightsStats != null ? FlightsStats.Count() : 0;
            SPathLength = FlightsStats.Select(f => f.PathLength).Sum();
            SDuration = new TimeSpan(0, 0, 0, (int)(FlightsStats.Select(f => f.Duration.TotalSeconds).Sum()));

            SMaxPathLengthFlightTitle = "";
            SMaxPathLengthFlightDate = "";
            SMinPathLengthFlightTitle = "";
            SMinPathLengthFlightDate = "";

            SMaxDurationFlightTitle = "";
            SMaxDurationFlightDate = "";
            SMinDurationFlightTitle = "";
            SMinDurationFlightDate = "";

            SMaxSpeedFlightTitle = "";
            SMaxSpeedFlightDate = "";
            SMinSpeedFlightTitle = "";
            SMinSpeedFlightDate = "";

            SMaxDistanceFlightTitle = "";
            SMaxDistanceFlightDate = "";
            SMinDistanceFlightTitle = "";
            SMinDistanceFlightDate = "";

            SMaxAltitudeFlightTitle = "";
            SMaxAltitudeFlightDate = "";
            SMinAltitudeFlightTitle = "";
            SMinAltitudeFlightDate = "";
        }
    }
}