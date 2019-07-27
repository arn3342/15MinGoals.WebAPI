using System;
using Users.DbManagement.Manager;

namespace Users.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UsersManager usersManager = new UsersManager();
            Console.WriteLine(usersManager.GenereateHashOfPassword("arn33423342").FinalPassword);
            Console.Read();
        }
    }
}
