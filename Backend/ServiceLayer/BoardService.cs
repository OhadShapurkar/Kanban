using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BoardFacade _bf;

        internal BoardService(BoardFacade bf)
        {
            _bf = bf; 
        }

        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string CreateBoard(string email, string boardName)
        {
            try
            {
                _bf.CreateBoard(email, boardName);
                Response ret = new(null, null);
                log.Info($"User {email} created board{boardName} successfuly");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                log.Warn($"User {email} failed to create board {boardName}: {ex.Message}");
                Response ret = new(null, ex.Message);
                return ret.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string DeleteBoard(string email, string boardName) 
        {
            try
            {
                _bf.DeleteBoard(email, boardName);
                Response ret = new(null, null);
                log.Info($"User {email} deleted board {boardName} succesfully");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to delete board {boardName}: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardname">The name of the board</param>
        /// <param name="coulmnOrdina">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newLimit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string ChangeLimit(string email, string boardname, int coulmnOrdina, int newLimit)
        {
            try
            {
                _bf.ChangeLimit(email, boardname, coulmnOrdina, newLimit);
                Response ret = new(null, null);
                log.Info($"User {email} changed limit of column {coulmnOrdina} to {newLimit} in {boardname} successfully");
                return ret.GetSerilizeResponse();
            }
            catch(Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to changed limit of column {coulmnOrdina} to {newLimit} in {boardname}: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs</returns>
        public string InProgressList(string email)
        {
            try
            {
                LinkedList<TaskBL> taskBLs = _bf.Inprogress(email);
                LinkedList<Task> tasks = new();
                foreach (TaskBL taskBL in taskBLs)
                {
                    tasks.AddLast(new Task(taskBL));
                }
                Response ret = new(tasks, null);
                log.Info($"User {email} recieved progress list successfully");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to recived progress list: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string MoveTask(string email, string boardName,int col, int taskId)
        {
            try
            {
                _bf.MoveTask(email, boardName,col,taskId);
                Response ret = new(null, null);
                log.Info($"User {email} has moved task {taskId} in coulmn {col} in {boardName} successfully");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to moved task {taskId} in coulmn {col} in {boardName}: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs </returns>
        public string GetColumn(string email, string boardName,int col) {
            try
            {
                LinkedList<TaskBL> taskBLs = _bf.GetColumn(email, boardName, col);
                LinkedList<Task> tasks = new();
                foreach(TaskBL taskBL in taskBLs)
                {
                    tasks.AddLast(new Task(taskBL));
                }

                Response ret = new(tasks, null);
                log.Info($"User {email} recieved coulmn {col} in {boardName} successfully");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to recieved coulmn {col} in {boardName}: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs</returns>
        public string GetColumnLimit(string email, string boardName, int col)
        {
            try
            {
                int limit = _bf.GetColumnLimit(email, boardName, col);
                Response ret = new(limit, null);
               log.Info($"User {email} recieved coulmn limit of {col} in {boardName} successfully");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to recieved coulmn limit of {col} in {boardName}: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs </returns>
        public string GetColumnName(string email, string boardName, int col) {
            try
            {
                string name = _bf.GetColumnName(email, boardName, col);
                Response ret = new(name, null);
                log.Info($"User {email} has succfully recived a colunm num: {col} name from the board {boardName}");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to recived the colunm name : {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }


        public string GetUserBoards(string email)
        {
            try
            {
                LinkedList<int> list = _bf.GetUserBoards(email);
                Response ret = new(list, null);
                log.Info($"User {email} has succfully recived a list of it's boards ids");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to recived the boards id : {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }


        /// <summary>		 
        /// This method adds a user as member to an existing board.		 
        /// </summary>		 
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string JoinBoard(string email, int boardID)
        {
            try
            {
                _bf.JoinBoard(email,boardID);
                Response ret = new(null, null);
                log.Info($"User {email} has succfully join the board {boardID}");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to join the board {boardID}: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }

        /// <summary>		 
        /// This method removes a user from the members list of a board.		 
        /// </summary>		 
        /// <param name="email">The email of the user. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                _bf.LeaveBoard(email, boardID);
                Response ret = new(null, null);
                log.Info($"User {email} has succfully left the board {boardID}");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to left the board {boardID}: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }


        /// <summary>		 
        /// This method returns a board's name		 
        /// </summary>		 
        /// <param name="boardId">The board's ID</param>		 
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string GetBoardName(int boardId)
        {
            try
            {
                string name = _bf.GetBoardName(boardId);
                Response ret = new(name, null);
                log.Info($"User has succfully recived a board name: {name}");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User failed to recived the board name : {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }

        /// <summary>		 
        /// This method transfers a board ownership.		 
        /// </summary>		 
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>		 
        /// <param name="newOwnerEmail">Email of the new owner</param>		 
        /// <param name="boardName">The name of the board</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                _bf.TransferOwnership(currentOwnerEmail,newOwnerEmail,boardName);
                Response ret = new(null, null);
                log.Info($"User {newOwnerEmail} is the new owner of {boardName} board");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {newOwnerEmail} failed to become the new board owner : {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }	 
    }
}
