using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardController
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private const string TableName = "Boards";

        internal BoardController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }
        internal List<BoardDAO> GetAllBoards()
        {
            List<BoardDAO> results = new List<BoardDAO>();
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
                        results.Add(new BoardDAO(dataReader));
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
        internal bool Insert(BoardDAO board) 
        {
            int res = -1;

            using (var connection = new SQLiteConnection(this._connectionString))
            {
                try
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    string insert = $"INSERT INTO {TableName} ({board.IdCol}, {board.NameCol}, {board.OwnerCol}, {board.backlogCol}, {board.inProgessCol}, {board.doneCol}) " +
                        $"Values (@idVal, @NameVal, @ownerVal, @backlogVal, @inProgressVal, @doneVal)";



                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.BoardID);
                    SQLiteParameter nameParam = new SQLiteParameter(@"NameVal", board.Name);
                    SQLiteParameter ownerParam = new SQLiteParameter(@"ownerVal", board.Owner);
                    SQLiteParameter backlogParam = new SQLiteParameter(@"backlogVal", board.BacklogLimit);
                    SQLiteParameter inProdessParam = new SQLiteParameter(@"inProgressVal", board.InProgressLimit);
                    SQLiteParameter doneParam = new SQLiteParameter(@"doneVal", board.DoneLimite);
                    command.CommandText = insert;
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(ownerParam);
                    command.Parameters.Add(backlogParam);
                    command.Parameters.Add(inProdessParam);
                    command.Parameters.Add(doneParam);


                    connection.Open();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex )
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
            return res > 0;
        }
        internal bool Delete(int id)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE id=@idVal"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@idVal", id));
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
                    CommandText = $"DELETE FROM {_tableName} "
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
