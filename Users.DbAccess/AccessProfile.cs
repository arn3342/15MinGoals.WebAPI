using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Models;

namespace Users.DbAccess
{
    public class AccessProfile
    {
        private MongoDbContext _dbContext;
        private IMongoCollection<User> user;
        private AccessUser accessUser;
        private User SelectedUser;

        public AccessProfile(string ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
            user = _dbContext.Db().GetCollection<User>(nameof(MongoDbContext.Collection.user));
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
        public async Task<bool> EditProfile(string email, string password, Profile profile)
        {
            #region Variables
            bool IsUpdateSuccessfull = false;
            var update_profile = Builders<User>.Update;
            var updates = new List<UpdateDefinition<User>>();

            Profile SelectedProfile = await GetProfile(email, password);
            #endregion


            #region Looping through properties of Profile class
            foreach (var property in typeof(Profile).GetProperties())
            {
                var value = property.GetValue(profile);
                var oldValue = property.GetValue(SelectedProfile);

                if (value != oldValue)
                {
                    string field_name = property.Name;
                    updates.Add(update_profile.Set("Profile." + field_name, value));
                }
            }
            #endregion

            #region Updating + catching exception
            try
            {
                await user.UpdateOneAsync(usr => usr.Id == SelectedUser.Id, update_profile.Combine(updates));
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

