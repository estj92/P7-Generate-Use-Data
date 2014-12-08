using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Station:ISequelize
    {
        public Station(string name, Coordinate location, int spaces, int bikes)
        {
            ID = ID_Gen;
            Name = name;
            Location = location;
            Spaces = spaces;
            Bikes = bikes;
        }

        private static int ID_Gen { get { return id_gen++; } }
        private static int id_gen = 0;

        public int ID { get; set; }
        public string Name { get; set; }
        public Coordinate Location { get; set; }
        public int Spaces { get; set; }
        public int Bikes { get; set; }


        public override string ToString()
        {
            return "Station: " + Location.ToString();
        }

        #region ISequelize Members

        public string ToSequelize()
        {
            StringBuilder sb = new StringBuilder("{ ");

            sb.Append("name: \"")
                .Append(Name)
                .Append("\", ");

            sb.Append(Location.LocationToSequelize)
                .Append(", ");

            sb.Append("spaces: ")
                .Append(Spaces)
                .Append(", ");

            sb.Append("bikes: ")
                .Append(Bikes);

            sb.Append(" }");
            return sb.ToString();
        }

        #endregion

    }
}
