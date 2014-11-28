using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Reservation: ISequelize
    {
        public Reservation(User user, Bike bike, DateTime startTime)
        {
            User = user;
            Bike = bike;
            StartTime = startTime;
        }

        public User User { get; set; }
        public Station Station { get; set; }
        public Bike Bike { get; set; }
        public DateTime StartTime { get; set; }


        public override string ToString()
        {
            return User + " " + Station + " " + Bike + " " + StartTime;
        }

        #region ISequelize Members

        public string ToSequelize()
        {
            StringBuilder sb = new StringBuilder("{ ");

            sb.Append("startTime: new Date")
                .Append(Extensions.DateTimeToSequelize(StartTime))
                .Append(", ");

            sb.Append("UserId: ")
                .Append(User.ProfileID)
                .Append(", ");

            sb.Append("BikeId: ")
                .Append(Bike.ID);

            sb.Append(" }");
            return sb.ToString();
        }

        #endregion
    }
}
