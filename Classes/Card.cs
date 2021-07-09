using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Card
    {
        public int card_id { get; set; }
        public int client_id { get; set; }
        public string type { get; set; }
        public string card_number { get; set; }
        public string security_code { get; set; }
        public string holdername { get; set; }
        public string expiration { get; set; }

        public Card(int card_id, int client_id, string type, string card_number, string security_code, string holdername, string expiration)
        {
            this.card_id = card_id;
            this.client_id = client_id;
            this.type = type;
            this.card_number = card_number;
            this.security_code = security_code;
            this.holdername = holdername;
            this.expiration = expiration;
        }

        public static List<Card> getCardsByClientId(int client_id)
        {
            String request = "Select * from cards where client_id ='" + client_id + "';";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Card> listeCards = new List<Card>();

            while (reader.Read())
            {
                int card_id = reader.GetInt32(0);
                string type = reader.GetString(2);
                string card_number = reader.GetString(3);
                string security_code = reader.GetString(4);
                string holdername = reader.GetString(5);
                DateTime date = reader.GetDateTime(6);
                string expiration = date.ToString("yyyy-MM-dd");
                listeCards.Add(new Card(card_id, client_id, type, card_number, security_code, holdername, expiration));
            }
            DBManager.closeConnection();
            return listeCards;
        }

        public Card(int card_id)
        {
            String request = "Select * from cards where id='" + card_id + "';";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.card_id = card_id;
                this.client_id = reader.GetInt32(1);
                this.type = reader.GetString(2);
                this.card_number = reader.GetString(3);
                this.security_code = reader.GetString(4);
                this.holdername = reader.GetString(5);
                DateTime date = reader.GetDateTime(6);
                this.expiration = date.ToString("yyyy-MM-dd");
            }
            DBManager.closeConnection();
        }

        public void save()
        {
            String request = "insert into client_card values(null, " + client_id + ", '" + type + "', '" + card_number + "', " + security_code + ", '" + holdername + "', '" + expiration + "', 'active', null)";
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void saveTemporary(string delete_on)
        {
            String request = "insert into client_card values(null, " + client_id + ", '" + type + "', '" + card_number + "', " + security_code + ", '" + holdername + "', '" + expiration + "', 'active',  "+ delete_on + ")";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void modify()
        {
            String request = "update client_card set expiration_date='" + expiration + "', holdername='"+holdername+"' where id=" + card_id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void delete()
        {
            String request = "update client_card set status='inactive' where id=" + card_id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }

        public void activate()
        {
            String request = "update client_card set status='active' where id=" + card_id;
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            int lines = DBManager.executeUpdate(request);
        }
    }
}