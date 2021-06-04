using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Database
{
    public static class DataBaseAccess
    {
        public static async void InitializeDatabase()
        {
            String dbpath = @"C:\Users\Judicator\Documents\MonkeyB\MonkeyB\database.db";
            await Task.Run(() =>
            {
                if (!System.IO.File.Exists(dbpath))
                {
                    System.IO.File.Create(dbpath);

                }
                using (SqliteConnection db =
                    new SqliteConnection($"Filename={dbpath}"))
                {
                    
                    db.Open();

                    string tableCommand =
                        "CREATE TABLE IF NOT EXISTS Users " +
                        "(ID INTEGER NOT NULL UNIQUE, " +
                        "username TEXT NOT NULL, " +
                        "password TEXT NOT NULL, " +
                        "PRIMARY KEY(ID AUTOINCREMENT))";

                    SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                    createTable.ExecuteReader();
                }
            });
        }
    }
}
