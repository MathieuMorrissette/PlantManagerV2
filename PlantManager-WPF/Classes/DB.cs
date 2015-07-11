using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Windows;

namespace PlantManager_WPF
{
    public class Db
    {
        private const string ConnectionString = "Data Source=plantmanager.sqlite;Version=3;foreign keys=true;";
        private static SQLiteConnection _dbConnection;

        public static void EnsureConnected()
        {
            if (_dbConnection == null)
                Connect();
        }

        private static void Connect()
        {
            _dbConnection = new SQLiteConnection(ConnectionString);
            _dbConnection.Open();
        }

        public static DataTable Query(string request, params string[] parameters)
        {
            EnsureConnected();

            DataTable dTable = new DataTable();

            using (DbTransaction trans = _dbConnection.BeginTransaction())
            {
                using (DbCommand command = _dbConnection.CreateCommand())
                {
                    command.CommandText = request;
                    foreach (string parameter in parameters)
                    {
                        command.Parameters.Add(new SQLiteParameter("", parameter));
                    }

                    DbDataReader reader = command.ExecuteReader();
                    dTable.Load(reader);
                }

                trans.Commit();
            }

            return dTable;
        }

        public static DataRow QueryFirst(string request, params string[] parameters)
        {
            EnsureConnected();

            DataTable dTable = null;

            using (DbTransaction trans = _dbConnection.BeginTransaction())
            {
                using (DbCommand command = _dbConnection.CreateCommand())
                {
                    command.CommandText = request;
                    foreach (string parameter in parameters)
                    {
                        command.Parameters.Add(new SQLiteParameter("", parameter));
                    }

                    var reader = command.ExecuteReader(CommandBehavior.SingleRow);
                    dTable = new DataTable();
                    dTable.Load(reader);
                }

                trans.Commit();
            }

            if (dTable == null)
            {
                return null;
            }

            if (dTable.Rows.Count == 0)
            {
                return null;
            }

            return dTable.Rows[0];
        }

        /* public static void CreateDatabase()
        {
            SQLiteConnection.CreateFile("plantmanager.sqlite");
        }*/

        public static void Execute(string request, params string[] parameters)
        {
            EnsureConnected();
            SQLiteCommand command = new SQLiteCommand(_dbConnection);
            command.CommandText = request;

            foreach (string parameter in parameters)
            {
                command.Parameters.Add(new SQLiteParameter("", parameter));
            }

            //_dbConnection.Open();

            command.ExecuteNonQuery();

           // _dbConnection.Close();
        }
    }
}