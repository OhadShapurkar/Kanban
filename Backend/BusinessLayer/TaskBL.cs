using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class TaskBL
    {
        private readonly TaskDAO dao;
        
        private readonly int id;
        private string title;
        private string description;
        private readonly DateTime creationDate;
        private DateTime dueDate;
        private string asignTo;

        internal TaskBL(int id, string title, string description, DateTime dueDate,int board)
        {
            if (dueDate <= DateTime.Now)
            {
                throw new Exception("due date should be after create date!");
            }
            if (title.Length > 50 || title.Length == 0|| description.Length > 300 || description.Length == 0) { throw new Exception("Title or description are to long"); }
            //dao = new TaskDAO(email, id,title,description,dueDate,board);
            this.creationDate = DateTime.Now;
            dao = new TaskDAO("unassigned", id,title,description,dueDate,creationDate,board);
            this.asignTo = "unassigned";
            this.id = id;
            this.Title = title;
            this.Description = description;
            this.dueDate = dueDate;
        }
        internal TaskBL(TaskBL other)
        {
            // TODO : build DAO????
            //dao = other.dao;
            this.id=other.id;
            this.creationDate = other.CreationDate;
            this.asignTo = other.asignTo;
            this.id = other.id;
            this.title = other.Title;
            this.description = other.description;
            this.dueDate = other.dueDate;
        }
        internal TaskBL(TaskDAO dao)
        {
            this.dao = dao;
            id = dao.Id;
            title = dao.Title;
            description = dao.Description;
            creationDate = dao.CreationDate;
            dueDate = dao.DueDate;
            asignTo = dao.AsignTo;
        }
        internal int Id { get { return id; } }
        internal string Title { get { return title; } set {
                if (value == null || value.Length <= 0 || value.Length > 50) { throw new Exception("Task can not be with empty title or be more than 50 characters"); }
                dao.Title = value;
                title = value;
            } }
        internal string Description { get { return description; } set 
            {
                if (value == null || value.Length == 0 || value.Length > 300) { throw new Exception("Task can not be with empty description or more than 300 characters"); }
                if (value.Length > 300) { throw new Exception("Description to large"); }
                dao.Description = value;
                description = value; 
            } }
        internal DateTime CreationDate { get { return creationDate; } }
        internal DateTime DueDate { get { return dueDate; } set {
                
                if (value <= DateTime.Now)
                {
                    throw new Exception("due date should be after create date!");
                }
                //TODO change Dao
                dueDate = value;
            }}
        internal string AssignTo { get { return asignTo; } set {
                if (value == AssignTo) { throw new Exception($"User {value} is assign to this task"); }
                dao.AsignTo = value;
                asignTo = value;
            } }
        internal void MoveTeask(string email,int col)
        {
            if(email != asignTo) { throw new Exception($"{email} is not asign to task number {id}"); }
            dao.Status = col;
        }
    }
}
