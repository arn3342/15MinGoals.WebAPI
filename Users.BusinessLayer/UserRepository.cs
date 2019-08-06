using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Users.DbAccess;
using Users.Models;

namespace Users.BusinessLayer
{
    public class UserRepository
    {
        AccessUser au;
        AccessProfile ap;
        public UserRepository(string conStr)
        {
             au = new AccessUser(conStr);
            ap = new AccessProfile(conStr);
        }

        public async Task<(bool UserExists, bool IsSuccessful)> GetUser(string email, string password = "")
        {
            await au.GetUser(email,password);
            return (true, true);
        }

        public async Task<(bool IsSuccessfull, string userId)> CreateUser(string email, string password)
        {
            await au.CreateUser(email, password);
            return (true, "hello");
        }

        public async Task<bool> EditProfile(string userId, Profile profile)
        {
            return await ap.EditProfile(userId, profile);
        }


    }
}
