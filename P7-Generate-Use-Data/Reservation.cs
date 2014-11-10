using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Reservation: ISequelize
    {
        public Reservation(User user, Station station, Bike bike, DateTime when)
        {
            User = user;
            Station = station;
            Bike = bike;
            When = when;
        }

        public User User { get; set; }
        public Station Station { get; set; }
        public Bike Bike { get; set; }
        public DateTime When { get; set; }


        public override string ToString()
        {
            return User + " " + Station + " " + Bike + " " + When;
        }

        #region ISequelize Members

        public string ToSequelize()
        {
            StringBuilder sb = new StringBuilder("{ ");

            sb.Append("startTime: new Date");
            sb.Append(Extensions.DateTimeToSequelize(When));
            sb.Append(", ");

            sb.Append("UserId: ");
            sb.Append(User.ProfileID);
            sb.Append(", ");

            sb.Append("BikeId: ");
            sb.Append(Bike.ID);

            sb.Append(" }");
            return sb.ToString();
        }

        #endregion
    }
}
