using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Coordinate
    {
        public Coordinate(double longtitude, double lattitude)
        {
            Longtitude = longtitude;
            Lattitude = lattitude;
        }

        public double Longtitude { get; set; }
        public double Lattitude { get; set; }
    }
}
