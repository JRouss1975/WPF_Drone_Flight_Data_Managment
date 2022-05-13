using System;
using System.Globalization;
using System.Windows.Data;

namespace DroneFlightDataManagment
{
    public class UnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string checkValue = value.ToString();
            string paramValue = parameter.ToString();

            if (checkValue.Equals("Metric"))
            {
                if (paramValue.Equals("Speed"))
                    return "[kmh]";
                return "[m]";
            }
            else
            {
                if (paramValue.Equals("Speed"))
                    return "[mph]";
                return "[ft]";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
