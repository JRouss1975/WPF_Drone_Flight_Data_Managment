using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFlightDataManagment
{
    [Serializable]
    public class Log : Observable
    {
        public int LogID { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Altitude { get; set; }
        public int Ascent { get; set; }
        public float Speed { get; set; }
        public int Distance { get; set; }
        public int MaxAltitude { get; set; }
        public int MaxAscent { get; set; }
        public float MaxSpeed { get; set; }
        public int MaxDistance { get; set; }
        public int Time { get; set; }
        public DateTime UTCDateTime { get; set; }
        public DateTime LocalDateTime { get; set; }
        public int Satellites { get; set; }
        //public float Pressure { get; set; }
        //public float Temperature { get; set; }
        //public float Voltage { get; set; }
        public float HomeLatitude { get; set; }
        public float HomeLongitude { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float VelocityZ { get; set; }
        public int Pitch { get; set; }
        public int Roll { get; set; }
        public int Yaw { get; set; }
        //public int PowerLevel { get; set; }
        public bool IsFlying { get; set; }
        public bool IsTakingPhoto { get; set; }
        public byte RemainPowerPercent { get; set; }
        public byte RemainLifePercent { get; set; }
        public int CurrentCurrent { get; set; }
        public int CurrentElectricity { get; set; }
        public int CurrentVoltage { get; set; }
        public float BatteryTemperature { get; set; }
        public byte DischargeCount { get; set; }
        public string Flightmode { get; set; }
        public bool IsMotorsOn { get; set; }
        public bool IsTakingVideo { get; set; }
        public int RcElevator { get; set; }
        public int RcAileron { get; set; }
        public int RcThrottle { get; set; }
        public int RcRudder { get; set; }
        public int RcGyro { get; set; }
        public long TimeStamp { get; set; }
        public int BatteryCell1 { get; set; }
        public int BatteryCell2 { get; set; }
        public int BatteryCell3 { get; set; }
        public int BatteryCell4 { get; set; }
        public int BatteryCell5 { get; set; }
        public int BatteryCell6 { get; set; }
        public int DroneType { get; set; }
        public string AppVersion { get; set; }
        public string PlaneName { get; set; }
        public string FlyControllerSerialNumber { get; set; }
        public string RemoteSerialNumber { get; set; }
        //public string BatterySerialNumber { get; set; }
        public string CenterBatteryProductDate { get; set; }
        public string CenterBatterySerialNo { get; set; }
        public int CenterBatteryFullCapacity { get; set; }
        public int CenterBatteryProductDateRaw { get; set; }
        public int PitchRaw { get; set; }
        public int RollRaw { get; set; }
        public int YawRaw { get; set; }
        public int GimbalPitchRaw { get; set; }
        public int GimbalRollRaw { get; set; }
        public int GimbalYawRaw { get; set; }
        public int FlyState { get; set; }
        public int AltitudeRaw { get; set; }
        public float SpeedRaw { get; set; }
        public float DistanceRaw { get; set; }
        public int VelocityXRaw { get; set; }
        public int VelocityYRaw { get; set; }
        public int VelocityZRaw { get; set; }
        //public byte DataReuse { get; set; }
        public string AppTip { get; set; }
        public string AppWarning { get; set; }
        public int DownlinkSignalQuality { get; set; }
        public int UplinkSignalQuality { get; set; }
        public int TransmissionChannel { get; set; }

        public override string ToString()
        {
            return $"{LogID} : {Latitude} , {Longitude} , {Altitude}, {LocalDateTime.ToShortTimeString()}, {new TimeSpan(0, 0, 0, 0, Time)}";
        }
    }
}
