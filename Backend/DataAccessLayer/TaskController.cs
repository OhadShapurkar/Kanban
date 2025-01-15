using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskController
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName = "Tasks";
        //TODO add date
        internal TaskController() {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        internal List<TaskDAO> GetAllTask(int BoardID)
        {
            List<TaskDAO> results = new List<TaskDAO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {_tableName} WHERE BoardId = {BoardID};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(new TaskDAO(dataReader));
                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    //connection.Close();
                }

            }
            return results;
        }

        internal bool Insert(TaskDAO Task)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this._connectionString))
            {
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string insert = $"INSERT INTO {TableName} ({Task.idCol}, {Task.BoardIdCol}, {Task.TitleCol}, {Task.StatusCol}, {Task.DescriptionCol}, {Task.AsingToCol}, DueDate,CreationDate) " +
                        $"VALUES (@idVal, @boardIdVal, @titleVal, @statusVal, @descripVal, @asignVal, @dueVal, @creatVal)";



                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", Task.Id);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", Task.BoardId);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", Task.Title);
                    SQLiteParameter statusParam = new SQLiteParameter(@"statusVal", Task.Status);
                    SQLiteParameter discripParam = new SQLiteParameter(@"descripVal", Task.Description);
                    SQLiteParameter asignParam = new SQLiteParameter(@"asignVal", Task.AsignTo);
                    SQLiteParameter dueParam = new SQLiteParameter(@"dueVal", Task.DueDate.ToString());
                    SQLiteParameter creatParam = new SQLiteParameter(@"creatVal", Task.CreationDate.ToString());
                    command.CommandText = insert;
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(statusParam);
                    command.Parameters.Add(discripParam);
                    command.Parameters.Add(asignParam);
                    command.Parameters.Add(dueParam);
                    command.Parameters.Add(creatParam);


                    connection.Open();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("filed to insert to the data base");
                }
                finally
                {
                    //connection.Close();

                }
                return res >= 0;
            }
        }

        internal bool Update(int id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@Val WHERE id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@Val", attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    //connection.Close();

                }
            }
            return res >= 0;
        }

        internal bool Update(int id, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@Val WHERE id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@Val", attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    //connection.Close();

                }
            }
            return res >= 0;
        }

        internal bool Delete(int boardId) {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE BoardId=@idVal"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@idVal", boardId));

                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    //connection.Close();

                }
            }
            return res >= 0;
        }

        internal bool Claer()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName}"
                };
                try
                {

                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    //connection.Close();

                }
            }
            return res >= 0;
        }


    }
}
