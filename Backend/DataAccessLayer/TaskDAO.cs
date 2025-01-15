using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskDAO
    {
        internal int Id {  get; set; }
        internal int BoardId {  get; set; }
        private string title;
        private string description;
        private int status;
        private readonly DateTime creationDate;
        private DateTime dueDate;
        private string asignTo;
        


        internal string Title
        { 
            get { return title; }
            set {
                if (IsPersist) { tc.Update(Id, TitleCol, value); }
                title = value;
            } 
        }
        internal string Description 
        { 
            get { return description; }
            set {
                if (IsPersist) { tc.Update(Id, DescriptionCol, value); }
                description = value;
            } 
        }
        internal int Status
        {
            get { return status; }
            set
            {
                if (IsPersist) { tc.Update(Id, StatusCol, value); }
                status = value;
            }
        }
        internal string AsignTo 
        { 
            get { return asignTo; } 
            set {
                if (IsPersist) { tc.Update(Id, AsingToCol, value); }
                asignTo = value;
            } 
        }


        internal DateTime CreationDate{ get { return creationDate; } set {} }
        internal DateTime DueDate { get { return dueDate; } set {
            if (IsPersist) { tc.Update(Id,DueDateCol,value.ToString()); }
            dueDate = value;
            } }
        //TODO add dates


        internal readonly string idCol = "Id";
        internal readonly string BoardIdCol = "BoardId";
        internal readonly string TitleCol = "Title";
        internal readonly string DescriptionCol = "Description";
        internal readonly string StatusCol = "Status";
        internal readonly string AsingToCol = "AsignTo";
        internal readonly string CreatCol = "CreationDate";
        internal readonly string DueDateCol = "DueDate";
        private bool IsPersist = false;
        private TaskController tc;

        internal TaskDAO(string email, int id, string title, string description, DateTime dueDate,DateTime creationDate, int BoardId)
        {
            this.tc = new TaskController();
            this.creationDate = DateTime.Now;
            this.AsignTo = email;
            this.Id = id;
            this.BoardId = BoardId;
            this.Title = title;
            this.Description = description;
            this.Status = 0;
            this.CreationDate = creationDate;
            this.dueDate = dueDate;

            Persist();
        }

        internal TaskDAO(SQLiteDataReader reader) 
        {
            this.tc = new TaskController();
            Id = (int)reader.GetValue(0);
            BoardId = (int)reader.GetValue(1);
            Title = reader.GetString(2);
            Status = (int)reader.GetValue(3);
            Description = reader.GetString(4);
            AsignTo = reader.GetString(5);
            DueDate = DateTime.Parse(reader.GetString(6));
            creationDate = DateTime.Parse(reader.GetString(7));
            IsPersist = true;
        }

        internal void Persist()
        {
            if (IsPersist) { throw new Exception("alredy saved"); }
            tc.Insert(this);
            IsPersist = true;
        }
    }
}
