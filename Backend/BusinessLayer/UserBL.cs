using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Globalization;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserBL
    {
        private string email;
        private readonly string password;
        private readonly UserDAO dao;
        //private int id;

        public string Email {
            get => email;
            set {
                string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$"; 

                if (!Regex.IsMatch(value.ToLower(), regex, RegexOptions.IgnoreCase)) throw new Exception($"{value} is not a valid email address");
                email = value ?? throw new Exception("email is null"); } }
        

        internal UserBL(string email, string password)
        {
            if (!CheckValidPassword(password)) { throw new Exception("Invalid password"); }
            dao = new UserDAO(email, password);
            
            this.Email = email;
            this.password = password;
           
        }

        internal UserBL(UserDAO userDAO) { 
            dao = userDAO;
            Email = userDAO.Email;
            password = userDAO.Password;
        }

        /// <summary>
        /// This method check the password of the user
        /// </summary>
        /// <param name="password">The user password.</param>
        /// <returns>true if the password match and false if not</returns>
        internal bool ChackPasswordMatch(string password)
        {
            return password.Equals(this.password);
        }


        private bool CheckValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$";
            if (password == null) { return false; }
            if (password.Length<6 || password.Length > 20) { return false; }
            if (!Regex.IsMatch(password, pattern)) { return false; }
         

            return true;
        }
    }
}
