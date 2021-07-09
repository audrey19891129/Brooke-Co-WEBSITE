using System;
using System.Collections.Generic;
using System.Data.Common;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Transaction
    {
        public int id { get; set; }
        public int card_id { get; set; }
        public string transaction_confirmation { get; set; }
        public string status { get; set; }
        public string validation { get; set; }
        public double amount { get; set; }
        public string date { get; set; }

        public Transaction(int id, int card_id, string transaction_confirmation, string status, string validation, double amount, string date)
        {
            this.id = id;
            this.card_id = card_id;
            this.transaction_confirmation = transaction_confirmation;
            this.status = status;
            this.validation = validation;
            this.amount = amount;
            this.date = date;
        }

        public Transaction() { }

        public Transaction(int id)
        {
            String request = "Select * from transactions where id=" + id + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.id = id;
                this.card_id = card_id;
                this.transaction_confirmation = transaction_confirmation;
                this.status = status;
                this.validation = validation;
                this.amount = amount;
                this.date = date;
            }
        }

        public virtual void save()
        {
            String request = "insert into transaction values(null, " + card_id + ", '" + transaction_confirmation + "', '" + status + "', '" + validation + "', " + amount + ", '" + date + "')";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }
    }
}