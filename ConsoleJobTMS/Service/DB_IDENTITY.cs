using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleJobTMS.Service
{

    public class DB_IDENTITY
    {
        public static string Server = clsUtility.ReadXML("Server");

        public static string Database = clsUtility.ReadXML("Database");

        public static string User = clsUtility.ReadXML("User");

        public static string Password = clsUtility.ReadXML("Password");


    }
    
}