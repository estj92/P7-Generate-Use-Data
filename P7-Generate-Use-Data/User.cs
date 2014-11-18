using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7_Generate_Use_Data
{
    class User: IEquatable<User>, ISequelize
    {
        public User(string profileId, string provider, string mail, string name)
        {
            ProfileID = profileId;
            Provider = provider;
            Mail = mail;
            Name = name;
        }

        public string ProfileID { get; set; }
        public string Provider { get; set; }
        public string Mail { get; set; }
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

            return this.ProfileID == other.ProfileID;
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
            return ProfileID.GetHashCode();
        }
        #endregion

        #region ISequelize Members

        public string ToSequelize()
        {
            StringBuilder sb = new StringBuilder("{ ");

            sb.Append("profile_id: ");
            sb.Append(ProfileID);
            sb.Append(", ");

            sb.Append("provider: \"");
            sb.Append(Provider);
            sb.Append("\", ");

            sb.Append("email: ");
            sb.Append(Mail);
            sb.Append(", ");

            sb.Append("display_name: \"");
            sb.Append(Name);
            sb.Append("\"");

            sb.Append(" }");
            return sb.ToString();
        }

        #endregion
    }
}
