using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using log4net;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private readonly UserFacade _uf;
        internal UserService(UserFacade uf)
        {
            _uf = uf;
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            try
            {
                _uf.Register(email, password);
                Response ret = new(null, null);
                log.Info($"User {email} has registed");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex) {
                Response ret = new(null, ex.Message);
                log.Warn($"User {email} has filed to log out: {ex.Message}");
                return ret.GetSerilizeResponse();
            }
            
        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="username">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs</returns>
        public string Login(string username, string password)
        {
            try {
                _uf.Login(username, password);
                Response ret = new(username, null);
                log.Info($"User {username} has loged in");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex){
                Response ret = new(null, ex.Message);
                log.Warn($"User {username} has filed to log in: {ex.Message}");
                return ret.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string Logout(string email) 
        {
            try
            {
                _uf.Logout(email);
                log.Info($"User {email} has loged out");
                return (new Response(null,null).GetSerilizeResponse());
            }
            catch (Exception ex)
            {
                Response ret = new(null, ex.Message);
                log.Warn($"User {email} has filed to log out: {ex.Message}");
                return ret.GetSerilizeResponse();
            }
        }


        
    }
}
