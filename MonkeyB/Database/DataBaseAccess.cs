using Microsoft.Data.Sqlite;
using MonkeyB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonkeyB.Database
{
    public static class DataBaseAccess
    {
        private static string FolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string DbPath { get; set; } = System.IO.Path.Combine(FolderPath, "database.db");

        /// <summary>
        /// Initilizes the DB
        /// </summary>
        public static async void InitializeDatabase()
        {
            await Task.Run(() =>
            {
                using (var db = new SqliteConnection($"Data Source=database.db"))
                {
                    System.Diagnostics.Debug.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\n");

                    db.Open();

                    // This table stores the users information
                    string UserTable =
                    "CREATE TABLE IF NOT EXISTS Users " +
                    "(userID INTEGER NOT NULL UNIQUE, " +
                    "username TEXT NOT NULL UNIQUE, " +
                    "password TEXT NOT NULL, " +
                    "euro_amount FLOAT NOT NULL DEFAULT 1000, " +
                    "PRIMARY KEY(userID AUTOINCREMENT))";

                    // This table stores all the owned crypto
                    string CryptoWalletTable =
                    "CREATE TABLE IF NOT EXISTS Cryptowallet " +
                    "(cryptowalletID INTEGER NOT NULL UNIQUE, " +
                    "coin TEXT NOT NULL, " +
                    "coin_amount  FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(cryptowalletID AUTOINCREMENT))";

                    // This table stores all the order data
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

                    // This table stores all the transaction data
                    string TransactionHistoryTable =
                    "CREATE TABLE IF NOT EXISTS TransactionHistory " +
                    "(ID INTEGER NOT NULL UNIQUE, " +
                    "currency_name STRING NOT NULL, " +
                    "currency_amount FLOAT NOT NULL, " +
                    "currency_value FLOAT NOT NULL, " +
                    "userID INTEGER NOT NULL," +
                    "FOREIGN KEY (userID) REFERENCES Users(userID)," +
                    "PRIMARY KEY(ID AUTOINCREMENT))";

                    // This tbale stores all the anchievements data
                    string AchievementTable =
                   "CREATE TABLE IF NOT EXISTS Achievements " +
                   "(AchievementID INTEGER NOT NULL UNIQUE, " +
                   "Name STRING NOT NULL, " +
                   "Description FLOAT NOT NULL, " +
                   "IsCompleted BOOLEAN NOT NULL, " +
                   "userID INTEGER NOT NULL, " +
                   "PRIMARY KEY(AchievementID AUTOINCREMENT)," +
                   "FOREIGN KEY(userID) REFERENCES Users(userID))";

                    // Insert admin user if not exsists
                    string adminCommand = 
                    "INSERT OR IGNORE INTO Users (username,password) " +
                    "VALUES ('admin','admin')";

                    SqliteCommand createUserTable           = new(UserTable, db);
                    SqliteCommand createCryptoWalletTable   = new(CryptoWalletTable, db);
                    SqliteCommand createOrderTable          = new(OrderTable, db);
                    SqliteCommand createTransactionTable    = new(TransactionHistoryTable, db);
                    SqliteCommand createAchievementTable    = new(AchievementTable, db);
                    SqliteCommand createAdmin               = new(adminCommand, db);

                    createUserTable.ExecuteReader();
                    createCryptoWalletTable.ExecuteReader();
                    createOrderTable.ExecuteReader();
                    createTransactionTable.ExecuteReader();
                    createAchievementTable.ExecuteReader();
                    createAdmin.ExecuteReader();
                }
            });
        }

        /// <summary>
        /// Add the coins to a users wallet if not added set already
        /// </summary>
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

        /// <summary>
        /// Add the achievements if not added set already
        /// </summary>
        public static void InitializeAchievements()
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                List<string> AchievementNames = new List<string>() { "10 Doge", "10 bit", "10 litecoin", "10k CLUB" };
                db.Open();
                SqliteCommand selectCommand;

                foreach (var achievements in AchievementNames)
                {
                    string insertCommandAchievements1 = $"INSERT INTO Achievements (Name, Description, IsCompleted, userID) VALUES ('10 Doge', 'Buy 10 Dogecoin', false, {App.UserID})";
                    string insertCommandAchievements2 = $"INSERT INTO Achievements (Name, Description, IsCompleted, userID) VALUES ('10 bit', 'Buy 10 bitcoin', false, {App.UserID})";
                    string insertCommandAchievements3 = $"INSERT INTO Achievements (Name, Description, IsCompleted, userID) VALUES ('10 litecoin', 'Buy 10 litecoin', false, {App.UserID})";
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

        /// <summary>
        /// Retrieve login data
        /// </summary>
        /// <param name="username"></param>
        /// <returns>LoginModel with the users login data</returns>
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

        /// <summary>
        /// Lowers the amount of coin in the users wallet
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <param name="userID"></param>
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

        /// <summary>
        /// Increment the amount of coin in the users wallet
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <param name="userID"></param>
        public static async void BuyCoin(string currency, float amount, int userID)
        {
            CryptoCurrencyModel model;
            ApiHandler apiHandler = new ApiHandler();
            
            model = await apiHandler.GetCoinValue(currency);


            await using var db = new SqliteConnection($"Data Source=database.db");
            db.Open();

            SqliteCommand updateCommand;
            updateCommand = new SqliteCommand($"UPDATE CryptoWallet SET coin_amount = coin_amount + '{amount}'   WHERE userID = '{userID}' AND coin = '{currency}'", db);
            updateCommand.ExecuteNonQuery();

            await Task.Run( async () =>
            {
                model = await apiHandler.GetCoinValue(currency);
                SqliteCommand insertCommand;
                float currentprice = 0.0F;
                switch (currency)
                {
                    case "bitcoin":
                        currentprice = model.bitcoin.eur;
                        break;
                    case "litecoin":
                        currentprice = model.litecoin.eur;
                        break;
                    case "dogecoin":
                        currentprice = model.dogecoin.eur;
                        break;
                }

                if (FetchTransactionHistory(userID).Exists(trans => trans.coinName == currency))
                {
                    updateCommand = new SqliteCommand($"UPDATE TransactionHistory SET currency_amount = {GetCoinAmount(currency, userID)}, currency_value = {currentprice} WHERE userID = '{userID}' AND currency_name = '{currency}' ", db);
                    updateCommand.ExecuteNonQuery();

                }
                else
                {
                    insertCommand = new SqliteCommand($"INSERT INTO TransactionHistory(currency_name, currency_amount,currency_value, userID) VALUES ('{currency}', {amount},{currentprice} ,{userID})", db);
                    insertCommand.ExecuteNonQuery();
                }
                   

            });
        }

        /// <summary>
        /// Increments the amount of euro that a user owns
        /// </summary>
        /// <param name="amount"></param>
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

        /// <summary>
        /// Lowers the amount of euro that a user owns
        /// </summary>
        /// <param name="amount"></param>
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

        /// <summary>
        /// Gets the amount of coin that a users owns
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="userID"></param>
        /// <returns>Float with the amount of currency</returns>
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

        /// <summary>
        /// Add a new user to the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>bool that indicates if the registration is succesfull</returns>
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

        /// <summary>
        /// Updates the amount of euro a user owns
        /// </summary>
        /// <param name="amount"></param>
        public static void UpdateEuroAmount(float amount)
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

        /// <summary>
        /// Gets amount of coin in the wallet from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List<CryptoWalletModel></returns>
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

        /// <summary>
        /// Gets the transaction history from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List<TransactionHistoryModel></returns>
        public static List<TransactionHistoryModel> FetchTransactionHistory(int id)
        {
            List<TransactionHistoryModel> coinList = new List<TransactionHistoryModel>();
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand selectCommand;

                selectCommand = new SqliteCommand
                    ($"SELECT currency_name, currency_amount,currency_value FROM TransactionHistory WHERE userID = {id}", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    coinList.Add(new TransactionHistoryModel(query.GetString(0), query.GetFloat(1), query.GetFloat(2)));
                }
            }

            return coinList;
        }

        /// <summary>
        /// Fetched the amount of of coin from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<TransactionHistoryModel> FetchCoinAndAmount(int id)
        {
            List<TransactionHistoryModel> coinList = new List<TransactionHistoryModel>();
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand selectCommand;

                selectCommand = new SqliteCommand
                    ($"SELECT Cryptowallet.coin , Cryptowallet.coin_amount FROM Cryptowallet WHERE userID = {id}", db);
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    coinList.Add(new TransactionHistoryModel(query.GetString(0), query.GetFloat(1)));
                }
            }

            return coinList;
        }


        /// <summary>
        /// CreateNewSellOrder
        /// </summary>
        /// <param name="type"></param>
        /// <param name="coinAmount"></param>
        /// <param name="euroAmount"></param>
        /// <param name="id"></param>
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

        /// <summary>
        /// Fetch orders
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List<OrderModel></returns>
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

        /// <summary>
        /// Fetch achievements from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List<AchievementModel></returns>
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
                    achievementList.Add(new AchievementModel(query.GetString(0), query.GetString(1), query.GetBoolean(2)));
                }

            }

            return achievementList;
        }

        /// <summary>
        /// Completes achievment when user fullfills requirement of achievement
        /// </summary>
        /// <param name="id">id of user</param>
        /// <param name="achievementName"> name of the achievement to be completed</param>
        public static void CompleteAchievement(int id, string achievementName)
        {
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand updateCommand;
                updateCommand = new SqliteCommand
                    ($"UPDATE Achievements SET IsCompleted = true WHERE userID = '{id}' AND Name = '{achievementName}'", db);

                SqliteDataReader updateQuery = updateCommand.ExecuteReader();

            }

        }

        /// <summary>
        /// Buy an order
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="orderModel"></param>
        /// <returns>bool</returns>
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

