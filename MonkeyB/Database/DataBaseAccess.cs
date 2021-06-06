﻿using Microsoft.Data.Sqlite;
using MonkeyB.Models;
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