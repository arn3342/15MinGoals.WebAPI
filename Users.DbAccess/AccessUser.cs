﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Users.DbAccess.Interfaces;
using Users.Models;
using Users.DbAccess.Tools;

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
        private IMongoCollection<User> users;
        Hashing hs;
        #endregion

        public AccessUser(string ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
            users = _dbContext.Db().GetCollection<User>(nameof(MongoDbContext.Collection.user));
            hs = new Hashing();
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
        public async Task<(bool UserExists, bool IsSuccessful, User user)> GetUser(string email, string password = "")
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

                    // Returing user if email and password matches
                    if (password != null && hs.ValidatePassword(password, document.Password))
                    {
                        user = document;
                        IsLoginSuccess = true;
                    }
                }
                else
                {
                    user = null;
                }
            }
            );
            #endregion

            return (IsExistingUser, IsLoginSuccess, user);
        }


        /// <summary>
        /// Method for creating User and Profile.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Return bool value representing successfully creation of the user and the userId</returns>

        public async Task<(bool IsSuccessfull, string userId)> CreateUser(User user)
        {
            var AsyncUserCall = await GetUser(user.Email);
            bool userExists = AsyncUserCall.UserExists;

            if (userExists)
            {
                return (false, null);
            }
            else
            {
                user.Id = ObjectId.GenerateNewId();
                user.Password = hs.HashPassword(user.Password);
                user.Profile.Profile_Id = ObjectId.GenerateNewId();

                try
                {
                    //inserting data to the users collection
                    await users.InsertOneAsync(user);
                    return (true, user.Id.ToString());
                }
                catch (Exception ex)
                {
                    return (false, null);
                }
            }
        }
    }
}