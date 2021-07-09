using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo
{
    public class DeliveryAdress
    {
        public int delivery_id { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public int civicnumber { get; set; }
        public string appartment { get; set; }
        public string zipcode { get; set; }
        public int client_id { get; set; }

        public DeliveryAdress()
        {

        }

        public DeliveryAdress(int delivery_id, string country, string province, string city, string street, int civicnumber, string appartment, string zipcode, int client_id)
        {
            this.country = country;
            this.delivery_id = delivery_id;
            this.province = province;
            this.city = city;
            this.street = street;
            this.civicnumber = civicnumber;
            this.appartment = appartment;
            this.zipcode = zipcode;
            this.client_id = client_id;
         }

        public DeliveryAdress(string country, string province, string city, string street, int civicnumber, string appartment, string zipcode, int client_id)
        {
            this.country = country;
            this.province = province;
            this.city = city;
            this.street = street;
            this.civicnumber = civicnumber;
            this.appartment = appartment;
            this.zipcode = zipcode;
            this.client_id = client_id;
        }

        public DeliveryAdress(int id)
        {
            String request = "Select * from delivery_adresses where id=" + id + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.delivery_id = reader.GetInt32(0);
                this.country = reader.GetString(1);
                this.province = reader.GetString(2);
                this.city = reader.GetString(3);
                this.street = reader.GetString(4);
                this.civicnumber = reader.GetInt32(5);
                this.appartment = reader.GetString(6);
                this.zipcode = reader.GetString(7);
                this.client_id = reader.GetInt32(8);
            }
            DBManager.closeConnection();
        }

        
        public static List<DeliveryAdress> getDeliveryAdressList()
        {
            String request = "Select * from delivery_adresses";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<DeliveryAdress> listeClients = new List<DeliveryAdress>();

            while (reader.Read())
            {
                int delivery_id = reader.GetInt32(0);
                string country = reader.GetString(1);
                string province = reader.GetString(2);
                string city = reader.GetString(3);
                string street = reader.GetString(4);
                int civicnumber = reader.GetInt32(5);
                string appartment = reader.GetString(6);
                string zipcode = reader.GetString(7);
                int client_id = reader.GetInt32(8);
                listeClients.Add(new DeliveryAdress(delivery_id, country, province, city, street, civicnumber, appartment, zipcode, client_id));
            }
            DBManager.closeConnection();
            return listeClients;
        }

        public static List<DeliveryAdress> getInactiveDeliveryAdressList(int clientId)
        {
            String request = "Select * from inactive_addresses where client_id = " + clientId;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<DeliveryAdress> listeClients = new List<DeliveryAdress>();

            while (reader.Read())
            {
                int delivery_id = reader.GetInt32(0);
                string country = reader.GetString(1);
                string province = reader.GetString(2);
                string city = reader.GetString(3);
                string street = reader.GetString(4);
                int civicnumber = reader.GetInt32(5);
                string appartment = reader.GetString(6);
                string zipcode = reader.GetString(7);
                int client_id = reader.GetInt32(8);
                listeClients.Add(new DeliveryAdress(delivery_id, country, province, city, street, civicnumber, appartment, zipcode, client_id));
            }
            DBManager.closeConnection();
            return listeClients;
        }

        public static List<DeliveryAdress> getDeliveryAdressListByClientId(int clientId)
        {
            String request = "Select * from adresses where client_id=" + clientId;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<DeliveryAdress> listeClients = new List<DeliveryAdress>();

            while (reader.Read())
            {
                int delivery_id = reader.GetInt32(0);
                string country = reader.GetString(1);
                string province = reader.GetString(2);
                string city = reader.GetString(3);
                string street = reader.GetString(4);
                int civicnumber = reader.GetInt32(5);
                string appartment = reader.GetString(6);
                string zipcode = reader.GetString(7);
                int client_id = reader.GetInt32(8);
                listeClients.Add(new DeliveryAdress(delivery_id, country, province, city, street, civicnumber, appartment, zipcode, client_id));
            }
            DBManager.closeConnection();
            return listeClients;
        }



        public void save()
        {
            String request = "insert into deliveryaddress values(null, " + client_id + ", '" + country + "', '" + province + "', '" + city + "', '" + street + "', " + civicnumber + ", " + appartment + ", '" + zipcode + "', 'active')";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void modify()
        {
            String request = "update deliveryaddress set country='" + country + "', province='" + province + "', city='" + city + "', street='" + street + "', civicnumber=" + civicnumber + ", appartment=" + appartment + ", zipcode='" + zipcode + "' where id=" + delivery_id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void delete()
        {
            String request = "update deliveryaddress set status='inactive' where id=" + delivery_id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void activate()
        {
            String request = "update deliveryaddress set status='active' where id=" + delivery_id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }
    }
}