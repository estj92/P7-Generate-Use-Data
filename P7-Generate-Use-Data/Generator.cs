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

        private Random NameRand = new Random();
        public string GetRandomName()
        {
            var first = FirstNames[NameRand.Next(0, FirstNames.Length)];
            var last = LastNames[NameRand.Next(0, LastNames.Length)];

            return first + " " + last;
        }
        #endregion



        public IEnumerable<User> GenerateUsers(int n)
        {
            var users = new List<User>(n);

            for (int i = 0; i < n; i++)
            {
                User user = new User(n, GetRandomName());
                users.Add(user);
            }

            return users;
        }
    }
}
