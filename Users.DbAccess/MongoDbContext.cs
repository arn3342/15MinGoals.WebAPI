using System;
using MongoDB.Driver;

namespace Users.DbAccess
{
    public class MongoDbContext
    {
        private MongoClient client;
        //private IMongoDatabase Db;

        public MongoDbContext(string conStr)
        {
            client = new MongoClient(conStr);
        }

        public IMongoDatabase Db()
        {
           return client.GetDatabase("15MinGoals_Users");
        }

        public enum Collection
        {
            user,
            goals
        }
        
    }
}
