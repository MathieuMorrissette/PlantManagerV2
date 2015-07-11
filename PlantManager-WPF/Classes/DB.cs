using System;
using System.Data;
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
        }

        public static DataTable Query(string request, params string[] parameters)
        {
            EnsureConnected();

            SQLiteCommand command = new SQLiteCommand(_dbConnection);
            command.CommandText = request;

            foreach (string parameter in parameters)
            {
                command.Parameters.Add(new SQLiteParameter("", parameter));
            }

            _dbConnection.Open();
            SQLiteDataReader reader = command.ExecuteReader();

            DataTable dTable = new DataTable();
            dTable.Load(reader);
            _dbConnection.Close();
            return dTable;
        }

        public static DataRow QueryFirst(string request, params string[] parameters)
        {
            EnsureConnected();
            SQLiteCommand command = new SQLiteCommand(_dbConnection);
            command.CommandText = request;

            foreach (string parameter in parameters)
            {
                command.Parameters.Add(new SQLiteParameter("", parameter));
            }

            try
            {
                _dbConnection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SQLiteDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow);
            DataTable dTable = new DataTable();
            dTable.Load(reader);
            _dbConnection.Close();

            return dTable.Rows.Count == 0 ? null : dTable.Rows[0];
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

            _dbConnection.Open();

            command.ExecuteNonQuery();

            _dbConnection.Close();
        }
    }
}