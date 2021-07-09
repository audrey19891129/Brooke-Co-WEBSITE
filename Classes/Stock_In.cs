using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Stock_In
    {
        public int id { get; set; }
        public string pcode { get; set; }
        public int provider_id { get; set; }
        public int purchase_id { get; set; }
        public int bought_quantity { get; set; }
        public double unit_price_bought { get; set; }
        public string reveived_date { get; set; }
        public int left_quantity { get; set; }

        public Stock_In() { }

        public Stock_In(int id, string pcode, int provider_id, int purchase_id, int bought_quantity, double unit_price_bought, string reveived_date, int left_quantity)
        {
            this.id = id;
            this.pcode = pcode;
            this.provider_id = provider_id;
            this.purchase_id = purchase_id;
            this.bought_quantity = bought_quantity;
            this.unit_price_bought = unit_price_bought;
            this.reveived_date = reveived_date;
            this.left_quantity = left_quantity;
        }

        public Stock_In(string pcode, int purchase_id, double unit_price_bought)
        {
            this.pcode = pcode;
            this.purchase_id = purchase_id;
            this.unit_price_bought = unit_price_bought;
        }

        public void Adding(string pcode, int purchase_id, double unit_price_bought)
        {
            string request = "INSERT INTO sql3373608.stock_in VALUES(null,'" + pcode + "', '" + 10 + "', '" + purchase_id + "', '" + 0 +"', '" +
                unit_price_bought + "'. '" + "'2020-01-01','" + 0 + "');";
            DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
           int nbrLine = DBManager.executeUpdate(request);
            if(nbrLine < 0)
            {
                request = "SELECT LAST_INSERTid() AS id";
                DbDataReader reader = DBManager.executeQuery(request);
                while (reader.Read())
                {
                    this.id = reader.GetInt32(0);
                }
            }
            DBManager.closeConnection();
        }

    }
}