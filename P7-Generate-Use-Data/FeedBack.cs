using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class FeedBack: ISequelize
    {
        public FeedBack(Coordinate location, string message, int userId)
        {
            ID = ID_Generator;
            Location = location;
            Message = message;
            UserID = userId;
        }

        private static int _IDGen;
        private static int ID_Generator { get { return _IDGen++; } }

        public int ID { get; set; }
        public Coordinate Location { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }

        #region ISequelize Members

        public string ToSequelize()
        {
            StringBuilder sb = new StringBuilder("{ ");

            sb.Append("location: ")
                .Append(Location.LocationToSequelize)
                .Append(", ");

            sb.Append("message")
                .Append(Message)
                .Append(", ");

            sb.Append("UserId")
                .Append(UserID);

            sb.Append(" }");
            return sb.ToString();
        }

        #endregion
    }
}
