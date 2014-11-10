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

        public Generator(double minDistBetweenStations, int retryTimes, Coordinate topLeft, Coordinate bottomRight, DateTime earliest, DateTime latest, TimeSpan shortest, TimeSpan longest)
        {
            DoubleRandom = new Random();
            Rand = new Random();

            MinDistanceBetweenStations = minDistBetweenStations;

            RetryCreateCoordinatesTime = retryTimes;
            RetryCreateReservationTimes = retryTimes;
            RetryFindBikeTimes = retryTimes;

            TopLeft = topLeft;
            BottomRight = bottomRight;

            Earliest = earliest;
            Latest = latest;

            Shortest = shortest;
            Longest = longest;
        }

        private Random DoubleRandom { get; set; }
        private Random Rand { get; set; }

        public double MinDistanceBetweenStations { get; set; }

        private int RetryCreateCoordinatesTime { get; set; }
        private int RetryCreateReservationTimes { get; set; }
        private int RetryFindBikeTimes { get; set; }

        private Coordinate TopLeft { get; set; }
        private Coordinate BottomRight { get; set; }

        private DateTime Earliest { get; set; }
        private DateTime Latest { get; set; }

        private TimeSpan Shortest { get; set; }
        private TimeSpan Longest { get; set; }

        #region Coordinate helpers
        public double RandomDouble(double min, double max)
        {
            return DoubleRandom.NextDouble() * (max - min) + min;
        }

        public Coordinate RandomCoordinateInArea(Coordinate topLeft, Coordinate bottomRight)
        {
            Coordinate newCoords = new Coordinate(RandomDouble(topLeft.Lattitude, bottomRight.Lattitude), RandomDouble(bottomRight.Longtitude, topLeft.Longtitude));
            return newCoords;
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

        private Coordinate TryToCreateCoordinates(double minDist, List<Coordinate> coordinates, Coordinate topLeft, Coordinate bottomRight)
        {
            for (int i = 0; i < RetryCreateCoordinatesTime; i++)
            {
                var newCoords = RandomCoordinateInArea(topLeft, bottomRight);

                if (!IsCoordsCloseToAnyOther(newCoords, coordinates, minDist))
                {
                    return newCoords;
                }
            }

            throw new Exception();
        }
        #endregion


        #region Users
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
        #endregion

        #region Stations
        public IEnumerable<Station> GenerateStations(int n)
        {
            var stations = new List<Station>(n);
            var coordinates = new List<Coordinate>(n);

            for (int i = 0; i < n; i++)
            {
                var newCoords = TryToCreateCoordinates(MinDistanceBetweenStations, coordinates, TopLeft, BottomRight);

                coordinates.Add(newCoords);
                Station station = new Station(newCoords);
                stations.Add(station);
            }

            return stations;
        }
        #endregion

        #region Bikes
        public IEnumerable<Bike> GenerateBikes(int n, IEnumerable<Station> stations)
        {
            Random rand = new Random();
            var bikes = new List<Bike>(n);

            for (int i = 0; i < n; i++)
            {
                if (rand.Next(5) < 3)
                {
                    // out
                    var coords = RandomCoordinateInArea(TopLeft, BottomRight);
                    Bike bike = new Bike(i, coords, false);
                    bikes.Add(bike);
                }
                else
                {
                    // station
                    var station = stations.ElementAt(rand.Next(stations.Count()));
                    Bike bike = new Bike(i, station.Location, true);
                    bikes.Add(bike);
                }
            }

            return bikes;
        }
        #endregion

        #region Reservations
        private Reservation TryToCreateReservation(IEnumerable<User> users, IEnumerable<Station> stations, IEnumerable<Bike> bikes, DateTime earliest, DateTime latest, Dictionary<User, List<DateTime>> userReservations, Dictionary<Bike, List<DateTime>> bikesReserved)
        {
            TimeSpan timeBetween = latest - earliest;
            for (int i = 0; i < RetryCreateReservationTimes; i++)
            {
                var user = users.PickRandom();
                var bike = bikes.PickRandom();
                var station = stations.PickRandom();
                var when = earliest.AddMinutes(Rand.Next((int)timeBetween.TotalMinutes));

                var bikeOkay = !(bikesReserved.ContainsKey(bike) && bikesReserved[bike].Contains(when));
                var userOkay = !(userReservations.ContainsKey(user) && userReservations[user].Contains(when));

                if (bikeOkay && userOkay)
                {
                    if (!bikesReserved.ContainsKey(bike))
                    {
                        bikesReserved.Add(bike, new List<DateTime>());
                    }
                    bikesReserved[bike].Add(when);

                    if (!userReservations.ContainsKey(user))
                    {
                        userReservations.Add(user, new List<DateTime>());
                    }
                    userReservations[user].Add(when);

                    return new Reservation(user, station, bike, when);
                }
            }

            throw new Exception();
        }
        // I'M THE CORNER CUTTER!
        public IEnumerable<Reservation> GenerateReservations(int n, IEnumerable<User> users, IEnumerable<Station> stations, IEnumerable<Bike> bikes)
        {
            var reservations = new List<Reservation>(n);
            var userReservations = new Dictionary<User, List<DateTime>>();
            var bikeReservations = new Dictionary<Bike, List<DateTime>>();

            for (int i = 0; i < n; i++)
            {
                var reservation = TryToCreateReservation(users, stations, bikes, Earliest, Latest, userReservations, bikeReservations);
                reservations.Add(reservation);
            }

            return reservations;
        } 
        #endregion

        // Do we care for night time?
        public IEnumerable<Trip> GenerateTrips(int n, IEnumerable<User> users, IEnumerable<Station> stations, IEnumerable<Bike> bikes)
        {
            var trips = new List<Trip>(n);
            var bikesInUse = new Dictionary<Bike, Tuple<DateTime, DateTime>>();
            var userInUse = new Dictionary<User, Tuple<DateTime, DateTime>>();

            for (int i = 0; i < n; i++)
            {
                //
            }

            return trips;
        }

        private Bike FindABikeForATrip(DateTime earliest, DateTime latest, List<Bike> bikes, Dictionary<Bike, Tuple<DateTime, DateTime>> bikesInUse, TimeSpan shortestTime, TimeSpan longestTime)
        {


            for (int i = 0; i < RetryFindBikeTimes; i++)
            {
                var bike = bikes[Rand.Next(bikes.Count)];

                // never been used
                if (!bikesInUse.ContainsKey(bike))
                {

                }
                else
                {
                    var uses = bikesInUse[bike];
                }
            }

            throw new NotImplementedException();
        }
    }
}
