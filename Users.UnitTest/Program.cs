using System;
using System.Collections.Generic;
using System.Text;
using Users.DbAccess;
using Users.WebAPI;
using Users.WebAPI.Controllers;

namespace Users.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AccessUser au = new AccessUser(Startup.ConnectionString);
            au.UserLogin(null, null);

        }
    }
}
