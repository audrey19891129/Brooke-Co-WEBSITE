using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace WebApplicationBrookeAndCo.Classes
{
    public class DBManager
    {
        private static string host { get; set; }
        private static string database { get; set; }
        private static string username { get; set; }
        private static string password { get; set; }
        private static int port { get; set; }
        private static MySqlConnection conn { get; set; }

        public static void createConnection()
        {
            string connString = ConfigurationManager.ConnectionStrings["DBMySQLBrookeAndCo"].ConnectionString;
            conn = new MySqlConnection(connString);
        }
        public static void createConnection(string servername, string database, string username, string password, int port)
        {
            DBManager.host = servername;
            DBManager.port = port;
            DBManager.database = database;
            DBManager.username = username;
            DBManager.password = password;
            string connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;
            conn = new MySqlConnection(connString);
        }

        public static int executeUpdate(string request)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = request;
            int lines = cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return lines;
        }

        public static DbDataReader executeQuery(string request)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = request;
            DbDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public static void closeConnection()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}