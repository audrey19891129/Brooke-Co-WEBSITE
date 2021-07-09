using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Stock_Out
    {
        public int id { get; set; }
        public int id_stock { get; set; }
        public int transaction { get; set; }
        public string type { get; set; }
        public int quantity { get; set; }

        public Stock_Out(int id, int id_stock, int transaction, string type, int quantity)
        {
            this.id = id;
            this.id_stock = id_stock;
            this.transaction = transaction;
            this.type = type;
            this.quantity = quantity;
        }

        public static void Adding(int id_stock,  int quantity)
        {
            string request = "INSERT INTO sql3373608.stock_out VALUES(null,'" + id_stock + "', '" + 10 + "', '" + "pending" + "', '" + 0 + "');";
            DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.executeQuery(request);
        }
    }
}