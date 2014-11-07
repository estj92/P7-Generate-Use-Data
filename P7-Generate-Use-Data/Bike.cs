using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Bike: IEquatable<Bike>
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
            return (ReferenceEquals(b1, null) ?
                false :
                b1.Equals(b2));
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
    }
}
