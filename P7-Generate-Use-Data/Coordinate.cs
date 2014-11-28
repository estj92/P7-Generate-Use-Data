using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Coordinate
    {
        public Coordinate(double lattitude, double longtitude)
        {
            Longtitude = longtitude;
            Lattitude = lattitude;
        }

        public double Longtitude { get; set; }
        public double Lattitude { get; set; }

        public override string ToString()
        {
            return "Long: " + Longtitude + " - Lat: " + Lattitude;
        }

        public string LocationToSequelize
        {
            get
            {
                return
                  LattitudeToSequelize + ", " + LongtitudeToSequelize;
            }
        }
        public string LattitudeToSequelize
        {
            get
            {
                return "lattitude: " + Lattitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        public string LongtitudeToSequelize
        {
            get
            {
                return "longtitude: " + Longtitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}
