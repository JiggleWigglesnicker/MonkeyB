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


                    string adminCommand = "INSERT OR IGNORE INTO Users (username,password, euro_amount) VALUES ('admin','admin',1000)";
                    //string addcoin1 = "INSERT OR IGNORE INTO Cryptowallet (coin,Amount,userID) VALUES ('bitcoin',5.0,1)";
                    //string addcoin2 = "INSERT OR IGNORE INTO Cryptowallet (coin,Amount,userID) VALUES ('litecoin',10.0,1)";
                    //string addcoin3 = "INSERT OR IGNORE INTO Cryptowallet (coin,Amount,userID) VALUES ('etherium',105000.0,1)";

                    SqliteCommand createTable1 = new SqliteCommand(tableCommand1, db);
                    SqliteCommand createTable2 = new SqliteCommand(tableCommand2, db);
                    SqliteCommand createTable3 = new SqliteCommand(tableCommand3, db);

                    SqliteCommand createAdmin = new SqliteCommand(adminCommand, db);

                    //SqliteCommand createcoin1 = new SqliteCommand(addcoin1, db);
                    //SqliteCommand createcoin2 = new SqliteCommand(addcoin2, db);
                    //SqliteCommand createcoin3 = new SqliteCommand(addcoin3, db);



                    createTable1.ExecuteReader();
                    createTable2.ExecuteReader();
                    createTable3.ExecuteReader();
                    createAdmin.ExecuteReader();

                    //createcoin1.ExecuteReader();
                    //createcoin2.ExecuteReader();
                    //createcoin3.ExecuteReader();

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
            // doesnt check if sufficent money available and value can go into minus. that check in viewmodel
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                SqliteCommand insertCommand;
                SqliteCommand updateCommand;

                insertCommand = new SqliteCommand
                    ($"INSERT OR IGNORE INTO Orders (cointype,coin_amount,euro_amount,outstanding,userID) VALUES ('{type}',{coinAmount},{euroAmount},true,{id})", db);

                updateCommand = new SqliteCommand
                    ($"UPDATE Cryptowallet SET coin_amount = coin_amount - {coinAmount} WHERE userID = '{id}'", db);

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
                    ($"SELECT orderID, cointype, coin_amount , euro_amount , outstanding, userID FROM Orders WHERE userID = {id} AND outstanding = true", db);

                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    orderList.Add(new OrderModel(query.GetInt32(0), query.GetString(1), query.GetFloat(2), query.GetFloat(3), query.GetBoolean(4), query.GetInt32(5)));
                }

            }

            return orderList;
        }

        public static bool BuyOrder(int userID, int orderID, OrderModel orderModel)
        {
            List<CryptoWalletModel> cryptoWalletList = FetchCoinsInWallet(userID);
            using (var db = new SqliteConnection($"Data Source=database.db"))
            {
                db.Open();

                foreach (CryptoWalletModel wallet in cryptoWalletList)
                {
                   
                    // doesnt check if sufficent money available and value can go into minus. that check in viewmodel
                    if (wallet.coinName == orderModel.CoinName)
                    {
                        if (userID == orderModel.UserID)
                        {
                            SqliteCommand updateCommand8 = new SqliteCommand
                            ($"UPDATE Cryptowallet SET coin_amount = coin_amount + {orderModel.Amount} WHERE userID = {userID} AND coin = '{orderModel.CoinName}'", db);
                            SqliteCommand updateCommand9 = new SqliteCommand
                            ($"DELETE FROM Orders WHERE orderID = {orderModel.ID}", db);

                            SqliteDataReader updateQuery8 = updateCommand8.ExecuteReader();
                            SqliteDataReader updateQuery9 = updateCommand9.ExecuteReader();

                            return true;
                        }

                        SqliteCommand updateCommand1 = new SqliteCommand
                            ($"UPDATE Cryptowallet SET coin_amount = coin_amount + {orderModel.Amount} WHERE userID = {userID} AND coin = '{orderModel.CoinName}'", db);
                        SqliteCommand updateCommand2 = new SqliteCommand
                            ($"UPDATE Orders SET outstanding = false WHERE orderID = {orderID}", db);
                        SqliteCommand updateCommand3 = new SqliteCommand
                            ($"UPDATE Users SET euro_amount = euro_amount - {orderModel.EuroAmount} WHERE userID = {userID}", db);
                        SqliteCommand updateCommand4 = new SqliteCommand
                            ($"UPDATE Users SET euro_amount = euro_amount + {orderModel.EuroAmount} WHERE userID = {orderModel.UserID}", db);

                        SqliteDataReader updateQuery1 = updateCommand1.ExecuteReader();
                        SqliteDataReader updateQuery2 = updateCommand2.ExecuteReader();
                        SqliteDataReader updateQuery3 = updateCommand3.ExecuteReader();
                        SqliteDataReader updateQuery4 = updateCommand4.ExecuteReader();

                        return true;

                    }

                }

                SqliteCommand insertCommand1 = new SqliteCommand
                            ($"INSERT OR IGNORE INTO Cryptowallet (coin, coin_amount, userID) VALUES ('{orderModel.CoinName}',{orderModel.Amount},{userID})", db);
                SqliteCommand updateCommand5 = new SqliteCommand
                            ($"UPDATE Orders SET outstanding = false WHERE orderID = {orderID}", db);
                SqliteCommand updateCommand6 = new SqliteCommand
                    ($"UPDATE Users SET euro_amount = euro_amount - {orderModel.EuroAmount} WHERE userID = {userID}", db);
                SqliteCommand updateCommand7 = new SqliteCommand
                            ($"UPDATE Users SET euro_amount = euro_amount + {orderModel.EuroAmount} WHERE userID = {orderModel.UserID}", db);

                SqliteDataReader insertQuery1 = insertCommand1.ExecuteReader();
                SqliteDataReader updateQuery5 = updateCommand5.ExecuteReader();
                SqliteDataReader updateQuery6 = updateCommand6.ExecuteReader();
                SqliteDataReader updateQuery7 = updateCommand7.ExecuteReader();

                return true;

            }

        }

    }
}
