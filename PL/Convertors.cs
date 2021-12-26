using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using BlApi;

namespace PL
{
    internal class BatteryToProgressBarConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Double)value * 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class BatteryToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double battery = (double)value;
            if (battery < 0.1)
                return Brushes.DarkRed;
            if (battery < 0.2)
                return Brushes.Red;
            if (battery < 0.4)
                return Brushes.Yellow;
            if (battery < 0.6)
                return Brushes.GreenYellow;
            
            return Brushes.DarkGreen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}