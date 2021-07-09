using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo
{
    class Book : Product
    {
        public string authors { get; set; }
        public string pubCo { get; set; }
        public string pubDate { get; set; }

        public Book(string pcode, string picture, string type, string title, string category, int inventory, string genre, double price, string authors, string pubCo, string pubDate)
        {
            this.pcode = pcode;
            this.picture = picture;
            this.type = type;
            this.title = title;
            this.category = category;
            this.inventory = inventory;
            this.genre = genre;
            this.price = price;
            this.authors = authors;
            this.pubCo = pubCo;
            this.pubDate = pubDate;
        }

        public Book(string pcode)
        {
            String request = "Select * from books where pcode='" + pcode + "';";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Book> listeProduits = new List<Book>();

            while (reader.Read())
            {
                this.pcode = reader.GetString(0);
                this.authors = reader.GetString(1);
                this.pubCo = reader.GetString(2);
                DateTime date = reader.GetDateTime(3);
                this.pubDate = date.ToString("yyyy-MM-dd");
                this.type = reader.GetString(4);
                this.category = reader.GetString(5);
                this.price = reader.GetDouble(6);
                this.picture = reader.GetString(7);
                this.title = reader.GetString(8);
                this.inventory = reader.GetInt32(9);
                this.genre = reader.GetString(12);
            }
            DBManager.closeConnection();
        }

        public static List<Book> getBookListByCategory(string cat)
        {
            
            String request = "Select * from books where category ='" + cat + "';";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Book> listeProduits = new List<Book>();

            while (reader.Read())
            {
                string pcode = reader.GetString(0);
                string authors = reader.GetString(1);
                string pubCo = reader.GetString(2);
                DateTime date = reader.GetDateTime(3);
                string pubDate = date.ToString("yyyy-MM-dd");
                string type = reader.GetString(4);
                string category = reader.GetString(5);
                double price = reader.GetDouble(6);
                string picture = reader.GetString(7);
                string title = reader.GetString(8);
                int inventory = reader.GetInt32(9);
                string genre = reader.GetString(12);

                listeProduits.Add(new Book(pcode, picture, type, title, category, inventory, genre, price, authors, pubCo, pubDate));
            }
            DBManager.closeConnection();
            return listeProduits;
        }
        public static List<Book> getBookList()
        {
            String request = "Select * from books";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Book> listeProduits = new List<Book>();

            while (reader.Read())
            {
                string pcode = reader.GetString(0);
                string authors = reader.GetString(1);
                string pubCo = reader.GetString(2);
                DateTime date = reader.GetDateTime(3);
                string pubDate = date.ToString("yyyy-MM-dd");
                string type = reader.GetString(4);
                string category = reader.GetString(5);
                double price = reader.GetDouble(6);
                string picture = reader.GetString(7);
                string title = reader.GetString(8);
                int inventory = reader.GetInt32(9);
                string genre = reader.GetString(12);

                listeProduits.Add(new Book(pcode, picture, type, title, category, inventory, genre, price, authors, pubCo, pubDate));
            }
            DBManager.closeConnection();
            return listeProduits;
        }

        public void save()
        {
            String request = "insert into product values(null, '" + pcode + "', '" + type + "', '" + category + "', " + price + ", '" + picture + "', '" + title + "','" + genre + "', 0, 0, 'inactive', 1 , 20)";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            int lines = DBManager.executeUpdate(request);

            request = "insert into book values('" + pcode + "', '" + authors + "', '" + pubCo + "', '" + pubDate + "');";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            lines = DBManager.executeUpdate(request);
            DBManager.closeConnection();
        }

        public void modify()
        {
            String request = "update product set type='" + type + "', category='" + category + "', price=" + price + ", title='" + title + "'  where pcode='" + pcode + "';" ;
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            int lines = DBManager.executeUpdate(request);

            request = "update book set authors='" + authors + "', pubco='" + pubCo + "', pubdate='" + pubDate + "' where pcode='" + pcode + "';";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            lines = DBManager.executeUpdate(request);
            DBManager.closeConnection();
        }

        public void delete()
        {
            String request = "DELETE book, product FROM book inner join product on product.pcode = book.pcode where book.pcode = '" + pcode + "';";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            int lines = DBManager.executeUpdate(request);
            DBManager.closeConnection();
        }
    }
}