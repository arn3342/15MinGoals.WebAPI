using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Users.DbAccess.Interfaces;
using Users.Models;

namespace Users.DbAccess
{
    /// <summary>
    /// A class to access the user's login credentials & profile.
    /// </summary>
    public class AccessUser
    {
        private MongoClient client;
        private IMongoDatabase Db;

        string ConnectionString = "mongodb+srv://15MinGoals_Admin:arn33423342@15mincluster0-drbj7.mongodb.net/test?retryWrites=true&w=majority";
        public AccessUser()
        {
            client = new MongoClient(ConnectionString);
            //getting the database
            Db = client.GetDatabase("15MinGoals_Users");
        }

        public void UserLogin(string email, string pass)
        {
            IMongoCollection<User> user = Db.GetCollection<User>("User");
            User u = new User();
            //u.Email = email;
            //u.Password = pass;



            //u.Email = "nayan";
            //u.Password = "12345";

            //user.InsertOne(u);
            
            //retreving the data from the collection

            //List<User> userList = user.AsQueryable().ToList<User>();
            //foreach (var i in userList)
            //{
            //    Console.WriteLine(i);
            //}

            //query of the collection data
            var results = user.Find(x => x.Email == "nayan").FirstOrDefault();
            Console.WriteLine(results.Email);


            Console.ReadKey();

        }
        

    }
}
