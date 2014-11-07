using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Generator
    {
        #region Names
        private string[] FirstNames = new string[] { 
            "Emil", 
            "Mathew",
            "Lars",
            "Eric",
            "Jens",
            "Morten",
            "Peter",
            "Simon",
            "Julie",
            "Line",
            "Trine",
            "Pia",
            "Svend" 
        };

        private string[] LastNames = new string[]{
            "Jensen",
            "Svendsen",
            "Fuglsang",
            "Petersen",
            "Madsen", 
            "Mikkelsen"
        };
        #endregion



        public IEnumerable<User> GenerateUsers(int n)
        {
            var users = new List<User>(n);
            Random rand = new Random();

            for (int i = 0; i < n; i++)
            {
                var first = FirstNames[rand.Next(0, FirstNames.Length)];
                var last = LastNames[rand.Next(0, LastNames.Length)];

                User user = new User(n, first + " " + last);
                users.Add(user);
            }

            return users;
        }

        Random DoubleRandom = new Random();
        public double RandomDouble(double min, double max)
        {
            return DoubleRandom.NextDouble() * (max - min) + min;
        }

        public bool IsCoordsCloseToAnyOther(Coordinate newCoord, IEnumerable<Coordinate> otherCoords, double minDist)
        {
            foreach (var coord in otherCoords)
            {
                var xDist = Math.Pow((newCoord.Longtitude - coord.Longtitude), 2);
                var yDist = Math.Pow((newCoord.Lattitude - coord.Lattitude), 2);
                var dist = Math.Sqrt(xDist + yDist);
                var absDist = Math.Abs(dist);

                if (absDist < minDist)
                {
                    return true;
                }
            }

            return false;
        }

        private int RetryCreateCoordinatesTime { get { return 5; } }
        private Coordinate TryToCreateCoordinates(double minDist, List<Coordinate> coordinates, Coordinate topLeft, Coordinate bottomRight)
        {
            for (int i = 0; i < RetryCreateCoordinatesTime; i++)
            {
                Coordinate newCoords = new Coordinate(RandomDouble(topLeft.Lattitude, bottomRight.Lattitude), RandomDouble(bottomRight.Longtitude, topLeft.Longtitude));

                if (!IsCoordsCloseToAnyOther(newCoords, coordinates, minDist))
                {
                    return newCoords;
                }
            }

            throw new Exception();
        }

        public IEnumerable<Station> GenerateStations(int n, double minDist, Coordinate topLeft, Coordinate bottomRight)
        {
            var stations = new List<Station>(n);
            var coordinates = new List<Coordinate>(n);

            for (int i = 0; i < n; i++)
            {
                var newCoords = TryToCreateCoordinates(minDist, coordinates, topLeft, bottomRight);

                coordinates.Add(newCoords);
                Station station = new Station(newCoords);
                stations.Add(station);
            }

            return stations;
        }




        public IEnumerable<Bike> GenerateBikes(int n)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reservation> GenerateReservations(int n, IEnumerable<User> users, IEnumerable<Station> stations, IEnumerable<Bike> bikes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trip> GenerateTrips(int n, IEnumerable<User> users, IEnumerable<Station> stations, IEnumerable<Bike> bikes)
        {
            throw new NotImplementedException();
        }
    }
}
