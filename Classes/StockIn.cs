using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;

namespace WebApplicationBrookeAndCo.Classes
{
    public class StockIn
    {
        public int id { get; set; }
        public string pcode { get; set; }
        public int provider_id { get; set; }
        public int purchase_id { get; set; }
        public int bought_quant { get; set; }
        public double unit_price { get; set; }
        public string received_date { get; set; }
        public int left_quant { get; set; }

        public StockIn(int id, string pcode, int provider_id, int purchase_id, int bought_quant, double unit_price, string received_date, int left_quant)
        {
            this.id = id;
            this.pcode = pcode;
            this.provider_id = provider_id;
            this.purchase_id = purchase_id;
            this.bought_quant = bought_quant;
            this.unit_price = unit_price;
            this.received_date = received_date;
            this.left_quant = left_quant;
        }

        public StockIn(int id)
        {
            String request = "Select * from stocks_in where id=" + id + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.id = id;
                this.pcode = pcode;
                this.provider_id = provider_id;
                this.purchase_id = purchase_id;
                this.bought_quant = bought_quant;
                this.unit_price = unit_price;
                this.received_date = received_date;
                this.left_quant = left_quant;
            }
        }

        public static List<StockIn> getStockInList()
        {
            String request = "Select * from stocks_in";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<StockIn> list = new List<StockIn>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string pcode = reader.GetString(1);
                int provider_id = reader.GetInt32(2);
                int purchase_id = reader.GetInt32(3);
                int bought_quant = reader.GetInt32(4);
                double unit_price = reader.GetFloat(5);
                DateTime d = reader.GetDateTime(6);
                string received_date = d.ToString("yyyy-MM-dd");
                int left_quant = reader.GetInt32(7);
                list.Add(new StockIn(id, pcode, provider_id, purchase_id, bought_quant, unit_price, received_date, left_quant));
            }
            DBManager.closeConnection();
            return list;
        }

        public static List<StockIn> getStockInListByPcode(string pcode)
        {
            String request = "Select * from stocks_in where pcode='" + pcode + "'";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<StockIn> list = new List<StockIn>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int provider_id = reader.GetInt32(2);
                int purchase_id = reader.GetInt32(3);
                int bought_quant = reader.GetInt32(4);
                double unit_price = reader.GetFloat(5);
                DateTime d = reader.GetDateTime(6);
                string received_date = d.ToString("yyyy-MM-dd");
                int left_quant = reader.GetInt32(7);
                list.Add(new StockIn(id, pcode, provider_id, purchase_id, bought_quant, unit_price, received_date, left_quant));
            }
            DBManager.closeConnection();
            return list;
        }

        public void save()
        {
            String request = "insert into stock_in values(null, '" + pcode + "', " + provider_id + ", " + purchase_id + ", " + bought_quant + "," + unit_price + ", '"+ received_date + "', " + left_quant+")";
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
    }
}