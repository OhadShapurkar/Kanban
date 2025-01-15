using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserSL
    {

        //public int id;
        public string Email { get; set; }
        public LinkedList<string> Bords { get; set; }


        public UserSL() {
        Bords = new LinkedList<string>();
        }
        public UserSL(string email)
        {
            
            this.Email = email;
            Bords = new LinkedList<string>();
        }

        internal UserSL(UserBL userBL)
        {
            //this.Bords = new LinkedList<string>(userBL);
            this.Email = userBL.Email;
        }

    }
}
