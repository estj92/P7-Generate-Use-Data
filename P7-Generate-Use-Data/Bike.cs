using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Bike
    {
        public Bike(int id, Coordinate location, bool atStation)
        {
            ID = id;
            Location = location;
            AtStation = atStation;
        }

        public int ID { get; set; }
        public Coordinate Location { get; set; }
        public bool AtStation { get; set; }

        public override string ToString()
        {
            return "Bike: " + ID.ToString().PadLeft(3) + " Station: " + AtStation.ToString().PadLeft(5) + " " + Location;
        }
    }
}
