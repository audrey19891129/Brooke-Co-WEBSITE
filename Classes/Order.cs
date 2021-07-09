using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Order
    {
        public int order_id { get; set; }
        public int client_id { get; set; }
        public string order_date { get; set; }
        public string status { get; set; }
        public double subtotal { get; set; }
        public List<Entry> entries { get; set; }
        public List<Delivery> deliveries { get; set; }

        public Order(int order_id, int client_id,string order_date,  string status, double subtotal)
        {
            this.order_id = order_id;
            this.client_id = client_id;
            this.order_date = order_date;
            this.status = status;
            this.subtotal = subtotal;
            entries = new List<Entry>();
            deliveries = new List<Delivery>();
        }

        public Order(int client_id, string status)
        {
            this.client_id = client_id;  
            this.status = status;
        }

        public void addEntryToOrder(Entry E)
        {
            entries.Add(E);
        }

        public void save()
        {
            String request = "insert into brookeandco.order (client_id, status) values(" + client_id + ", 'ongoing')";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
            DBManager.closeConnection();
        }

        public void modify(string request)
        {
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
            DBManager.closeConnection();
        }

        public void setOrderDate(string date)
        {
            String request = "update brookeandco.order set date='" + date + "' where order_id=" + order_id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
            DBManager.closeConnection();
        }

        public void deleteAllEntries()
        {
            List<Entry> entries = Entry.getEntriesListByOrderId(order_id);

            foreach( Entry e in entries)
            {
                e.delete();
            }
        }

        public void deleteEntry(string Pcode)
        {
            List<Entry> entries = Entry.getEntriesListByOrderId(order_id);

            foreach (Entry e in entries)
            {
                if(e.pcode == Pcode)
                e.delete();
            }
        }

        public Order(int orderId)
        {
            String request = "Select * from brookeandco.orders where order_id=" + orderId + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                try
                {
                    this.order_id = reader.GetInt32(0);
                    this.client_id = reader.GetInt32(1);
                    DateTime date = reader.GetDateTime(2);
                    this.order_date = date.ToString("yyyy-MM-dd");
                    this.status = reader.GetString(3);
                    this.subtotal = reader.GetDouble(4);
                }
                catch (System.Data.SqlTypes.SqlNullValueException)
                {
                    this.order_id = reader.GetInt32(0);
                    this.client_id = reader.GetInt32(1);
                    this.order_date = "";
                    this.status = reader.GetString(3);
                    this.subtotal = 0;
                }
            }
            DBManager.closeConnection();
        }

        public static List<Order> getOrdersList()
        {
            String request = "Select * from brookeandco.orders";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Order> listeOrders = new List<Order>();

            while (reader.Read())
            {
                int order_id = reader.GetInt32(0);
                int client_id = reader.GetInt32(1);
                DateTime date = reader.GetDateTime(2);
                string order_date = date.ToString("yyyy-MM-dd");
                string status = reader.GetString(3);
                double subtotal = reader.GetDouble(4);
                listeOrders.Add(new Order(order_id, client_id, order_date, status, subtotal));
            }
            DBManager.closeConnection();
            return listeOrders;
        }

        public static List<Order> getOrdersListByClientId(int clientId)
        {
            String request = "Select * from brookeandco.orders where client_id=" + clientId + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Order> listeOrders = new List<Order>();

            while (reader.Read())
            {
                try
                {
                    int order_id = reader.GetInt32(0);
                    int client_id = reader.GetInt32(1);
                    DateTime date = reader.GetDateTime(2);
                    string order_date = date.ToString("yyyy-MM-dd");
                    string status = reader.GetString(3);
                    double subtotal = reader.GetDouble(4);
                    listeOrders.Add(new Order(order_id, client_id, order_date, status, subtotal));
                }
                catch (System.Data.SqlTypes.SqlNullValueException)
                {
                    int order_id = reader.GetInt32(0);
                    int client_id = reader.GetInt32(1);
                    string order_date = "";
                    string status = reader.GetString(3);
                    double subtotal = 0;
                    listeOrders.Add(new Order(order_id, client_id, order_date, status, subtotal));
                }
            }
            DBManager.closeConnection();
            return listeOrders;
        }
    }
}
