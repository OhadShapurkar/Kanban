using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardSL
    {
        public string _name;
        public string _id;
        public Task[] _tasks;
        public BoardSL(string name,  string id, Task[] tasks)
        {
            _name = name;
            _id = id;
            _tasks = tasks;
        }
            
    }
}
