﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class members
    {
        internal int id;
        internal string email;
    
        internal members(int id, string email)
        {
            this.id = id;
            this.email = email;
        }
    }
}
