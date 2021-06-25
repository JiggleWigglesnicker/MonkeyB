using Microsoft.Data.Sqlite;
using MonkeyB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

                    string UserTable =
                    "CREATE TABLE IF NOT EXISTS Users " +
                    "(userID INTEGER NOT NULL UNIQUE, " +
                    "username TEXT NOT NULL UNIQUE, " +
                    "password TEXT NOT NULL, " +
                    "euro_amount FLOAT NOT NULL DEFAULT 1000, " +
                    "PRIMARY KEY(userID AUTOINCREMENT))";

                    string CryptoWalletTable =
                    "CREATE TABLE IF NOT EXISTS Cryptowallet " +
                    "(cryptowalletID INTEGER NOT NULL UNIQUE, " +
                    "coin TEXT NOT NULL, " +
                    "coin_amount  FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(cryptowalletID AUTOINCREMENT))";

                    string OrderTable =
                    "CREATE TABLE IF NOT EXISTS Orders " +
                    "(orderID INTEGER NOT NULL UNIQUE, " +
                    "cointype TEXT NOT NULL, " +
                    "coin_amount  FLOAT NOT NULL, " +
                    "euro_amount  FLOAT NOT NULL, " +
                    "outstanding BOOLEAN NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(orderID AUTOINCREMENT))";

                    string TransactionHistoryTable =
                    "CREATE TABLE IF NOT EXISTS TransactionHistory " +
                    "(ID INTEGER NOT NULL UNIQUE, " +
                    "currency_name STRING NOT NULL, " +
                    "currency_amount FLOAT NOT NULL, " +
                    "currency_value FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID))";

                    string AchievementTable =
                   "CREATE TABLE IF NOT EXISTS Achievements " +
                   "(AchievementID INTEGER NOT NULL UNIQUE, " +
                   "Name STRING NOT NULL, " +
                   "Description FLOAT NOT NULL, " +
                   "IsCompleted BOOLEAN NOT NULL, " +
                   "userID INTEGER NOT NULL, " +
                   "PRIMARY KEY(AchievementID AUTOINCREMENT)," +
                   "FOREIGN KEY(userID) REFERENCES Users(userID))";


                    string adminCommand = "INSERT OR IGNORE INTO Users (username,password) VALUES ('admin','admin')";

                    SqliteCommand createTable1 = new SqliteCommand(UserTable, db);
                    SqliteCommand createTable2 = new SqliteCommand(CryptoWalletTable, db);
                    SqliteCommand createTable3 = new SqliteCommand(OrderTable, db);
                    SqliteCommand createTable4 = new SqliteCommand(TransactionHistoryTable, db);
                    SqliteCommand createTable5 = new SqliteCommand(AchievementTable, db);
                    SqliteCommand createAdmin = new SqliteCommand(adminCommand, db);

                    createTable1.ExecuteReader();
                    createTable2.ExecuteReader();
                    createTable3.ExecuteReader();
                    createTable4.ExecuteReader();
                    createTable5.ExecuteReader();



                    createAdmin.ExecuteReader();
                        

                }
            });
        }

        public static void InitializeCoins()
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                List<string> CurrencyNames = new List<string>() { "bitcoin", "dogecoin", "litecoin" };
                db.Open();
                SqliteCommand selectCommand;
                foreach (var currency in CurrencyNames)
                {
                    SqliteCommand initCommand = new SqliteCommand($"INSERT OR IGNORE INTO CryptoWallet(coin, coin_amount, userID) VALUES('{currency}', 0,{App.UserID})", db);
                    selectCommand = new SqliteCommand
                        ($"SELECT coin, userID FROM Cryptowallet WHERE userID = '{App.UserID}' AND coin = '{currency}'", db);
                    var result = selectCommand.ExecuteReader();
                    if (result.HasRows || currency == String.Empty) return;
                    initCommand.ExecuteReader();
                }
            }
        }

        public static void InitializeAchievements()
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                List<string> AchievementNames = new List<string>() { "10 Doge", "10 bit", "10 ethereum", "10k CLUB" };
                db.Open();
                SqliteCommand selectCommand;

                foreach (var achievements in AchievementNames)
                {
                    string insertCommandAchievements1 = $"INSERT INTO Achievements (Name, Description, IsCompleted, userID) VALUES ('10 Doge', 'Buy 10 Dogecoin', false, {App.UserID})";
                    string insertCommandAchievements2 = $"INSERT INTO Achievements (Name, Description, IsCompleted, userID) VALUES ('10 bit', 'Buy 10 bitcoin', false, {App.UserID})";
                    string insertCommandAchievements3 = $"INSERT INTO Achievements (Name, Description, IsCompleted, userID) VALUES ('10 Etherium', 'Buy 10 ethereum', false, {App.UserID})";
                    string insertCommandAchievements4 = $"INSERT INTO Achievements (Name, Description, IsCompleted, userID) VALUES ('10k CLUB', 'have 10.000 in funds', false, {App.UserID})";

                    selectCommand = new SqliteCommand
                        ($"SELECT Name, userID FROM Achievements WHERE userID = '{App.UserID}' AND Name = '{achievements}'", db);
                    var result = selectCommand.ExecuteReader();
                    if (result.HasRows) return;
                    SqliteCommand insertCreateCommandAchievements1 = new SqliteCommand(insertCommandAchievements1, db);
                    SqliteCommand insertCreateCommandAchievements2 = new SqliteCommand(insertCommandAchievements2, db);
                    SqliteCommand insertCreateCommandAchievements3 = new SqliteCommand(insertCommandAchievements3, db);
                    SqliteCommand insertCreateCommandAchievements4 = new SqliteCommand(insertCommandAchievements4, db);
                    insertCreateCommandAchievements1.ExecuteReader();
                    insertCreateCommandAchievements2.ExecuteReader();
                    insertCreateCommandAchievements3.ExecuteReader();
                    insertCreateCommandAchievements4.ExecuteReader();
                }
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

        public static List<TransactionHistoryModel> FetchTransactionHistory(int id)
        {
            List<TransactionHistoryModel> coinList = new List<TransactionHistoryModel>();
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand selectCommand;

                selectCommand = new SqliteCommand
                    ($"SELECT Cryptowallet.coin , Cryptowallet.coin_amount FROM Cryptowallet WHERE userID = {id}", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                
                // elke coin in wallet de waarde van ophalen
                
                while (query.Read())
                {
                    coinList.Add(new TransactionHistoryModel(query.GetString(0), query.GetFloat(1)));
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

        public static List<AchievementModel> FetchAchievements(int id)
        {
            List<AchievementModel> achievementList = new List<AchievementModel>();
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand selectCommand;

                selectCommand = new SqliteCommand
                    ($"SELECT Name, Description, IsCompleted FROM Achievements WHERE userID = {id}", db);

                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    achievementList.Add(new AchievementModel(query.GetString(0),query.GetString(1),query.GetBoolean(2)));
                }

            }

            return achievementList;
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

