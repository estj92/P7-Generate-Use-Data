using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class User: IEquatable<User>, ISequelize
    {
        public User(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int ID { get; set; }
        public string Name { get; set; }


        public override string ToString()
        {
            return Name;
        }

        #region IEquatable<User> Members
        public bool Equals(User other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            User user = obj as User;

            return ReferenceEquals(user, null) ?
                false :
                this.Equals(user);
        }

        public static bool operator ==(User u1, User u2)
        {
            if (ReferenceEquals(u1, u2))
            {
                return true;
            }
            if (ReferenceEquals(u1, null) || ReferenceEquals(u2, null))
            {
                return false;
            }

            return u1.Equals(u2);
        }

        public static bool operator !=(User u1, User u2)
        {
            return !(u1 == u2);
        }

        public override int GetHashCode()
        {
            return ID;
        }
        #endregion

        #region ISequelize Members

        public string ToSequelize()
        {
            StringBuilder sb = new StringBuilder("{ ");



            sb.Append(" }");
            return sb.ToString();
        }

        #endregion
    }
}
