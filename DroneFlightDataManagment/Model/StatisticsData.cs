using System;

namespace DroneFlightDataManagment
{
    public class StatisticsData
    {
        public LengthUnits LengthUnit { get; set; }
        public string Title { get; set; }
        public DateTime FlightDate { get; set; }
        public float MaxDistance { get; set; }
        public double PathLength { get; set; }
        public TimeSpan Duration { get; set; }   
        public float MaxSpeed { get; set; }   
        public float MaxAltitude { get; set; }
        public float DistancePerConsumptionUnit { get; set; }

        public StatisticsData()
        {

        }
    }
}
