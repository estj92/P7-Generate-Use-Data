using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class Program
    {
        static void Main(string[] args)
        {
            var topLeft = new Coordinate(100, 100);
            var bottomRight = new Coordinate(200, 200);
            DateTime earliest = DateTime.Now.AddYears(-10);
            DateTime latest = DateTime.Now;
            TimeSpan shortest = new TimeSpan(0, 10, 0);
            TimeSpan longest = new TimeSpan(3, 0, 0);
            Generator generator = new Generator(1, 10, topLeft, bottomRight, earliest, latest, shortest, longest);


            var users = generator.GenerateUsers(2000);
            var stations = generator.GenerateStations(50);
            var bikes = generator.GenerateBikes(150, stations);
            var reservations = generator.GenerateReservations(users.Count() * 3, users, stations, bikes);
            var trips = generator.GenerateTrips(10000, users, stations, bikes);


            Console.WriteLine("Users:\t\t" + users.Count().ToString().PadLeft(5));
            Console.WriteLine("Stations:\t" + stations.Count().ToString().PadLeft(5));
            Console.WriteLine("Bikes:\t\t" + bikes.Count().ToString().PadLeft(5));
            Console.WriteLine("Reservations:\t" + reservations.Count().ToString().PadLeft(5));
            Console.WriteLine("Trips:\t\t" + trips.Count().ToString().PadLeft(5));

            var b2 = bikes.Take(10);
            var r2 = reservations.Take(10);
            var s2 = stations.Take(10);
            var t2 = trips.Take(10);
            var u2 = users.Take(10);

            Print(b2);
            //Print(r2);
            //Print(s2);
            //Print(t2);
            //Print(u2);

            Console.WriteLine("~~~~~DONE~~~~~");
            Console.ReadKey();
        }

        private static void Print(IEnumerable<ISequelize> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item.ToSequelize());
            }
        }
    }
}
