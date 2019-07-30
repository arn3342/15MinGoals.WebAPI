using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Users.DbAccess.Interfaces;

namespace Users.DbAccess
{
    /// <summary>
    /// A class to access the user's login credentials & profile.
    /// </summary>
    public class AccessUser
    {
        private MongoClient client;

        public AccessUser(string ConnectionString)
        {
            client = new MongoClient(ConnectionString);
        }
    }
}
