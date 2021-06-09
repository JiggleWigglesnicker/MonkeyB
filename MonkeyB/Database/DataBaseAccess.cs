using Microsoft.Data.Sqlite;
using MonkeyB.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            String fullPath = System.IO.Path.Combine(folderPath, "MonkeyB");
            if(!Directory.Exists(fullPath)) 
                Directory.CreateDirectory(fullPath);
            String dbpath = System.IO.Path.Combine(fullPath, "database.db");
            await Task.Run(() =>
            {
                using (var db = new SqliteConnection($"Data Source={dbpath}"))
                {

                    db.Open();


                    string tableCommand1 =
                    "CREATE TABLE IF NOT EXISTS Users " +
                    "(userID INTEGER NOT NULL UNIQUE, " +
                    "username TEXT NOT NULL UNIQUE, " +
                    "password TEXT NOT NULL, " +
                    "PRIMARY KEY(userID AUTOINCREMENT))";

                    string tableCommand2 =
                    "CREATE TABLE IF NOT EXISTS Cryptowallet " +
                    "(cryptowalletID INTEGER NOT NULL UNIQUE, " +
                    "coin TEXT NOT NULL, " +
                    "coin_amount FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID),"+
                    "PRIMARY KEY(cryptowalletID AUTOINCREMENT))";

                    string tableCommand3 =
                    "CREATE TABLE IF NOT EXISTS Wallet " +
                    "(walletID INTEGER NOT NULL UNIQUE, " +
                    "currency TEXT NOT NULL, " +
                    "currency_amount FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(walletID AUTOINCREMENT))";

                    string adminCommand = "INSERT INTO Users (username,password) VALUES ('admin','admin')";


                    SqliteCommand createTable1 = new SqliteCommand(tableCommand1, db);
                    SqliteCommand createTable2 = new SqliteCommand(tableCommand2, db);
                    SqliteCommand createTable3 = new SqliteCommand(tableCommand3, db);
                    SqliteCommand createAdmin = new SqliteCommand(adminCommand, db);

                    createTable1.ExecuteReader();
                    createTable2.ExecuteReader();
                    createTable3.ExecuteReader();
                    createAdmin.ExecuteReader();
                }
            });
        }


        public static LoginModel RetrieveLogin(String username)
        {
            String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            String dbpath = System.IO.Path.Combine(folderPath, "database.db");
            LoginModel model = new LoginModel();

            using (var db = new SqliteConnection($"Data Source={dbpath}"))
            {

                db.Open();

                SqliteCommand selectCommand;
                selectCommand = new SqliteCommand
                    ($"SELECT username, password from Users WHERE username = '{username}'", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    model.username = query.GetString(0);
                    model.password = query.GetString(1);
                }
            }

            return model;
        }

    }
}
