using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Convert
    {
        public static string ConvertLattitude(double coord)   /// Funcs to Convert lattitudes and longtitudes from decimal to Degrees
        {
            char direction;
            double sec = (double)Math.Round(coord * 3600);
            double deg = Math.Abs(sec / 3600);
            sec = Math.Abs(sec % 3600);
            double min = sec / 60;
            sec %= 60;
            if (coord >= 0)
                direction = 'N';
            else
                direction = 'S';
            return $"{(int)deg}°{(int)min}'{sec}''{ direction}";
        }
        public static string ConvertLongitude(double coord)
        {
            char direction;
            double sec = (double)Math.Round(coord * 3600);
            double deg = Math.Abs(sec / 3600);
            sec = Math.Abs(sec % 3600);
            double min = sec / 60;
            sec %= 60;
            if (coord >= 0)
                direction = 'E';
            else
                direction = 'W';
            return $"{(int)deg}°{(int)min}'{sec}''{ direction}";
        }
    }

}


  
    

       
  
   
