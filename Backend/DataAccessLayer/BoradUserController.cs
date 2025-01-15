using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoradUserController
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName = "UserToBoards";

        internal BoradUserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        internal List<string> GetAllMembers(int BoardID)
        {
            List<string> results = new List<string>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT Email FROM {_tableName} WHERE BoardId = {BoardID};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(dataReader.GetString(0));
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

        internal List<members> GetBoardByUser()
        {
            List<members> results = new List<members>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(new members((int)dataReader.GetValue(0), dataReader.GetString(1)));
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

        internal bool Insert(int BoardID,string emial)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this._connectionString))
            {
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                     
                    string insert = $"INSERT INTO {TableName} (Email, BoardId) " +
                        $"Values (@emailVal, @boardIdVal)";


                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", emial);
                    SQLiteParameter idParam = new SQLiteParameter(@"BoardIdVal", BoardID);
                    
                    command.CommandText = insert;
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(idParam);

                    connection.Open();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception )
                {

                }
                finally
                {
                    //connection.Close();

                }
                return res >= 0;
            }
        }

        internal bool Delete(int BoardID, string emial)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE BoardId=@idVal AND Email=@emailVal"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@idVal", BoardID));
                    command.Parameters.Add(new SQLiteParameter("@emailVal", emial));
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

        internal bool Delete(int BoardID) 
        { 
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
                    command.Parameters.Add(new SQLiteParameter("@idVal", BoardID));
                    
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
