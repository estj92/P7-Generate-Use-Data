using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    static class Extensions
    {
        static Extensions()
        {
            Rand = new Random();
        }

        private static Random Rand { get; set; }


        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            if (source.Count() == 0)
            {
                return default(T);
            }
            else
            {
                return source.ElementAt(Rand.Next(source.Count()));
            }
        }

        public static string DateTimeToSequelize(this DateTime source)
        {
            return "(" + source.Year + ", " +
                      source.Month + ", " +
                      source.Day + ", " +
                      source.Hour + ", " +
                      source.Minute + ")";
        }
    }
}
