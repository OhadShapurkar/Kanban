    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardBL
    {
        private readonly BoardDAO dao;
        private readonly TaskController tc;
        private string oner;
        private readonly string name;
        private readonly int id;
        private readonly LinkedList<TaskBL> backlog;
        private int backlogLimit;
        private readonly LinkedList<TaskBL> inProgess;
        private int inProgessLimit;
        private readonly LinkedList<TaskBL> done;
        private int doneLimit;
        private readonly LinkedList<string> members;

        internal int Id {  get { return id; } }
        internal string Oner {  get { return oner; } }
        internal string Name { get { return name; } }

        internal int BacklogLimit { get { return backlogLimit;  }}

        internal int InProgressLimit { get { return inProgessLimit; }}

        internal int DoneLimite {  get { return doneLimit; }}

        internal BoardBL(string oner,string name,int id)
        {
            this.dao = new BoardDAO(id, oner, name);
            this.tc = new();
            this.id = id;
            this.oner = oner;
            this.name = name;
            this.backlog = new LinkedList<TaskBL>();
            this.backlogLimit = -1;
            this.inProgess = new LinkedList<TaskBL>();
            this.inProgessLimit = -1;
            this.done = new LinkedList<TaskBL>();
            this.doneLimit = -1;
            this.members = new LinkedList<string>();
            dao.JoinBoard(oner);
            members.AddFirst(oner);
            

            
        }

        internal BoardBL(BoardDAO dao)
        {
            tc = new();
            this.dao = dao;
            id = dao.BoardID;
            oner = dao.Owner;
            name = dao.Name;
            backlogLimit = dao.BacklogLimit;
            inProgessLimit = dao.InProgressLimit;
            doneLimit = dao.DoneLimite;

            backlog = new LinkedList<TaskBL>();
            inProgess = new LinkedList<TaskBL>();
            done = new LinkedList<TaskBL>();

            // get all task of the board and add them to colums
            tc.GetAllTask(Id).ForEach(t =>
            {
                switch (t.Status)
                {
                    case 0:
                        backlog.AddFirst(new TaskBL(t)); break;
                    case 1:
                        inProgess.AddFirst(new TaskBL(t)); break;
                    case 2:
                        done.AddFirst(new TaskBL(t)); break;
                    default:
                        break;
                }
            });
            members = new LinkedList<string>(dao.GetMember());
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="_id">Id of the task</param>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>void </returns>
        internal void CreatTask(int _id, string title,string description,DateTime dueDate) 
        {
            if ((this.backlogLimit == backlog.Count))
            {
                throw new Exception("cannot create new task, reached to backlog task limit");
            }
            backlog.AddLast(new TaskBL(_id, title, description, dueDate,this.id));
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>void </returns>
        internal void MoveTask(string email,int col,int taskId) 
        { 
            TaskBL task = GetTask(taskId,col);
            switch(col)
            {
                case 0:
                    if(inProgess.Count == inProgessLimit) { throw new Exception("You reached the limit of the column"); }
                    task.MoveTeask(email,1);
                    backlog.Remove(task);
                    inProgess.AddLast(task);
                    break;
                case 1:
                    if (done.Count == doneLimit) { throw new Exception("You reached the limit of the column"); }
                    task.MoveTeask(email,2);
                    inProgess.Remove(task);
                    done.AddLast(task);
                    break;
                case 2:throw new Exception("You can't advanse the task any ferther");
                default: throw new Exception($"There is no {col} column");
            }
        }

        private TaskBL GetTask(int id,int col)
        {
            switch (col)
            {
                case 0:
                    foreach (TaskBL node in this.backlog)
                    {
                        if (node.Id == id)return node;
                    }
                    break;
                case 1:
                    foreach (TaskBL node in this.inProgess)
                    {
                        if (node.Id == id)
                            return node;
                    }
                    break;
                case 2:
                    foreach (TaskBL node in this.done)
                    {
                        if (node.Id == id)
                            return node;
                    }
                    break ;
                default: throw new Exception($"There is no {col} column");
            }
            throw new Exception("task not found");
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newTitle">New title for the task</param>
        /// <returns>void </returns>
        internal void EditsTaskTitel(int taskId,int col,string newTitle) 
        {
            TaskBL task  = GetTask(taskId,col);
            task.Title = newTitle;
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newDescription">New description for the task</param>
        /// <returns>void </returns>
        internal void EditsTaskDescription(int taskId,int col, string newDescription)
        {
            TaskBL task = GetTask(taskId,col);
            task.Description = newDescription;
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>void </returns>
        internal void EditsTaskDueDate(int taskId,int col, DateTime dueDate) 
        {
            TaskBL task = GetTask(taskId, col);
            task.DueDate = dueDate;
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        internal void ChangeLimit(int col,int limit) {
            if (limit < -1) { throw new Exception("limit canot be negetiv"); }
            switch (col)
            {
                case 0:
                    if (limit < backlog.Count && limit != -1) { throw new Exception("Limit to small' try moving some task before changing the limit"); }
                    dao.BacklogLimit = limit;
                    backlogLimit = limit;
                    break;
                case 1:
                    if (limit < inProgess.Count && limit != -1) { throw new Exception("Limit to small' try moving some task before changing the limit"); }
                    dao.InProgressLimit = limit;
                    inProgessLimit = limit;
                    break;
                case 2:
                    if (limit < done.Count && limit != -1) { throw new Exception("Limit to small' try moving some task before changing the limit"); }
                    dao.DoneLimite = limit;
                    doneLimit = limit;
                    break;
                default: throw new Exception($"There is no {col} column");
            }
                
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The column's limit</returns>
        internal int GetLimit(int col) 
        {
            return col switch
            {
                0 => this.backlogLimit,
                1 => this.inProgessLimit,
                2 => this.doneLimit,
                _ => throw new Exception("Invalid column number"),
            };
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A list of the column's tasks</returns>
        internal LinkedList<TaskBL> GetColumn(int col)
        {
            return col switch
            {
                0 => backlog,
                1 => inProgess,
                2 => done,
                _ => throw new Exception($"There is no {col} column"),
            };
        }


        internal LinkedList<TaskBL> GetInProgres(string email)
        {
            LinkedList<TaskBL> ret = new();
            foreach (var task in inProgess)
            {
                if (email == task.AssignTo) { ret.AddLast(new TaskBL(task)); }
            }
            return ret;
        }
        internal void DeleteBoard(string email)
        {
            if(email != oner) { throw new Exception($"{email} is not the owner of board {name}"); }
            tc.Delete(Id);
            dao.DeleteBoard();
        }

        internal void DeleteData()
        {
            if (!tc.Claer())
            {
                throw new Exception("Faild to clear Data");
            }
        }

        internal void JoinBoard(string email)
        {
            dao.JoinBoard(email);
            members.AddLast(email);
        }

        internal void LeaveBoard(string email)
        {
            if (email == oner) { throw new Exception("Owner cant leave board"); }
            
            foreach (var t in backlog){if(email == t.AssignTo) { t.AssignTo = "unassigned"; } }
            foreach (var t in inProgess) { if (email == t.AssignTo) { t.AssignTo = "unassigned"; } }
            foreach (var t in done) { if (email == t.AssignTo) { t.AssignTo = "unassigned"; } }
            
            dao.LeaveBoard(email);
            members.Remove(email);
        }

        internal void GetBoardName(int boardID)
        {
            throw new NotImplementedException();
        }
        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail)
        {
            if (oner != currentOwnerEmail) { throw new Exception($"User {currentOwnerEmail} is not the current owner"); }
            if (!members.Contains(newOwnerEmail)) { throw new Exception($"User {newOwnerEmail} is not a nenber of the board {name}"); }
            dao.Owner = newOwnerEmail;
            oner = newOwnerEmail;
        }

        internal void AssignTask( int columnOrdinal, int taskID, string emailAssignee)
        {
            if (!members.Contains(emailAssignee)) { throw new Exception($"User {emailAssignee} is not a member of board {name}"); }
            TaskBL t = GetTask(taskID,columnOrdinal);
            t.AssignTo = emailAssignee;
        }

        internal void Claer() 
        {
            if (!tc.Claer())
            {
                throw new Exception("Faild to clear Data");
            }
        }
    }
}
