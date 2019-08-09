using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Users.DbAccess;
using Users.Models;

namespace Users.BusinessLayer
{
    public class ProfileRepository
    {
        AccessProfile ap;
        public async Task<Profile> GetProfile(User user)
        {
            if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Password))
            {
                var AsyncProfileCallback = await ap.GetProfile(user.Email, user.Password);
                return AsyncProfileCallback.profile;
            }
            return (null);
        }

        public async Task<bool> EditProfile(User user)
        {
            if (!object.Equals(user.Id, default(ObjectId)))
            {
                return await ap.EditProfile(user);
            }
            return false;
        }
    }
}
