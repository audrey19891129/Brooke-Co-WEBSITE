using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Employee
    {
        public string title { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public Employee() { }
        public Employee(string username, string password)
        {
            String request = "Select * from brookeandco.employees where username='" + username + "' and password='" + password +"';";
            //DBManager.createConnection("sql3.freemysqlhosting.net", "sql3373608", "sql3373608", "cHKivCByKH", 3306);
            DBManager.createConnection("localhost", "brookeandco", "root", "buzzy2626", 3306);
            DbDataReader reader = DBManager.executeQuery(request);

            while (reader.Read())
            {
                try
                {
                    this.title = reader.GetString(0);
                    this.username = reader.GetString(1);
                    this.password = reader.GetString(2);
                    this.firstname = reader.GetString(3);
                    this.lastname = reader.GetString(4);
                }
                catch (System.Data.SqlTypes.SqlNullValueException)
                {
                    this.title = "0";
                    this.username = "0";
                    this.password = "0";
                    this.firstname = "";
                    this.lastname = "";
                }
            }
            DBManager.closeConnection();
        }


    }
}