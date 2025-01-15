using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Task
    {
        public int Id {  get; set; }
        public DateTime CreationTime { get; set; }
        public string Title {  get; set; }
        public string Description {  get; set; }
        
        public DateTime DueDate {  get; set; }



        public Task(int id, string title, string description, DateTime publiction, DateTime dueDate)
        {
            Id = id;
            Title = title;
            Description = description;
            CreationTime = publiction;
            DueDate = dueDate;
        }

        internal Task(TaskBL task)
        {
            this.Id = task.Id;
            this.Title = task.Title;
            this.Description = task.Description;
            this.CreationTime = task.CreationDate;
            this.DueDate = task.DueDate;
        }

    }
}
