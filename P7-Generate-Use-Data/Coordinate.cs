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
                return Lattitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", " + Longtitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}
