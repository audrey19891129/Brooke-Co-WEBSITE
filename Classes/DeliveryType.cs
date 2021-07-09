using System;
using System.Collections.Generic;
using System.Data.Common;

namespace WebApplicationBrookeAndCo.Classes
{
    public class DeliveryType
    {
        public int id { get; set; }
        public string name { get; set; }
        public double fixed_rate { get; set; }
        public int local_days { get; set; }
        public int overseas_days { get; set; }

        public DeliveryType(int id, string name, double fixed_rate, int local_days, int overseas_days)
        {
            this.id = id;
            this.name = name;
            this.fixed_rate = fixed_rate;
            this.local_days = local_days;
            this.overseas_days = overseas_days;
        }

        public DeliveryType(int id)
        {
            String request = "Select * from services where id=" + id + ";";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                this.id = reader.GetInt32(0);
                this.name = reader.GetString(1);
                this.fixed_rate = reader.GetFloat(2);
                this.local_days = reader.GetInt32(3);
                this.overseas_days = reader.GetInt32(4);
            }
        }

        public static List<DeliveryType> getDeliveryTypeList()
        {
            String request = "Select * from services";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);
            List<DeliveryType> listServices = new List<DeliveryType>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                double fixed_rate = reader.GetFloat(2);
                int local_days = reader.GetInt32(3);
                int overseas_days = reader.GetInt32(4);
                listServices.Add(new DeliveryType(id, name, fixed_rate, local_days, overseas_days));
            }
            return listServices;
        }
    }
}