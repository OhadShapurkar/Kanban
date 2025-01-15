using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserDAO
    {
        internal string Email {  get; set; }
        internal string Password {  get; set; }




        private bool IsPersist = false;
        internal readonly string UserEmailColumnName = "Email";
        internal readonly string UserPassColumnName = "Password";
        private readonly UserController uc;

        internal UserDAO(string email, string password)
        {
            this.uc = new UserController();
            this.Email = email;
            this.Password = password;
            Persist();
        }
        internal UserDAO(SQLiteDataReader reader) {
            Email = reader.GetString(0);
            Password = reader.GetString(1);
            IsPersist = true;
        }

        internal void Persist()
        {
            uc.Insert(this);
            IsPersist = true;
        }
    }
}
