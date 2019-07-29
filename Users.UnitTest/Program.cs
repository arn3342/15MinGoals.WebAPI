using System;
using System.Collections.Generic;
using System.Text;
using Users.DbAccess;
namespace Users.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DbInitializer db = new DbInitializer("mongodb+srv://15MinGoals_Admin:arn33423342@15mincluster0-drbj7.mongodb.net/test?retryWrites=true&w=majority");
            var conn = db.Initialize();
            Console.WriteLine(conn.ListDatabases());
            Console.Read();
        }
    }
}
