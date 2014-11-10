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
            TimeSpan longest = new TimeSpan(6, 0, 0);
            Generator generator = new Generator(1, 10, topLeft, bottomRight, earliest, latest, shortest, longest);


            var users = generator.GenerateUsers(2000);
            var stations = generator.GenerateStations(50);
            var bikes = generator.GenerateBikes(150, stations);
            var reservations = generator.GenerateReservations(users.Count() * 3, users, stations, bikes);
            // trips


            Console.WriteLine("Users:\t\t" + users.Count().ToString().PadLeft(5));
            Console.WriteLine("Stations:\t" + stations.Count().ToString().PadLeft(5));
            Console.WriteLine("Bikes:\t\t" + bikes.Count().ToString().PadLeft(5));
            Console.WriteLine("Reservations:\t" + reservations.Count().ToString().PadLeft(5));


            Console.WriteLine("~~~~~DONE~~~~~");
            Console.ReadKey();
        }
    }
}
