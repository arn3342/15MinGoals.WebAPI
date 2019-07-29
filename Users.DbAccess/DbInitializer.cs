using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Users.DbAccess
{
    public class DbInitializer
    {
        string ConnectionString = "";
        public DbInitializer(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        public MongoClient Initialize()
        {
            MongoClient db = new MongoClient(ConnectionString);
            return db;
        }
    }

}
