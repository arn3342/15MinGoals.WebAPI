using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

        #region Global Variables
        private MongoDbContext _dbContext;
        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private IMongoCollection<User> users;
        #endregion

        //string ConnectionString = "mongodb+srv://15MinGoals_Admin:arn33423342@15mincluster0-drbj7.mongodb.net/test?retryWrites=true&w=majority";
        public AccessUser(string ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
            users = _dbContext.Db().GetCollection<User>(nameof(MongoDbContext.Collection.user));

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
        public async Task<(bool UserExists, bool IsSuccessful)> GetUser(string email, string password = "")
        {
            #region Variables
            bool IsExistingUser, IsLoginSuccess; IsExistingUser = IsLoginSuccess = false;
            User user = new User();
            #endregion

            #region Checking user's existance
            var filter = Builders<User>.Filter.Eq(x => x.Email, email);

            await users.Find(filter).ForEachAsync(document =>
            {
                if (document != null)
                {
                    // User exists
                    IsExistingUser = true;
                    user = document;
                    autoResetEvent.Set();
                }
            }
            );
            autoResetEvent.WaitOne();

            #endregion
            Hashing hs = new Hashing();

            //checking wheather the password matches with the hashed password
            bool isPassWordVerified = hs.ValidatePassword(password, user.Password);

            #region Matching email & password
            if(IsExistingUser && isPassWordVerified)
            {
                IsLoginSuccess = true;
            }
            else
            {
                IsLoginSuccess = false;
            }

            #endregion

            return (IsExistingUser, IsLoginSuccess);
        }


        /// <summary>
        /// Method for creating User and Profile.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Return bool value representing successfully creation of the user and the userId</returns>

        public async Task<(bool IsSuccessfull,string userId)> CreateUser(string email,string password)
        {
            bool IsUserCreated = false;
            bool IsEmailExists = false;
            User user = new User();
            Profile profile = new Profile();
            Hashing hs = new Hashing();


            //checking wheather the email already exists or not.
            var filter = Builders<User>.Filter.Eq(x => x.Email, email);
            await users.Find(filter).ForEachAsync(document =>
            {
                if(document != null)
                {
                    //if email exists
                    IsEmailExists = true;
                }
            });

            if (!IsEmailExists)
            {
                user.Id = ObjectId.GenerateNewId();
                user.Email = email;
                user.Password = hs.HashPassword(password);
                profile.Profile_Id = ObjectId.GenerateNewId();
                //creating reference to the profile collection
                user.Profile = profile;

                try
                {
                    //inserting data to the users collection
                    await users.InsertOneAsync(user);
                    autoResetEvent.Set();
                    IsUserCreated = true;
                    autoResetEvent.WaitOne();

                }
                catch (Exception ex)
                {
                    IsUserCreated = false;
                }
                return (IsUserCreated, user.Id.ToString());
            }
            return (IsUserCreated,null);
        }
    }
}