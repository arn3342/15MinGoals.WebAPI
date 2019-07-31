using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
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
        #region An enum to hold the fields of the related collection
        private enum CollectionFields
        {
            email,
            password,
            Profile_Id,
            IsFacebookAuth,
            IsLinkedInAuth
        }
        #endregion

        private MongoClient client;
        private IMongoDatabase Db;
        
        //string ConnectionString = "mongodb+srv://15MinGoals_Admin:arn33423342@15mincluster0-drbj7.mongodb.net/test?retryWrites=true&w=majority";
        public AccessUser(string ConnectionString)
        {
            client = new MongoClient(ConnectionString);
            //getting the database
            Db = client.GetDatabase("15MinGoals_Users");
            var test = GetUser("").Result.IsSuccessful;
            
        }

        /// <summary>
        /// Function to check if user already exists, if login was successful, and returns users' profile.
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>True if user already exists, True if login was successful</returns>
        /// <example>
        /// <code>
        /// bool UserAlreadyExists = GetUser("email").Result.UserExistsl
        /// bool UserAlreadyExists = GetUser("email", "password").Result.IsSuccessful;
        /// User user = GetUser("email", "password").Result.ReturnedUser;
        /// </code>
        /// </example>
        public async Task<(bool UserExists, bool IsSuccessful, User ReturnedUser)> GetUser(string email, string password="")
        {
            #region Variables
            IMongoCollection<User> users = Db.GetCollection<User>("User");
            bool IsExistingUser, IsLoginSuccess; IsExistingUser = IsLoginSuccess = false;
            User user = new User();
            #endregion

            #region Test codes
            //User u = new User();
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
            //var results = user.Find(x => x.Email == "nayan").FirstOrDefault();
            #endregion

            #region Checking user's existance
            BsonDocument filter = new BsonDocument(nameof(CollectionFields.email), email);
            await users.Find(filter).ForEachAsync(document =>
            {
                if (document != null)
                {
                    // User exists
                    user = document;
                    IsExistingUser = true;
                }
            } 
            );
            #endregion

            #region Matching email & password
            if (user.Password == password)
            {
                IsLoginSuccess = true;
            }
            else
            {
                // Not sending back the user's info if the password doesn't match.
                IsLoginSuccess = false;
                user = null;
            }
            #endregion

            return (IsExistingUser, IsLoginSuccess, user);
        }
    }
}