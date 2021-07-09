using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApplicationBrookeAndCo.v2
{
    public class Product2
    {
        public int id { get; set; }
        public string pcode { get; set; }
        public string picture { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public int inventory { get; set; }
        public string genre { get; set; }
        public double price { get; set; }

        public Product2(int Id, string Pcode, string Picture, string Type, string Title, string Category, int Inventory, string Genre, double Price)
        {
            id = Id;
            pcode = Pcode;
            picture = Picture;
            type = Type;
            title = Title;
            category = Category;
            inventory = Inventory;
            genre = Genre;
            price = Price;
        }

        public Product2()
        {
        }
    }

    public class Book2 : Product2
    {
        public static string authors { get; set; }
        public static string pubCo { get; set; }
        public static string pubDate { get; set; }
        
        public Book2(string Authors, string Pubco, String Pubdate, int Id, string Pcode, string Picture, string Type, string Title, string Category, int Inventory, string Genre, double Price) : base( Id,  Pcode,  Picture,  Type,  Title,  Category,  Inventory,  Genre,  Price)
        {
            authors = authors;
            pubCo = Pubco;
            pubDate = Pubdate;
        }
    }
}