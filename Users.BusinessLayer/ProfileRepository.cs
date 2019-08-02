using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Users.DbAccess;
using Users.Models;

namespace Users.BusinessLayer
{
    class ProfileRepository
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
    }
}
