using System;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserFacade
    {
        private readonly Dictionary<string, UserBL> users = new();
        private readonly Authenticator authenticator;
        private readonly UserController uc;

        internal UserFacade(Authenticator at)
        {
            users = new();
            authenticator = at;
            uc = new UserController();
        }


        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>void </returns>
        internal void Register(string email, string password) {
            if (users.ContainsKey(email)) { throw new Exception($"email {email} already exist"); }
            UserBL ubl = new(email, password);
            users.Add(email, ubl);
            authenticator.Conect(email);
        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>void </returns>
        internal void Login(string email, string password){
            if (!users.ContainsKey(email)){throw new Exception("failed to conect");}
            if (!users[email].ChackPasswordMatch(password)){throw new Exception("failed to conect");}

            authenticator.Conect(email);
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>void </returns>
        internal void Logout(string email) {
            if (!users.ContainsKey(email)) { throw new Exception("failed to disconect"); }
            authenticator.Disconnect(email);
        }

        internal void LoadData()
        {
            uc.GetAllUsers().ForEach(u => { users.Add(u.Email, new UserBL(u)); });
        }
        
        internal void DeleteData()
        {
           if(!uc.Claer())
            {
                throw new Exception("Faild to clear Data");
            }
            users.Clear();
        }
        
    }   
}
