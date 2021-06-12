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
            await Task.Run(() =>
            {
                using (var db = new SqliteConnection($"Data Source=database.db"))
                {

                    db.Open();


                    string tableCommand1 =
                    "CREATE TABLE IF NOT EXISTS Users " +
                    "(userID INTEGER NOT NULL UNIQUE, " +
                    "username TEXT NOT NULL UNIQUE, " +
                    "password TEXT NOT NULL, " +
                    "euro_amount FLOAT NOT NULL, " +
                    "PRIMARY KEY(userID AUTOINCREMENT))";

                    string tableCommand2 =
                    "CREATE TABLE IF NOT EXISTS Cryptowallet " +
                    "(cryptowalletID INTEGER NOT NULL UNIQUE, " +
                    "coin TEXT NOT NULL, " +
                    "coin_amount FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(cryptowalletID AUTOINCREMENT))";

                    string tableCommand3 =
                    "CREATE TABLE IF NOT EXISTS Orders " +
                    "(orderID INTEGER NOT NULL UNIQUE, " +
                    "cointype TEXT NOT NULL, " +
                    "coin_amount FLOAT NOT NULL, " +
                    "euro_amount FLOAT NOT NULL, " +
                    "outstanding BOOLEAN NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(orderID AUTOINCREMENT))";


                    string adminCommand = "INSERT OR IGNORE INTO Users (username,password,euro_amount) VALUES ('admin','admin',1000)";

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
            LoginModel model = new LoginModel();

            using (var db = new SqliteConnection($"Data Source=database.db"))
            {

                db.Open();

                SqliteCommand selectCommand;
                selectCommand = new SqliteCommand
                    ($"SELECT username, password, userID from Users WHERE username = '{username}'", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    model.username = query.GetString(0);
                    model.password = query.GetString(1);
                    App.UserID = query.GetString(2);
                }
            }

            return model;
        }

        public static void updateEuroAmount(float amount)
        {

            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand updateCommand;
                updateCommand = new SqliteCommand
                    ($"UPDATE Users SET euro_amount = {amount} WHERE userID = '{App.UserID}'", db);
                updateCommand.ExecuteReader();

            }


        }

    }
}
