using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BoardFacade _bf;
        internal TaskService(BoardFacade bf)
        {
            this._bf = bf;
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string CreateTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                _bf.CreateTask(email, boardName, title, description, dueDate);
                Response ret = new(null,null);
                log.Info($"user {email} task has succfully created");
                return ret.GetSerilizeResponse();

            }
            catch(Exception ex)
            {
                Response ret = new(null,ex.Message);
                log.Warn($"user {email} has faild to create task : {ex.Message}");
                return ret.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardname">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="id">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string EditTaskTitle(string email, string boardname,int id,int col, string title)
        {
            try
            {
                _bf.EditTaskTitle(email, boardname, id,col, title);
                Response ret = new(null,null);
                log.Info($"user {email} task has succfully edited title");
                return ret.GetSerilizeResponse();
            } catch(Exception ex)
            {
                Response ret = new(null,ex.Message);
                log.Warn($"user {email} has faild to edit task title : {ex.Message}");
                return ret.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardname">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="id">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string EditTaskDescription(string email, string boardname,int id,int col ,string description)
        {
            try
            {
                _bf.EditTaskDescriptiion(email, boardname, id,col, description);
                Response ret = new(null, null);
                log.Info($"user {email} task has succfully edited description");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response ret = new(null, ex.Message);
                log.Warn($"user {email} has faild to edit task description : {ex.Message}");
                return ret.GetSerilizeResponse();
            }
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardname">The name of the board</param>
        /// <param name="col">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="id">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string EditTaskDueDate(string email, string boardname,int id,int col, DateTime dueDate)
        {
            try
            {
                _bf.EditTaskDueDate(email, boardname, id,col, dueDate);
                Response ret = new(null, null);
                log.Info($"user {email} task has succfully edited duedate");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response ret = new(null, ex.Message);
                log.Warn($"user {email} has faild to edit task duedate : {ex.Message}");
                return ret.GetSerilizeResponse();
            }
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
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                _bf.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                Response ret = new(null, null);
                log.Info($"User {email} has succfully bind asigned to the task");
                return ret.GetSerilizeResponse();
            }
            catch (Exception ex)
            {
                Response response = new(null, ex.Message);
                log.Warn($"User {email} failed to asigned to the task: {ex.Message}");
                return response.GetSerilizeResponse();
            }
        }
    }
}
