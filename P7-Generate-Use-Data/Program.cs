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

            foreach (var item in generator.GenerateStations(100, 1, new Coordinate(100, 100), new Coordinate(200, 200)))
            {
                Console.WriteLine(item);
            }


            Console.WriteLine("~~~~~DONE~~~~~");
            Console.ReadKey();
        }
    }
}
