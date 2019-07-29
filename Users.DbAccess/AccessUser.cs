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
    public class AccessUser : IGlobalOperation
    {
        MongoClient client;

        public AccessUser(MongoClient cl)
        {
            client = cl;
        }
        public void DeleteAll(object Query)
        {
            
        }

        public void DeleteOne(object Query)
        {
            throw new NotImplementedException();
        }

        public void GetAll(object Query)
        {
            client;
        }

        public void GetOne(object Query)
        {
            //mongodb.getData(Convert.ToInt32(Query));
        }

        public void UpdateAll(object Query)
        {
            throw new NotImplementedException();
        }

        public void UpdateOne(object Query)
        {
           
        }
    }
}
