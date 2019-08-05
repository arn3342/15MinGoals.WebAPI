using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Users.Models;

namespace Users.DbAccess
{
    public class AccessProfile
    {
        private MongoDbContext _dbContext;
        private IMongoCollection<User> user;

        public AccessProfile(string ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
            user = _dbContext.Db().GetCollection<User>(nameof(MongoDbContext.Collection.user));
        }

        /// <summary>
        /// This methoe Find out a Specific Profile denpending on the given email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Returns a user object where we have the reference of the profile collection.So we can reterieve 
        /// the profile data from here</returns>
        public async Task <User> GetProfile(string email,string password)
        {
            #region
            bool IsExisting = false;
            User u = new User();
            Hashing hs = new Hashing();
            #endregion

            var filterUser = Builders<User>.Filter.Eq(x=> x.Email,email);
            await user.Find(filterUser).ForEachAsync(document =>
            {
                if(document != null)
                {
                    u = document;
                    IsExisting = true;
                }
            }
            );
            
            if(IsExisting)
            {
               var verifiedPass= hs.ValidatePassword(password, u.Password);
                if(!verifiedPass)
                {
                    //error message(need to discuss wheateher it should return a http status code like 201,404 or string error message)
                }
            }
            return u;


        }

        /// <summary>
        /// Method for updating the profile of any user after successfully registration to the system.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="profile"></param>
        /// <returns> Return a boolean value that represents wheater the update is successfull or not.</returns>

        public async Task<bool> EditProfile(string userId, Profile profile)
        {
            bool IsUpdateSuccessfull = false;
            Profile p = new Profile();

            p.First_Name = profile.First_Name;
            p.Last_Name = profile.Last_Name;
            p.Work_Institute = profile.Work_Institute;
            p.Default_Credential = profile.Default_Credential;
            p.Country = profile.Country;
            p.Education_Institute = profile.Education_Institute;
            p.Tour_Complete = profile.Tour_Complete;

            var UserFilter = Builders<User>.Filter.Eq(x => x.Id, new ObjectId(userId));
            User SelectedUser = await user.Find(UserFilter).FirstOrDefaultAsync();
            var update_profile = Builders<User>.Update.Set(g => g.Profile, p);

            try
            {
                await user.UpdateOneAsync<User>(usr => usr.Id == new ObjectId(userId),update_profile);
                IsUpdateSuccessfull = true;
            }
            catch(Exception e)
             {
                IsUpdateSuccessfull = false;
            }

            return IsUpdateSuccessfull;

        }




        }
    }

