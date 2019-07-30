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
            AccessUser au = new AccessUser();
            au.UserLogin(null, null);

        }
    }
}
