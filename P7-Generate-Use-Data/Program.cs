using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace P7_Generate_Use_Data
{
    class Program
    {
        private static char[] CommentSplitter = new char[] { '#' };
        private static char[] CommaSplitter = new char[] { ',' };
        private static int GetInt(string str)
        {
            var split = str.Split(CommentSplitter);
            return int.Parse(split[0]);
        }
        private static Coordinate GetCoords(string str)
        {
            var split = str.Split(CommentSplitter);
            var vals = split[0].Split(CommaSplitter);
            return new Coordinate(double.Parse(vals[0]), double.Parse(vals[1]));
        }

        private static IEnumerable<string> ReadNames(string str)
        {
            return
                str.Split(CommaSplitter)
                .Select(s => s.Trim());
        }

        static void Main(string[] args)
        {
            IEnumerable<string> firstNames;
            IEnumerable<string> lastNames;
            int nUsers = 100;
            int nStations = 10;
            int nBikes = 50;
            int nReservations = 150;
            int nTrips = 200;

            Coordinate topLeft = new Coordinate(100, 100);
            Coordinate bottomRight = new Coordinate(200, 200);
            DateTime earliest = DateTime.Now.AddYears(-10);
            DateTime latest = DateTime.Now;
            TimeSpan shortest = new TimeSpan(0, 10, 0);
            TimeSpan longest = new TimeSpan(3, 0, 0);

            using (StreamReader reader = new StreamReader("settings.txt"))
            {
                firstNames = ReadNames(reader.ReadLine());
                lastNames = ReadNames(reader.ReadLine());
                nUsers = GetInt(reader.ReadLine());
                nStations = GetInt(reader.ReadLine());
                nBikes = GetInt(reader.ReadLine());
                nReservations = GetInt(reader.ReadLine());
                nTrips = GetInt(reader.ReadLine());

                topLeft = GetCoords(reader.ReadLine());
                bottomRight = GetCoords(reader.ReadLine());

                earliest = new DateTime(GetInt(reader.ReadLine()), 1, 1);
            }

            Generator generator = new Generator(firstNames, lastNames, 1, 10, topLeft, bottomRight, earliest, latest, shortest, longest);

            var users = generator.GenerateUsers(nUsers);
            var stations = generator.GenerateStations(nStations);
            var bikes = generator.GenerateBikes(nBikes, stations);
            var reservations = generator.GenerateReservations(nReservations, users, stations, bikes);
            var trips = generator.GenerateTrips(nTrips, users, stations, bikes);


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

            //Print(b2);
            //Print(r2);
            //Print(s2);
            //Print(t2);
            //Print(u2);

            WriteToFile(bikes.Cast<ISequelize>(), reservations.Cast<ISequelize>(), stations.Cast<ISequelize>(), trips.Cast<ISequelize>(), users.Cast<ISequelize>());

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

        private static void WriteToFile(IEnumerable<ISequelize> bikes, IEnumerable<ISequelize> reservations, IEnumerable<ISequelize> stations, IEnumerable<ISequelize> trips, IEnumerable<ISequelize> users)
        {
            string file = "create-use-data.js";
            File.Delete(file);

            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine(@"var models = require(__dirname + '/../../models');");
                writer.WriteLine("var Sequelize = require(\"sequelize\");");
                writer.WriteLine(@"module.exports = {");


                WriteItems(bikes, "Bike", writer);
                writer.WriteLine(",");
                WriteItems(reservations, "Reservation", writer);
                writer.WriteLine(",");
                WriteItems(stations, "Station", writer);
                writer.WriteLine(",");
                WriteItems(trips, "Trip", writer);
                writer.WriteLine(",");
                WriteItems(users, "User", writer);

                writer.WriteLine();
                writer.WriteLine("}");
            }
        }

        private static void WriteItems(IEnumerable<ISequelize> items, string name, StreamWriter writer)
        {
            writer.Write("  Create");
            writer.Write(name);
            writer.WriteLine("s: function(done){");
            writer.WriteLine("    models.sequelize.sync().success(function () {");
            writer.Write("      models.");
            writer.Write(name);
            writer.WriteLine(".bulkCreate([");
            writer.Write("        ");

            var sequelizeStrings = items.Select(i => i.ToSequelize());
            var sequeliseString = string.Join("," + Environment.NewLine + "        ", sequelizeStrings);
            writer.WriteLine(sequeliseString);

            writer.WriteLine("      ]).success(function() {");
            writer.WriteLine("        done();");
            writer.WriteLine("      })"); // semicolon?
            writer.WriteLine("    });");
            writer.Write("  }");
        }
    }
}
