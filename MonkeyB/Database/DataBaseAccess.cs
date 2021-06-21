using Microsoft.Data.Sqlite;
using MonkeyB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    "euro_amount FLOAT NOT NULL DEFAULT 1000, " +
                    "PRIMARY KEY(userID AUTOINCREMENT))";

                    string tableCommand2 =
                    "CREATE TABLE IF NOT EXISTS Cryptowallet " +
                    "(cryptowalletID INTEGER NOT NULL UNIQUE, " +
                    "coin TEXT NOT NULL UNIQUE, " +
                    "coin_amount  FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(cryptowalletID AUTOINCREMENT))";

                    string tableCommand3 =
                    "CREATE TABLE IF NOT EXISTS Orders " +
                    "(orderID INTEGER NOT NULL UNIQUE, " +
                    "cointype TEXT NOT NULL, " +
                    "coin_amount  FLOAT NOT NULL, " +
                    "euro_amount  FLOAT NOT NULL, " +
                    "outstanding BOOLEAN NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(orderID AUTOINCREMENT))";

                    string tableCommand4 =
                    "CREATE TABLE IF NOT EXISTS Transactions " +
                    "(ID INTEGER NOT NULL UNIQUE, " +
                    "currency_name STRING NOT NULL, " +
                    "currency_amount FLOadAT NOT NULL, " +
                    "currency_value FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID))";

                    string adminCommand = "INSERT OR IGNORE INTO Users (username,password) VALUES ('admin','admin')";


                    SqliteCommand createTable1 = new SqliteCommand(tableCommand1, db);
                    SqliteCommand createTable2 = new SqliteCommand(tableCommand2, db);
                    SqliteCommand createTable3 = new SqliteCommand(tableCommand3, db);
                    SqliteCommand createTable4 = new SqliteCommand(tableCommand4, db);
                    SqliteCommand createAdmin = new SqliteCommand(adminCommand, db);




                    createTable1.ExecuteReader();
                    createTable2.ExecuteReader();
                    createTable3.ExecuteReader();
                    createTable4.ExecuteReader();
                    createAdmin.ExecuteReader();
                }
            });
        }

        public static void InitializeCoins()
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand bitcoinCommand = new SqliteCommand($"INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES('bitcoin', 1000,{App.UserID})", db);
                SqliteCommand dogecoinCommand = new SqliteCommand($"INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES('dogecoin', 1000,{App.UserID})", db);
                SqliteCommand litcoinCommand = new SqliteCommand($"INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES('litecoin', 1000,{App.UserID})", db);

                bitcoinCommand.ExecuteReader();
                dogecoinCommand.ExecuteReader();
                litcoinCommand.ExecuteReader();
            }
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
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand updateCommand;
                updateCommand = new SqliteCommand($"UPDATE CryptoWallet SET coin_amount= coin_amount - '{amount}'   WHERE userID = '{userID}' AND coin = '{currency}'", db);
                updateCommand.ExecuteNonQuery();
            }

        }

        public static void BuyCoin(string currency, float amount, int userID)
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand updateCommand;
                updateCommand = new SqliteCommand($"UPDATE CryptoWallet SET coin_amount = coin_amount + '{amount}'   WHERE userID = '{userID}' AND coin = '{currency}'", db);
                updateCommand.ExecuteNonQuery();
            }

        }

        public static void BuyEuro(float amount)
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand updateCommand;
                updateCommand = new SqliteCommand($"UPDATE users SET euro_amount = euro_amount + '{amount}'   WHERE userID = '{App.UserID}'", db);
                updateCommand.ExecuteNonQuery();
            }

        }


        public static void SellEuro(float amount)
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand updateCommand;
                updateCommand = new SqliteCommand($"UPDATE users SET euro_amount = euro_amount - '{amount}'   WHERE userID = '{App.UserID}'", db);
                updateCommand.ExecuteNonQuery();
            }

        }

        public static float GetCoinAmount(string currency, int userID)
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
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
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand selectCommand;
                selectCommand = new SqliteCommand($"INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES ('{currency}', 0, {userID})", db);
                SqliteDataReader query = selectCommand.ExecuteReader();


            }
        }

        public static bool RegisterUser(string username, string password)
        {
            string registerCommand = ($"INSERT OR IGNORE INTO Users (username,password, euro_amount) VALUES ('{username}','{password}',1000)");
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();
                SqliteCommand selectCommand;
                selectCommand = new SqliteCommand
                    ($"SELECT username, password, userID from Users WHERE username = '{username}'", db);
                var result = selectCommand.ExecuteReader();
                if (result.HasRows || username == String.Empty || password == String.Empty) return false;
                SqliteCommand createTable1 = new SqliteCommand(registerCommand, db);
                createTable1.ExecuteReader();
                return true;
            }
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


        /// <summary>
        /// Returns the amount of euro's a user has
        /// </summary>
        /// <returns>Euro amount</returns>
        public static float GetEuroAmount()
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand selectCommand;
                selectCommand = new SqliteCommand($"SELECT euro_amount from users WHERE userID = '{App.UserID}' ", db);
                SqliteDataReader query = selectCommand.ExecuteReader();


                float amount = 0;
                while (query.Read())
                {
                    amount = query.GetFloat(0);
                }

                return amount;
            }
        }


        public static List<CryptoWalletModel> FetchCoinsInWallet(int id)
        {
            List<CryptoWalletModel> coinList = new List<CryptoWalletModel>();
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand selectCommand;

                selectCommand = new SqliteCommand
                    ($"SELECT Cryptowallet.coin , Cryptowallet.coin_amount, Users.euro_amount FROM Cryptowallet INNER JOIN Users ON Cryptowallet.userID = Users.userID WHERE Users.userID = {id}", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    coinList.Add(new CryptoWalletModel(query.GetString(0), query.GetFloat(1), query.GetFloat(2)));
                }
            }

            return coinList;
        }

        public static void CreateNewSellOrder(string type, float coinAmount, float euroAmount, int id)
        {

            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand insertCommand;
                SqliteCommand updateCommand;

                insertCommand = new SqliteCommand
                    ($"INSERT OR IGNORE INTO Orders (cointype,coin_amount,euro_amount,outstanding,userID) VALUES ('{type}',{coinAmount},{euroAmount},true,{id})", db);

                updateCommand = new SqliteCommand
                    ($"UPDATE Cryptowallet SET coin_amount = coin_amount - {coinAmount} WHERE userID = '{id}' AND coin = '{type}'", db);

                SqliteDataReader query1 = insertCommand.ExecuteReader();
                SqliteDataReader query2 = updateCommand.ExecuteReader();

            }


        }

        public static List<OrderModel> FetchOrders(int id)
        {
            List<OrderModel> orderList = new List<OrderModel>();
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand selectCommand;

                selectCommand = new SqliteCommand
                    ($"SELECT orderID, cointype, coin_amount , euro_amount , outstanding, userID FROM Orders WHERE outstanding = true", db);

                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    orderList.Add(new OrderModel(query.GetInt32(0), query.GetString(1), query.GetFloat(2), query.GetFloat(3), query.GetBoolean(4), query.GetInt32(5)));
                }

            }

            return orderList;
        }

        public static bool BuyOrder(int userID, OrderModel orderModel)
        {
            List<CryptoWalletModel> cryptoWalletList = FetchCoinsInWallet(userID);

            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                if (userID == orderModel.UserID)
                {
                    SqliteCommand RevertSellOrderCommand = new SqliteCommand
                    ($"UPDATE Cryptowallet SET coin_amount = coin_amount + {orderModel.Amount} WHERE userID = {userID} AND coin = '{orderModel.CoinName}'", db);
                    SqliteCommand deleteCommand = new SqliteCommand
                    ($"DELETE FROM Orders WHERE orderID = {orderModel.ID}", db);

                    SqliteDataReader revertQuery = RevertSellOrderCommand.ExecuteReader();
                    SqliteDataReader deleteQuery = deleteCommand.ExecuteReader();

                    return true;
                }

                if (cryptoWalletList.Exists(e => e.coinName == orderModel.CoinName))
                {
                    SqliteCommand updateCryptoWalletCommand1 = new SqliteCommand
                        ($"UPDATE Cryptowallet SET coin_amount = coin_amount + {orderModel.Amount} WHERE userID = {userID} AND coin = '{orderModel.CoinName}'", db);

                    SqliteDataReader updateCryptoWalletQuery = updateCryptoWalletCommand1.ExecuteReader();
                }
                else
                {
                    SqliteCommand insertNewCryptoCommand = new SqliteCommand
                            ($"INSERT OR IGNORE INTO Cryptowallet (coin, coin_amount, userID) VALUES ('{orderModel.CoinName}',{orderModel.Amount},{userID})", db);
                    SqliteDataReader insertNewCryptoQuery = insertNewCryptoCommand.ExecuteReader();
                }

                SqliteCommand updateOrdersCommand = new SqliteCommand
                           ($"UPDATE Orders SET outstanding = false WHERE orderID = {orderModel.ID}", db);
                SqliteCommand updateUsersCommand = new SqliteCommand
                    ($"UPDATE Users SET euro_amount = euro_amount - {orderModel.EuroAmount} WHERE userID = {userID}", db);
                SqliteCommand updateUsersEuroCommand = new SqliteCommand
                            ($"UPDATE Users SET euro_amount = euro_amount + {orderModel.EuroAmount} WHERE userID = {orderModel.UserID}", db);

                SqliteDataReader updateOrderQuery = updateOrdersCommand.ExecuteReader();
                SqliteDataReader updateUsersQuery6 = updateUsersCommand.ExecuteReader();
                SqliteDataReader updateUsersEuroQuery7 = updateUsersEuroCommand.ExecuteReader();

                return true;


            }

        }

    }
}
