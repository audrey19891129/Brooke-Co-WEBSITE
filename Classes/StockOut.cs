using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;

namespace WebApplicationBrookeAndCo.Classes
{
    public class StockOut
    {
        public int id { get; set; }
        public int stock_id { get; set; }
        public int transaction_id { get; set; }
        public string type { get; set; }
        public int quant { get; set; }

        public StockOut(int id, int stock_id, int transaction_id, string type, int quant)
        {
            this.id = id;
            this.stock_id = stock_id;
            this.transaction_id = transaction_id;
            this.type = type;
            this.quant = quant;
        }

        public static List<StockOut> getStockOutList()
        {
            String request = "Select * from stocks_out";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<StockOut> list = new List<StockOut>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int stock_id = reader.GetInt32(1);
                int transaction_id = reader.GetInt32(2);
                string type = reader.GetString(3);
                int quant = reader.GetInt32(4);
                list.Add(new StockOut(id, stock_id, transaction_id, type, quant));
            }
            DBManager.closeConnection();
            return list;
        }

        public void save()
        {
            String request = "insert into stock_out values(null, " + stock_id + ", " + transaction_id + ", '" + type + "', " + quant + ")";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }
    }
}