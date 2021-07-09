using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Entry
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public string pcode { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double subtotal { get; set; }

        public Entry(int id, int order_id, string pcode, int quantity, double price, double subtotal)
        {
            this.id = id;
            this.order_id = order_id;
            this.pcode = pcode;
            this.quantity = quantity;
            this.price = price;
            this.subtotal = subtotal;
        }

        public void save()
        {
            String request = "insert into entry values(null, '" + pcode + "', '" + quantity + "', '" + price + "', " + order_id + ")";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void modifyEntry(int quant)
        {
            String request = "update entry set quantity=" + quant + " where id=" + id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void delete()
        {
            String request = "delete entry from entry where id=" + id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }
        public Entry(int entryId)
        {
            String request = "Select * from entries where id=" + entryId + ";";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.id = reader.GetInt32(0);
                this.order_id = reader.GetInt32(1);
                this.pcode = reader.GetString(2);
                this.quantity = reader.GetInt32(3);
                this.price = reader.GetDouble(4);
                this.subtotal = reader.GetDouble(5);
            }
        }

        public static List<Entry> getEntriesList()
        {
            String request = "Select * from entries";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Entry> listeEntries = new List<Entry>();

            while (reader.Read())
            {
                try
                {
                    int id = reader.GetInt32(0);
                    int order_id = reader.GetInt32(1);
                    string pcode = reader.GetString(2);
                    int quantity = reader.GetInt32(3);
                    double price = reader.GetDouble(4);
                    double subtotal = reader.GetDouble(5);
                    listeEntries.Add(new Entry (id, order_id, pcode, quantity, price, subtotal));
                }
                catch (System.Data.SqlTypes.SqlNullValueException e)
                {
                    int id = 0;
                    int order_id = 0;
                    string pcode = "";
                    int quantity = 0;
                    double price = 0;
                    double subtotal = 0;
                    listeEntries.Add(new Entry(id, order_id, pcode, quantity, price, subtotal));
                }
            }
            return listeEntries;
        }

        public static List<Entry> getEntriesListByOrderId(int orderId)
        {
            String request = "Select * from entries where order_id=" + orderId + ";";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Entry> listeEntries = new List<Entry>();

            while (reader.Read())
            {
                try
                {
                    int id = reader.GetInt32(0);
                    int order_id = reader.GetInt32(1);
                    string pcode = reader.GetString(2);
                    int quantity = reader.GetInt32(3);
                    double price = reader.GetDouble(4);
                    double subtotal = reader.GetDouble(5);
                    listeEntries.Add(new Entry(id, order_id, pcode, quantity, price, subtotal));
                }
                catch (System.Data.SqlTypes.SqlNullValueException e)
                {
                    int id = 0;
                    int order_id = 0;
                    string pcode = "";
                    int quantity = 0;
                    double price = 0;
                    double subtotal = 0;
                    listeEntries.Add(new Entry(id, order_id, pcode, quantity, price, subtotal));
                }
            }
            return listeEntries;
        }
    }
}
