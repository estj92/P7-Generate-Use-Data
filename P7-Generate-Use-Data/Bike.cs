using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Bike: IEquatable<Bike>, ISequelize
    {
        public Bike(int id, int battery, Coordinate location, bool occupied, Station station)
        {
            ID = id;
            Location = location;
            Station = station;
            Occupied = occupied;
            Battery = battery;
        }

        public int ID { get; set; }
        public int Battery { get; set; }
        public Coordinate Location { get; set; }
        public Station Station { get; set; }
        public bool Occupied { get; set; }

        public override string ToString()
        {
            return "Bike: " + ID.ToString().PadLeft(3) + " Station: " + Station.ToString().PadLeft(5) + " " + Location;
        }

        #region IEquatable<Bike> Members

        public bool Equals(Bike other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            Bike b = obj as Bike;

            return (ReferenceEquals(b, null) ?
                false :
                this.Equals(b));
        }

        public static bool operator ==(Bike b1, Bike b2)
        {
            if (ReferenceEquals(b1, b2))
            {
                return true;
            }
            if (ReferenceEquals(b1, null) || ReferenceEquals(b2, null))
            {
                return false;
            }

            return b1.Equals(b2);
        }

        public static bool operator !=(Bike b1, Bike b2)
        {
            return !(b1 == b2);
        }

        public override int GetHashCode()
        {
            return ID;
        }

        #endregion

        #region ISequelize Members

        public string ToSequelize()
        {
            StringBuilder sb = new StringBuilder("{ ");

            sb.Append("location: \"");
            sb.Append(Location.Longtitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "," + Location.Lattitude.ToString(System.Globalization.CultureInfo.InvariantCulture));
            sb.Append("\", ");

            sb.Append("batteryLife: ");
            sb.Append(Battery);
            sb.Append(", ");

            sb.Append("status: \'");
            if (Occupied)
            {
                sb.Append("un");
            }
            sb.Append("occupied\', ");

            sb.Append("StationId: ");
            sb.Append(Station == null ? "null" : "1");

            sb.Append(" }");
            return sb.ToString();
        }

        #endregion
    }
}
