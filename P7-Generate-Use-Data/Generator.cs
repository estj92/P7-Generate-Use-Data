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
        private string[] FirstNames { get; set; }
        private string[] LastNames { get; set; }

        private string RandomFirstName()
        {
            return FirstNames[Rand.Next(FirstNames.Length)];
        }
        private string RandomLastName()
        {
            return LastNames[Rand.Next(LastNames.Length)];
        }
        #endregion

        public Generator(IEnumerable<string> firstNames, IEnumerable<string> lastNames, double minDistBetweenStations, int retryTimes, Coordinate topLeft, Coordinate bottomRight, DateTime earliest, DateTime latest, TimeSpan shortest, TimeSpan longest)
        {
            DoubleRandom = new Random();
            Rand = new Random();

            FirstNames = firstNames.ToArray();
            LastNames = lastNames.ToArray();

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
        private TimeSpan TimeBetweenEarliestAndLatest { get { return Latest - Earliest; } }

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

        public bool IsCoordsCloseToAnyOther(Coordinate newCoord, List<Coordinate> otherCoords, double minDist)
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
        public List<User> GenerateUsers(int n)
        {
            var users = new List<User>(n);
            Random rand = new Random();

            for (int i = 0; i < n; i++)
            {
                string firstName = RandomFirstName();
                string lastName = RandomLastName();
                User user = new User(i.ToString(), "facebook", firstName + lastName + i + @"Someprovider.top", firstName + " " + lastName);
                users.Add(user);
            }

            return users;
        }
        #endregion

        #region Stations
        public List<Station> GenerateStations(int n)
        {
            var stations = new List<Station>(n);
            var coordinates = new List<Coordinate>(n);

            for (int i = 0; i < n; i++)
            {
                var newCoords = TryToCreateCoordinates(MinDistanceBetweenStations, coordinates, TopLeft, BottomRight);

                coordinates.Add(newCoords);
                Station station = new Station(RandomLastName() + Rand.Next() + "-station", newCoords, 10000, 0);
                stations.Add(station);
            }

            return stations;
        }
        #endregion

        #region Bikes
        public List<Bike> GenerateBikes(int n, List<Station> stations)
        {
            Random rand = new Random();
            var bikes = new List<Bike>(n);

            for (int i = 0; i < n; i++)
            {
                int battery = Rand.Next(0, 100);

                if (rand.Next(5) < 3)
                {
                    var status = ((Rand.Next(0, 5) > 0) ? Bike.Status.Occupied : Bike.Status.Unoccupied);

                    // out
                    var coords = RandomCoordinateInArea(TopLeft, BottomRight);
                    Bike bike = new Bike(i, battery, coords, status, null);
                    bikes.Add(bike);
                }
                else
                {
                    // station
                    var station = stations[rand.Next(stations.Count)];
                    Bike bike = new Bike(i, battery, station.Location, Bike.Status.Unoccupied, station);
                    station.Bikes++;
                    bikes.Add(bike);
                }
            }

            return bikes;
        }
        #endregion

        #region Reservations
        private Reservation TryToCreateReservation(List<User> users, List<Station> stations, List<Bike> bikes, DateTime earliest, DateTime latest, Dictionary<User, List<DateTime>> userReservations, Dictionary<Bike, List<DateTime>> bikesReserved)
        {
            TimeSpan timeBetween = latest - earliest;
            for (int i = 0; i < RetryCreateReservationTimes; i++)
            {
                var user = users.PickRandom();
                var bike = bikes.PickRandom();
                //var station = stations.PickRandom();
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

                    return new Reservation(user, bike, when);
                }
            }

            throw new Exception();
        }
        // I'M THE CORNER CUTTER!
        public List<Reservation> GenerateReservations(int n, List<User> users, List<Station> stations, List<Bike> bikes)
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

        #region Trips
        // Do we care for night time?
        public List<Trip> GenerateTrips(int n, List<User> users, List<Station> stations, List<Bike> bikes)
        {
            var trips = new List<Trip>(n);
            var bikesInUse = new Dictionary<Bike, List<Tuple<DateTime, DateTime>>>();
            var usersInUse = new Dictionary<User, List<Tuple<DateTime, DateTime>>>();

            for (int i = 0; i < n; i++)
            {
                var start = Earliest.AddMinutes(Rand.Next((int)TimeBetweenEarliestAndLatest.TotalMinutes));
                var end = start.AddMinutes(Rand.Next((int)Shortest.TotalMinutes, (int)Longest.TotalMinutes + 1));

                var bike = FindABikeForATrip(bikes, bikesInUse, start, end);
                var user = Rand.Next(0, 2) == 0 ?
                    FindAUserForATrip(users, usersInUse, start, end) :
                    null;

                Coordinate startCoord = Rand.Next(0, 4) == 0 ?
                    RandomCoordinateInArea(TopLeft, BottomRight) :
                    stations[Rand.Next(stations.Count)].Location;
                Coordinate endCoord = Rand.Next(0, 4) == 0 ?
                    RandomCoordinateInArea(TopLeft, BottomRight) :
                    stations[Rand.Next(stations.Count)].Location;

                Trip trip = new Trip(user, bike, start, end, startCoord, endCoord);

                trips.Add(trip);
            }

            return trips;
        }

        private Bike FindABikeForATrip(List<Bike> bikes, Dictionary<Bike, List<Tuple<DateTime, DateTime>>> bikesInUse, DateTime start, DateTime end)
        {
            for (int i = 0; i < RetryFindBikeTimes; i++)
            {
                var bike = bikes[Rand.Next(bikes.Count)];

                // never been used
                if (!bikesInUse.ContainsKey(bike))
                {
                    bikesInUse.Add(bike, new List<Tuple<DateTime, DateTime>>());
                }

                var uses = bikesInUse[bike];

                var timeOverlaps = uses
                    .FirstOrDefault(t => TimeOverlap(t.Item1, t.Item2, start, end));

                if (timeOverlaps == null)
                {
                    return bike;
                }
            }

            throw new Exception();
        }

        private User FindAUserForATrip(List<User> users, Dictionary<User, List<Tuple<DateTime, DateTime>>> usersInUse, DateTime start, DateTime end)
        {
            for (int i = 0; i < RetryFindBikeTimes; i++)
            {
                var user = users[Rand.Next(users.Count)];

                if (!usersInUse.ContainsKey(user))
                {
                    usersInUse.Add(user, new List<Tuple<DateTime, DateTime>>());
                }

                var uses = usersInUse[user];

                var timeOverlaps = uses.FirstOrDefault(t => TimeOverlap(t.Item1, t.Item2, start, end));

                if (timeOverlaps == null)
                {
                    return user;
                }
            }

            return null;
        }

        private bool TimeOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            // e1 < s2
            // e2 < s1
            return (end1 <= start2 || end2 <= start1);
        }
        #endregion

        #region Feedbacks
        public List<FeedBack> GenerateFeedBacks(int n, List<User> users)
        {
            var feedbacks = new List<FeedBack>(n);

            for (int i = 0; i < n; i++)
            {
                FeedBack fb = new FeedBack(RandomCoordinateInArea(TopLeft, BottomRight), "I IS VERY GOOD MESSAGE. SEND MONEY.", users.PickRandom().ID);
                feedbacks.Add(fb);
            }

            return feedbacks;
        }
        #endregion
    }
}
