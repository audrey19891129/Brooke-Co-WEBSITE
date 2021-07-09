using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationBrookeAndCo.Classes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DBManager.createConnection();
                Console.WriteLine("connected");
            }
            catch(Exception ex)
            {
                Console.WriteLine("error");
            }
        }
    }
}