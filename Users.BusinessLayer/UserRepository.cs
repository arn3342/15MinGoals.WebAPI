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
        public UserRepository(string conStr)
        {
             au = new AccessUser(conStr);
        }

        public async Task<(bool UserExists, bool IsSuccessful, User ReturnedUser)> GetUser(string email, string password = "")
        {
            await au.GetUser(email,password);
            return (true, true, new User());
        }

    }
}
