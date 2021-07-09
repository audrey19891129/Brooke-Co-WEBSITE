using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo
{
	public class Product
	{
        public string pcode { get; set; }
        public string picture { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public int inventory { get; set; }
        public string genre { get; set; }
        public double price { get; set; }
        public int bought { get; set; }
        public int left { get; set; }
        public int warehouse { get; set; }

        public Product()
        {
            this.pcode = "";
            this.picture = "";
            this.type = "";
            this.title = "";
            this.category = "";
            this.inventory = 0;
            this.genre = "";
            this.price = 0;
        }

        public Product(string pcode, string picture, string type, string title, string category, int inventory, string genre, double price)
		{
            this.pcode = pcode;
			this.picture = picture;
			this.type = type;
			this.title = title;
			this.category = category;
			this.inventory = inventory;
			this.genre = genre;
			this.price = price;
		}

        public Product(string pcode, string type, string title, string category, string genre, double price, string picture, int bought, int left, int warehouse)
        {
            this.pcode = pcode;
            this.picture = picture;
            this.type = type;
            this.title = title;
            this.category = category;
            this.genre = genre;
            this.price = price;
            this.bought = bought;
            this.left = left;
            this.warehouse = warehouse;
        }

        public Product(string Pcode)
        {
            String request = "Select * from products2 where pcode='" + Pcode + "';";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                try
                {
                    this.pcode = reader.GetString(0);
                    this.type = reader.GetString(1);
                    this.title = reader.GetString(2);
                    this.category = reader.GetString(3);
                    this.price = reader.GetDouble(4);
                    this.picture = reader.GetString(5);
                    this.bought = reader.GetInt32(6);
                    this.left = reader.GetInt32(7);
                    this.genre = reader.GetString(8);
                    
                }
                catch (System.Data.SqlTypes.SqlNullValueException)
                {
                }
            }
            DBManager.closeConnection();
        }

        public static List<Product> getProducts()
        {

            String request = "Select * from products";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Product> list = new List<Product>();

            while (reader.Read())
            {
                string pcode = reader.GetString(0);
                string type = reader.GetString(1);
                string title = reader.GetString(2);
                string category = reader.GetString(3);
                string genre = reader.GetString(4);
                double price = reader.GetDouble(5);
                string picture = reader.GetString(6);
                int bought = reader.GetInt32(7);
                int left = reader.GetInt32(8);
                int warehouse = reader.GetInt32(9);
                list.Add(new Product(pcode, type, title, category, genre, price, picture, bought, left, warehouse));
            }
            DBManager.closeConnection();
            return list;
        }

        public static List<Product> getProductsByTitle(string title)
        {

            String request = "Select * from products where title like \"%" + title + "%\"";
            
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Product> list = new List<Product>();

            while (reader.Read())
            {
                string pcode = reader.GetString(0);
                string type = reader.GetString(1);
                string category = reader.GetString(3);
                string genre = reader.GetString(4);
                double price = reader.GetDouble(5);
                string picture = reader.GetString(6);
                int bought = reader.GetInt32(7);
                int left = reader.GetInt32(8);
                int warehouse = reader.GetInt32(9);
                list.Add(new Product(pcode, type, title, category, genre, price, picture, bought, left, warehouse));
            }
            DBManager.closeConnection();
            return list;
        }

        public static List<Product> getTopTen()
        {

            String request = "Select * from topTen";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Product> list = new List<Product>();

            while (reader.Read())
            {
                string pcode = reader.GetString(0);
                string type = reader.GetString(1);
                string title = reader.GetString(2);
                string category = reader.GetString(3);
                string genre = reader.GetString(4);
                double price = reader.GetDouble(5);
                string picture = reader.GetString(6);
                int bought = reader.GetInt32(7);
                int left = reader.GetInt32(8);
                int warehouse = reader.GetInt32(9);
                list.Add(new Product(pcode, type, title, category, genre, price, picture, bought, left, warehouse));
            }
            DBManager.closeConnection();
            return list;
        }

        public void save()
        {
            String request = "insert into product values(null, '" + pcode + "', '" + type + "', '" + category + "', " + price + ", '" + picture + "', '" + title + "','" + genre + "', 0, 0, 'inactive', 1 , 20)";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            int lines = DBManager.executeUpdate(request);
            DBManager.closeConnection();
        }

        public void modify(string request)
        {
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            int lines = DBManager.executeUpdate(request);
            DBManager.closeConnection();
        }
    }
}