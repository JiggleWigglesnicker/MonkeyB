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
            String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            String dbpath = System.IO.Path.Combine(folderPath, "database.db");
            await Task.Run(() =>
            {
                using (var db = new SqliteConnection($"Data Source={dbpath}"))
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
