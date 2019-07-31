using System;
using System.Collections.Generic;
using System.Text;
using Users.DbAccess;

namespace Users.BusinessLayer
{
    public class UserRepository
    {
        public UserRepository(string conStr)
        {
            AccessUser au = new AccessUser(conStr);
        }
    }
}
