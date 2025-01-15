using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardFacade
    {
        private readonly Dictionary<int, BoardBL> board;
        private readonly Authenticator authenticator;
        private int taskIdCuonter;
        private int boardIdConter;
        private readonly BoardController bc;
        private readonly BoradUserController buc;
        private readonly Dictionary<string, LinkedList<int>> userToBoard;



        internal BoardFacade(Authenticator authenticator)
        {
            this.authenticator = authenticator;
            this.taskIdCuonter = 0;
            this.boardIdConter = 0;
            board = new Dictionary<int, BoardBL>();
            userToBoard = new Dictionary<string, LinkedList<int>>();
            bc = new();
            buc = new();
        }

        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>void </returns>
        internal void CreateBoard(string email, string name)
        {
            if (!authenticator.IsConect(email)) { throw new Exception("User not conected"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            if (GetBoardId(email,name) != -1) { throw new Exception($"user alredy have a {name} board"); }
            board[boardIdConter] = new BoardBL(email, name,boardIdConter);
            userToBoard[email].AddLast(boardIdConter);
            buc.Insert(boardIdConter,email);
            boardIdConter++;
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>void </returns>
        internal void DeleteBoard(string email, string name)
        {
            if (!authenticator.IsConect(email)) { throw new Exception("User not conected"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email,name);
            if (id == -1) { throw new Exception($"User dosen't have a {name} board"); }
            board[id].DeleteBoard(email);
            board.Remove(id);
            foreach (string k in userToBoard.Keys)
            {
                if (userToBoard[k].Contains(id)) { userToBoard[k].Remove(id); }
            }

        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="name">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>void </returns>
        internal void ChangeLimit(string email, string name, int col, int limit)
        {
            if (!authenticator.IsConect(email)) { throw new Exception("User not conected"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, name);
            if (id == -1) { throw new Exception($"User dosen't have a {name} board"); }

            board[id].ChangeLimit(col, limit);
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="name">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>void </returns>
        internal void MoveTask(string email, string name, int col, int taskId)
        {
            if (!authenticator.IsConect(email)) { throw new Exception("User not conected"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, name);
            if (id == -1) { throw new Exception($"User dosen't have a {name} board"); }

            board[id].MoveTask(email,col, taskId);
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="name">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="descrition">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>void </returns>
        internal void CreateTask(string email, string name, string title, string descrition, DateTime dueDate)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, name);
            if (id == -1) { throw new Exception($"User dosen't have a {name} board"); }

            board[id].CreatTask(taskIdCuonter, title, descrition, dueDate);
            taskIdCuonter++;
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newTitle">New title for the task</param>
        /// <returns>void </returns>
        internal void EditTaskTitle(string email, string boardName, int taskId, int col, string newTitle)   
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (taskId < 0 || taskId > taskIdCuonter) { throw new Exception("Invaled task Id"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            
            int id = GetBoardId(email, boardName);
            if (id == -1) { throw new Exception($"User dosen't have a {boardName} board"); }

            board[id].EditsTaskTitel(taskId, col, newTitle);
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newDescription">New description for the task</param>
        /// <returns>void </returns>
        internal void EditTaskDescriptiion(string email, string boardName, int taskId, int col, string newDescription)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (taskId < 0 || taskId > taskIdCuonter) { throw new Exception("Invaled task Id"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, boardName);
            if (id == -1) { throw new Exception($"User dosen't have a {boardName} board"); }


            board[id].EditsTaskDescription(taskId, col, newDescription);
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>void </returns>
        internal void EditTaskDueDate(string email, string boardName, int taskId, int col, DateTime dueDate)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (taskId < 0 || taskId > taskIdCuonter) { throw new Exception("Invaled task Id"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, boardName);
            if (id == -1) { throw new Exception($"User dosen't have a {boardName} board"); }


            board[id].EditsTaskDueDate(taskId, col, dueDate);

        }

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A list of the in-progress tasks of the user</returns>
        internal LinkedList<TaskBL> Inprogress(string email)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            LinkedList<TaskBL> ret = new();
            LinkedList<int> boards = userToBoard[email];
            foreach (int boardId in boards)
            {
                LinkedList<TaskBL> colume = board[boardId].GetInProgres(email);
                foreach (TaskBL task in colume)
                {
                    ret.AddLast(new TaskBL(task));
                }
            }
            return ret;
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A list of the column's tasks</returns>
        internal LinkedList<TaskBL> GetColumn(string email, string boardName, int col)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, boardName);
            if (id == -1) { throw new Exception($"User dosen't have a {boardName} board"); }

            return board[id].GetColumn(col);
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The column's limit</returns>
        internal int GetColumnLimit(string email, string boardName, int col)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, boardName);
            if (id == -1) { throw new Exception($"User dosen't have a {boardName} board"); }

            return board[id].GetLimit(col);
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The column's name</returns>
        internal string GetColumnName(string email, string boardName, int col)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, boardName);
            if (id == -1) { throw new Exception($"User dosen't have a {boardName} board"); }

            return col switch
            {
                0 => "backlog",
                1 => "in progress",
                2 => "done",
                _ => throw new Exception($"There is no {col} column"),
            };
        }

        internal LinkedList<int> GetUserBoards(string email)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            return userToBoard[email];
        }

        /// <summary>		 
        /// This method adds a user as member to an existing board.		 
        /// </summary>		 
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        internal void JoinBoard(string email, int boardID)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            string boardName = board[boardID].Name;
            //int id = GetBoardId(email, boardName);
            if (GetBoardId(email, boardName) != -1) { throw new Exception($"User have a {boardName} board and canot conect to another"); }
            board[boardID].JoinBoard(email);
            userToBoard[email].AddFirst(boardID);
        }

        /// <summary>		 
        /// This method removes a user from the members list of a board.		 
        /// </summary>		 
        /// <param name="email">The email of the user. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        internal void LeaveBoard(string email, int boardID)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            if (!userToBoard[email].Contains(boardID)) { throw new Exception($"User {email} is not a member in board {boardID}"); }
            board[boardID].LeaveBoard(email);
            userToBoard[email].Remove(boardID);
        }

        /// <summary>		 
        /// This method returns a board's name		 
        /// </summary>		 
        /// <param name="boardId">The board's ID</param>		 
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        internal string GetBoardName(int boardId)
        {
            if(!board.ContainsKey(boardId)) { throw new Exception($"Ther is no board with id {boardId}"); }
            return board[boardId].Name;
        }

        /// <summary>		 
        /// This method transfers a board ownership.		 
        /// </summary>		 
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>		 
        /// <param name="newOwnerEmail">Email of the new owner</param>		 
        /// <param name="boardName">The name of the board</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            if (!authenticator.IsConect(currentOwnerEmail)) { throw new Exception($"User {currentOwnerEmail} not coneted"); }
            if (!userToBoard.ContainsKey(currentOwnerEmail)) { userToBoard[currentOwnerEmail] = new LinkedList<int>(); }
            int id = GetBoardId(currentOwnerEmail, boardName);
            if (id == -1) { throw new Exception($"User dosen't have a {boardName} board"); }

            board[id].TransferOwnership(currentOwnerEmail, newOwnerEmail);
        }

        /// <summary>		 
        /// This method assigns a task to a user		 
        /// </summary>		 
        /// <param name="email">Email of the user. Must be logged in</param>		 
        /// <param name="boardName">The name of the board</param>		 
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>		 
        /// <param name="taskID">The task to be updated identified a task ID</param>        		 
        /// <param name="emailAssignee">Email of the asignee user</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        internal void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            if (!authenticator.IsConect(email)) { throw new Exception($"User {email} not coneted"); }
            if (!userToBoard.ContainsKey(email)) { userToBoard[email] = new LinkedList<int>(); }
            int id = GetBoardId(email, boardName);
            if (id == -1) { throw new Exception($"User dosen't have a {boardName} board"); }

            board[id].AssignTask(columnOrdinal, taskID, emailAssignee);
        }
        internal void LoadData()
        {
            bc.GetAllBoards().ForEach(b => 
            {
                board.Add(b.BoardID, new BoardBL(b));
                boardIdConter = Math.Max(boardIdConter, b.BoardID); 
            });
            boardIdConter++;
            buc.GetBoardByUser().ForEach(m =>
            {
                if (!userToBoard.ContainsKey(m.email))
                {
                    userToBoard.Add(m.email, new LinkedList<int>());
                }
                userToBoard[m.email].AddFirst(m.id);
            });
        }



        private int GetBoardId(string email, string boardName)
        {
            foreach (int id in userToBoard[email])
            {
                if (board[id].Name == boardName)return id;
            }
            return -1;
        }

        internal void DeleteData()
        {
            for (int i = 0; i < boardIdConter; i++)
            {
                if (board.ContainsKey(boardIdConter))
                {
                    board[i].Claer();
                    break;
                }
            }
            if (!buc.Claer() || !bc.Claer() )
            {
                throw new Exception("Faild to clear Data");
            }
            board.Clear();
            userToBoard.Clear();
        }

    }
}
