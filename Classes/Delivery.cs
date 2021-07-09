using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Delivery
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public DeliveryAdress delivery_address { get; set; }
        public int address_id { get; set; }
        public int service_id { get; set; }
        public int tracking_id { get; set; }
        public string status { get; set; }
        public int confirmation_number { get; set; }
        public string delivery_date { get; set; }
        public double total_cost { get; set; }

        public Delivery(int id, int order_id, DeliveryAdress delivery_address, int address_id, int service_id, int tracking_id, string status, int confirmation_number, string delivery_date, double total_cost)
        {
            this.id = id;
            this.order_id = order_id;
            this.delivery_address = delivery_address;
            this.address_id = address_id;
            this.service_id = service_id;
            this.tracking_id = tracking_id;
            this.status = status;
            this.confirmation_number = confirmation_number;
            this.delivery_date = delivery_date;
            this.total_cost = total_cost;
        }

        public void save()
        {
            String request = "insert into delivery values(null, " + order_id + ", " + address_id + ", " + service_id + ", null, 'received', null, '" + delivery_date + "', " + total_cost + ")";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public Delivery(int id)
        {
            String request = "Select * from deliveries where id=" + id + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.id = reader.GetInt32(0);
                this.order_id = reader.GetInt32(1);
                this.address_id = reader.GetInt32(2);
                this.service_id = reader.GetInt32(3);
                this.tracking_id = reader.GetInt32(4);
                this.status = reader.GetString(5);
                this.confirmation_number = reader.GetInt32(6);
                DateTime date = reader.GetDateTime(7);
                this.delivery_date = date.ToString("yyyy-MM-dd");
                this.total_cost = reader.GetFloat(8);
            }
        }

        public static List<Delivery> getDeliveriesList()
        {
            String request = "Select * from deliveries";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Delivery> listDeliveries = new List<Delivery>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int order_id = reader.GetInt32(1);
                int address_id = reader.GetInt32(2);
                int service_id = reader.GetInt32(3);
                int tracking_id = reader.GetInt32(4);
                string status = reader.GetString(5);
                int confirmation_number = reader.GetInt32(6);
                DateTime date = reader.GetDateTime(7);
                string delivery_date = date.ToString("yyyy-MM-dd");
                DeliveryAdress delivery_address = new DeliveryAdress();
                double total_cost = reader.GetFloat(8);
                listDeliveries.Add(new Delivery(id, order_id, delivery_address, address_id, service_id, tracking_id, status, confirmation_number, delivery_date, total_cost));
            }
            return listDeliveries;
        }

        public static List<Delivery> getDeliveriesListByOrderId(int orderId)
        {
            String request = "Select * from deliveries where order_id=" + orderId + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Delivery> listDeliveries = new List<Delivery>();

            while (reader.Read())
            {
                try
                {
                    int id = reader.GetInt32(0);
                    int order_id = reader.GetInt32(1);
                    int address_id = reader.GetInt32(2);
                    int service_id = reader.GetInt32(3);
                    int tracking_id = reader.GetInt32(4);
                    string status = reader.GetString(5);
                    int confirmation_number = reader.GetInt32(6);
                    DateTime date = reader.GetDateTime(7);
                    string delivery_date = date.ToString("yyyy-MM-dd");
                    DeliveryAdress delivery_address = new DeliveryAdress();
                    double total_cost = reader.GetFloat(8);
                    listDeliveries.Add(new Delivery(id, order_id, delivery_address, address_id, service_id, tracking_id, status, confirmation_number, delivery_date, total_cost));
                }
                catch (System.Data.SqlTypes.SqlNullValueException)
                {
                    int id = reader.GetInt32(0);
                    int order_id = reader.GetInt32(1);
                    int address_id = reader.GetInt32(2);
                    int service_id = reader.GetInt32(3);
                    int tracking_id = 0;
                    string status = reader.GetString(5);
                    int confirmation_number = 0;
                    DateTime date = reader.GetDateTime(7);
                    string delivery_date = date.ToString("yyyy-MM-dd");
                    DeliveryAdress delivery_address = new DeliveryAdress();
                    double total_cost = reader.GetFloat(8);
                    listDeliveries.Add(new Delivery(id, order_id, delivery_address, address_id, service_id, tracking_id, status, confirmation_number, delivery_date, total_cost));

                }
            }
            return listDeliveries;
        }
    }
}