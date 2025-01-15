using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Authenticator
    {
        private HashSet<string> users;

        internal Authenticator() { 
            users = new HashSet<string>();
        }

        /// <summary>
        /// This method add email to the conected set
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <returns>void </returns>
        internal void Conect(string email){
            if(email == null) { throw new ArgumentNullException("email is null"); }
            if (users.Contains(email)) { throw new ArgumentException("email already conected"); }
            users.Add(email);
        }

        /// <summary>
        /// This method remove email to the conected set
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <returns>void </returns>
        internal void Disconnect(string email) {
            if(email == null) { throw new ArgumentNullException("email is null");}
            if (!users.Contains(email)) { throw new Exception("user not conected"); }
            users.Remove(email);
        }

        /// <summary>
        /// This method check if a user is loged in.
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <returns>true if the email is in the set</returns>
        internal bool IsConect(string email){
            return users.Contains(email);
        }

    }
}
