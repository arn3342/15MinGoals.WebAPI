using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.DbAccess.Tools;
using Users.Models;

namespace Users.DbAccess
{
    public class AccessProfile
    {
        private MongoDbContext _dbContext;
        private IMongoCollection<User> userCollection;
        private AccessUser accessUser;
        private User SelectedUser;

        public AccessProfile(string ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
            userCollection = _dbContext.Db().GetCollection<User>(nameof(MongoDbContext.Collection.user));
            accessUser = new AccessUser(ConnectionString);
        }

        /// <summary>
        /// This methoe Find out a Specific Profile denpending on the given email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Returns a user object where we have the reference of the profile collection.So we can reterieve 
        /// the profile data from here</returns>
        public async Task<Profile> GetProfile(string email, string password)
        {
            #region variables
            var AsyncUserCall = await accessUser.GetUser(email, password);
            SelectedUser = AsyncUserCall.user;
            #endregion

            return SelectedUser.Profile;
        }

        /// <summary>
        /// Method for updating the profile of any user after successfully registration to the system.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="profile"></param>
        /// <returns> Return a boolean value that represents wheater the update is successfull or not.</returns>
        public async Task<bool> EditProfile(User user)
        {
            
            Profile SelectedProfile = await GetProfile(user.Email, user.Password);

            bool IsUpdateSuccessfull = false;
            #region Updating + catching exception
            try
            {
                FilterOperations filterOperations = new FilterOperations();
                await userCollection.UpdateOneAsync(usr => usr.Id == SelectedUser.Id, filterOperations.BuildUpdateFilter<User>(user, SelectedProfile, user.Profile));
                IsUpdateSuccessfull = true;
            }
            catch (Exception e)
            {
                IsUpdateSuccessfull = false;
            }
            #endregion
            return IsUpdateSuccessfull;
        }
    }
}