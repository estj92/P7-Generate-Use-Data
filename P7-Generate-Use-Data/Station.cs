using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Station:ISequelize
    {
        public Station(Coordinate location)
        {
            Location = location;
        }
        public Coordinate Location { get; set; }


        public override string ToString()
        {
            return "Station: " + Location.ToString();
        }

        #region ISequelize Members

        public string ToSequelize()
        {
            StringBuilder sb = new StringBuilder("{ ");



            sb.Append(" }");
            return sb.ToString();
        }

        #endregion
    }
}
