using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Provider
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string adress { get; set; }

        public Provider() { }

        public Provider(int id, string name, string phone, string adress)
        {
            this.id = id;
            this.name = name;
            this.phone = phone;
            this.adress = adress;
        }

        public List<Provider> GetProviders()
        {
            string request = "SELECT * FROM sql3373608.provider";
            DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<Provider> list = new List<Provider>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string phone = reader.GetString(2);
                string adress = reader.GetString(3);

                Provider pv = new Provider(id, name, phone, adress);
                list.Add(pv);
            }
            DBManager.closeConnection();
            return list;
        }

        public void AddProvider(string name, string phone, string adress)
        {
            string request = "INSERT INTO sql3373608.provider VALUES(null,'" + name + "', '" + phone + "', '" + adress + "')";
            DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.executeQuery(request);
        }
    }
}