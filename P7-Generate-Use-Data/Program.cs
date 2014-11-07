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
            Generator generator = new Generator();

            var topLeft = new Coordinate(100, 100);
            var bottomRight = new Coordinate(200, 200);

            var users = generator.GenerateUsers(2000);
            var stations = generator.GenerateStations(50, 1, topLeft, bottomRight);
            var bikes = generator.GenerateBikes(150, stations, topLeft, bottomRight);
            var reservations = generator.GenerateReservations(users.Count() * 3, users, stations, bikes, DateTime.Now.AddYears(-10), DateTime.Now);
            // trips

            foreach (var item in reservations.Take(100))
            {
                Console.WriteLine(item);
            }


            Console.WriteLine("~~~~~DONE~~~~~");
            Console.ReadKey();
        }
    }
}
