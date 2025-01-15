using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserController
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName = "User";

        internal UserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        internal List<UserDAO> GetAllUsers()
        {
            List<UserDAO> results = new();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(new UserDAO(dataReader));
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

        internal bool Insert(UserDAO user){
            int res = -1;

            using (var connection = new SQLiteConnection(this._connectionString))
            {
                try
                {
                    SQLiteCommand command = new(null, connection);
                    string insert = $"INSERT INTO {TableName} ({user.UserEmailColumnName}, {user.UserPassColumnName}) " +
                        $"Values (@emailVal, @passwordVal)";



                    SQLiteParameter emailParam = new(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new(@"passwordVal", user.Password);
                    command.CommandText = insert;
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);

                    connection.Open();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    //connection.Close();

                }
                return res >= 0;
            }
        }


        internal bool Claer()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} "
                };
                try
                {
                    
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


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
