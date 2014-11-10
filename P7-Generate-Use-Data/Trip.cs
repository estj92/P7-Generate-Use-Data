﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Trip
    {
        public Trip(User user, Bike bike, DateTime startTime, DateTime endTime, Coordinate startLocation, Coordinate endLocation)
        {
            User = user;
            Bike = bike;
            StartTime = startTime;
            Endtime = endTime;
            StartLocation = startLocation;
            EndLocation = endLocation;
        }

        public User User { get; set; }
        public Bike Bike { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Endtime { get; set; }
        public Coordinate StartLocation { get; set; }
        public Coordinate EndLocation { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(User == null ? "No user" : User.Name);
            sb.Append('\t');
            sb.Append(Bike.ID.ToString());
            sb.Append('\t');
            sb.Append((Endtime - StartTime).TotalMinutes + " minutes");

            return sb.ToString();
        }
    }
}
