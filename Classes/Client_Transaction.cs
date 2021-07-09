using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Client_Transaction : Transaction
    {
        public int order_id { get; set; }

        public Client_Transaction(int id, int card_id, string transaction_confirmation, string status, string validation, double amount, int order_id, string date)
        {
            this.id = id;
            this.card_id = card_id;
            this.transaction_confirmation = transaction_confirmation;
            this.status = status;
            this.validation = validation;
            this.amount = amount;
            this.order_id = order_id;
            this.date = date;
        }

        public Client_Transaction(int id)
        {
            String request = "Select * from clients_transactions where id=" + id + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.id = reader.GetInt32(0);
                this.card_id = reader.GetInt32(1);
                this.transaction_confirmation = reader.GetString(2);
                this.status = reader.GetString(3);
                this.validation = reader.GetString(4);
                this.amount = reader.GetFloat(5);
                this.order_id = reader.GetInt32(6);
                DateTime D = reader.GetDateTime(7);
                this.date = D.ToString("yyyy-MM-dd");
            }
        }

        public override void save()
        {
            base.save();
            String request = "select last_insert_id()";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            int last = 0;
            while (reader.Read())
            {
                this.id = reader.GetInt32(0);
                last = reader.GetInt32(0);
            }
            request = "insert into transaction_client values(" + last + ", " + order_id + ")";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }
    }
}