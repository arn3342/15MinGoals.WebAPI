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
        public async Task<User> GetProfile(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                return await ap.GetProfile(email, password);
            }
            return (null);
        }

        public async Task<bool> EditProfile(string userId, Profile profile)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return await ap.EditProfile(userId, profile);
            }
            return false;
        }
    }
}
