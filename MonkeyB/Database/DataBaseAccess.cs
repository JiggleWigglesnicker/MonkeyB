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

        private static string FolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string DbPath { get; set; } = System.IO.Path.Combine(FolderPath, "database.db");
       

        public static async void InitializeDatabase()
        {
            await Task.Run(() =>
            {
                using (var db = new SqliteConnection($"Data Source=database.db"))
                {
                    System.Diagnostics.Debug.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\n");

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
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(cryptowalletID AUTOINCREMENT))";

                    string tableCommand3 =
                    "CREATE TABLE IF NOT EXISTS Wallet " +
                    "(walletID INTEGER NOT NULL UNIQUE, " +
                    "currency TEXT NOT NULL, " +
                    "currency_amount FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(walletID AUTOINCREMENT))";

                    string tableCommand4 =
                    "CREATE TABLE IF NOT EXISTS Transactions " +
                    "(ID INTEGER NOT NULL UNIQUE, " +
                    "currency_name STRING NOT NULL, " +
                    "currency_amount FLOadAT NOT NULL, " +
                    "currency_value FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID))";

                    string adminCommand = "INSERT OR IGNORE INTO Users (username,password) VALUES ('admin','admin')";
                    string euroCommand = "INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES ('eur', 1000, 1)";
                    string bitcoinCommand = "INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES ('bitcoin', 0, 1)";
                    string dogeCommand = "INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES ('dogecoin', 0, 1)";




                    SqliteCommand createTable1  = new SqliteCommand(tableCommand1, db);
                    SqliteCommand createTable2  = new SqliteCommand(tableCommand2, db);
                    SqliteCommand createTable3  = new SqliteCommand(tableCommand3, db);
                    SqliteCommand createTable4  = new SqliteCommand(tableCommand4, db);
                    SqliteCommand createAdmin   = new SqliteCommand(adminCommand, db);
                    SqliteCommand addBitcoin    = new SqliteCommand(bitcoinCommand, db);
                    SqliteCommand addEur        = new SqliteCommand(euroCommand, db);
                    SqliteCommand addDoge       = new SqliteCommand(dogeCommand, db);




                    createTable1.ExecuteReader();
                    createTable2.ExecuteReader();
                    createTable3.ExecuteReader();
                    createTable4.ExecuteReader();
                    createAdmin.ExecuteReader();
                    addBitcoin.ExecuteReader();
                    addEur.ExecuteReader();
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
                    App.UserID = query.GetInt32(2);
                }
            }

            return model;
        }

        internal static float checkWalletAmount(string v)
        {
            throw new NotImplementedException();
        }

        public static void SellCoin(string currency, float amount, int userID)
        {
            using (var db = new SqliteConnection($"Data Source={DbPath}"))
            {
                db.Open();

                SqliteCommand updateCommand;
                updateCommand = new SqliteCommand($"UPDATE CryptoWallet SET coin_amount= coin_amount - '{amount}'   WHERE userID = '{userID}' AND coin = '{currency}'", db);
                updateCommand.ExecuteNonQuery();
            }

        }

        public static void BuyCoin(string currency, float amount, int userID)
        {
            using (var db = new SqliteConnection($"Data Source={DbPath}"))
            {
                db.Open();

                SqliteCommand updateCommand;
                updateCommand = new SqliteCommand($"UPDATE CryptoWallet SET coin_amount = coin_amount + '{amount}'   WHERE userID = '{userID}' AND coin = '{currency}'", db);
                updateCommand.ExecuteNonQuery();
            }

        }

        public static float GetCoinAmount(string currency, int userID)
        {
            using (var db = new SqliteConnection($"Data Source={DbPath}"))
            {
                db.Open();

                SqliteCommand selectCommand;
                selectCommand = new SqliteCommand($"SELECT coin_amount from Cryptowallet WHERE userID = '{userID}' AND coin = '{currency}'", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                //addCurrency("bitcoin", App.UserID);
                float amount = 0;

                

                while (query.Read())
                {
                    amount = query.GetFloat(0);
                }

                return amount;
            }
        }

        private static void addCurrency(string currency, int userID)
        {
            using (var db = new SqliteConnection($"Data Source={DbPath}"))
            {
                db.Open();

                SqliteCommand selectCommand;
                selectCommand = new SqliteCommand($"INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES ('bitcoin', 20, 1)", db);
                SqliteDataReader query = selectCommand.ExecuteReader();


            }
        }

    }
}
