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

        public async Task<(bool UserExists, bool IsSuccessful,User user)> GetUser(string email, string password = "")
        {
          return await au.GetUser(email, password);
        }

        public async Task<(bool IsSuccessfull, string userId)> CreateUser(User user)
        {
           return await au.CreateUser(user);
        }

        public async Task<bool> EditProfile(User user, User newUser = null)
        {
            return await ap.EditProfile(user, newUser);
        }


    }
}
