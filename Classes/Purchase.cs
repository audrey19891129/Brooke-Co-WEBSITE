using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Purchase
    {
        public int id { get; set; }
        public int employee_id { get; set; }
        public string status  { get; set; }

        public Purchase() { }

        public Purchase(int id, int employee_id, string status)
        {
            this.id = id;
            this.employee_id = employee_id;
            this.status = status;
        }

        public Purchase(int employee_id)
        {
            this.employee_id = employee_id;
        }

        public static List<Purchase> GetPurchases()
        {
            string request = "SELECT * FROM sql3373608.purchase";
            DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Purchase> list = new List<Purchase>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int employee_id = reader.GetInt32(1);
                string status = reader.GetString(2);

                Purchase ps = new Purchase(id, employee_id, status);
                list.Add(ps);
            }
            DBManager.closeConnection();
            return list;
        }

        public void Adding(int employee_id)
        {
            string request = "INSERT INTO sql3373608.purchase VALUES(null,'" + employee_id + "', 'add')";
            DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            int nbrLine = DBManager.executeUpdate(request);
            if (nbrLine < 0)
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