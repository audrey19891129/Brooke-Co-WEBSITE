using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Sale
    {
        public string pcode { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string picture { get; set; }
        public string genre { get; set; }
        public int quantity { get; set; }
        public double price_bought { get; set; }
        public double price_sold { get; set; }
        public string order_date { get; set; }
        public double profits { get; set; }

        public Sale(string pcode, string type, string title, string category, string picture, string genre, int quantity, double price_bought, double price_sold, string order_date, double profits)
        {
            this.pcode = pcode;
            this.type = type;
            this.title = title;
            this.category = category;
            this.genre = genre;
            this.quantity = quantity;
            this.order_date = order_date;
            this.picture = picture;
            this.price_bought = price_bought;
            this.price_sold = price_sold;
            this.profits = profits;
        }

        public static List<Sale> getSalesList()
        {

            String request = "Select * from sales";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Sale> list = new List<Sale>();

            while (reader.Read())
            {
                string pcode = reader.GetString(0);
                string type = reader.GetString(1);
                string title = reader.GetString(2);
                string category = reader.GetString(3);
                string picture = reader.GetString(4);
                string genre = reader.GetString(5);
                int quantity = reader.GetInt32(6);
                double price_bought = reader.GetFloat(7);
                double price_sold = reader.GetFloat(8);
                DateTime date = reader.GetDateTime(9);
                string order_date = date.ToString("yyyy-MM-dd");
                double profits = reader.GetFloat(10);
                list.Add(new Sale(pcode, type, title, category, picture, genre, quantity, price_bought, price_sold, order_date, profits));
            }
            DBManager.closeConnection();
            return list;
        }
    }
}