using System;
using Users.DbManagement.Manager;

namespace Users.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UsersManager usersManager = new UsersManager();
            var test = usersManager.TestingMongo();
            Console.WriteLine(test);
            Console.Read();
        }
    }
}
