using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo
{
    public class Client
    {
        public int id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string bday { get; set; }
        public List<DeliveryAdress> adresses { get; set; }
        public List<Order> orders { get; set; }

        public Client(int id, string email, string username, string password, string name, string lastname, string bday)
        {
            this.id = id;
            this.email = email;
            this.username = username;
            this.password = password;
            this.name = name;
            this.lastname = lastname;
            this.bday = bday;
            adresses = new List<DeliveryAdress>();
            orders = new List<Order>();
        }

        public Client(string email, string username, string password, string name, string lastname, string bday)
        {
            this.email = email;
            this.username = username;
            this.password = password;
            this.name = name;
            this.lastname = lastname;
            this.bday = bday;
        }

        public Client(int id)
        {

            String request = "Select * from clients where id=" + id + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.id = reader.GetInt32(0);
                this.email = reader.GetString(1);
                this.username = reader.GetString(2);
                this.password = reader.GetString(3);
                this.name = reader.GetString(4);
                this.lastname = reader.GetString(5);
                DateTime date = reader.GetDateTime(6);
                this.bday = date.ToString("yyyy-MM-dd");
                this.orders = new List<Order>();
                this.adresses = new List<DeliveryAdress>();
            }
            DBManager.closeConnection();
        }

        public Client(string user, string pass)
        {
            String request = "Select * from clients where username='" + user + "' and password='" + pass + "';";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            while (reader.Read())
            {
                this.id = reader.GetInt32(0);
                this.email = reader.GetString(1);
                this.username = reader.GetString(2);
                this.password = reader.GetString(3);
                this.name = reader.GetString(4);
                this.lastname = reader.GetString(5);
                DateTime date = reader.GetDateTime(6);
                this.bday = date.ToString("yyyy-MM-dd");
                this.orders = new List<Order>();
                this.adresses = new List<DeliveryAdress>();
            }
            DBManager.closeConnection();
        }

        public Client(string request)
        {
            //String request = "Select * from clients where username='" + username + "'";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            while (reader.Read())
            {
                try
                {
                    this.id = reader.GetInt32(0);
                    this.email = reader.GetString(1);
                    this.username = reader.GetString(2);
                    this.password = reader.GetString(3);
                    this.name = reader.GetString(4);
                    this.lastname = reader.GetString(5);
                    DateTime date = reader.GetDateTime(6);
                    this.bday = date.ToString("yyyy-MM-dd");
                    this.orders = new List<Order>();
                    this.adresses = new List<DeliveryAdress>();
                }
                catch
                {
                    this.id = 0;
                    this.email = "";
                    this.username = "";
                    this.password = "";
                    this.name = "";
                    this.lastname = "";
                    this.bday = "";
                    this.orders = new List<Order>();
                    this.adresses = new List<DeliveryAdress>();
                }
               
            }
            DBManager.closeConnection();
        }

        public static List<Client> getClientsList()
        {
            String request = "Select * from clients";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Client> listeClients = new List<Client>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string email = reader.GetString(1);
                string username = reader.GetString(2);
                string password = reader.GetString(3);
                string name = reader.GetString(4);
                string lastname = reader.GetString(5);
                DateTime date = reader.GetDateTime(6);
                string bday = date.ToString("yyyy-MM-dd");

                listeClients.Add(new Client(id, email, username, password, name, lastname, bday));
            }
            DBManager.closeConnection();
            return listeClients;
        }


        public void save()
        {
            String request = "insert into client (email, username, password, name, lastname, bday) values('" + email + "', '" + username + "', '" + password + "', '" + name + "', '" + lastname + "', '" + bday +"')";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void modify()
        {
            String request = "update client set email='" + email + "', username='" + username + "', password='" + password + "', name='" + name + "', lastname='" + lastname + "', bday='" + bday +"' where id =" + id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void delete()
        {
            String request = "update client set status='inactive' where id=" + id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }
    }
}