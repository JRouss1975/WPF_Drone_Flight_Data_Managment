using System;
using System.ComponentModel;

namespace DroneFlightDataManagment
{
    [Serializable]
    public enum LengthUnits
    {
        Metric,
        Imperial,
    }

    [Serializable]
    public enum TemperatureUnits
    {
        Celsious,
        Fahrenheit
    }

    [Serializable]
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MapProviders
    {
        [Description("Google Terrain Map")]
        GoogleTerrainMap,
        [Description("Google Map")]
        GoogleMap,
        Bing
    }
}
