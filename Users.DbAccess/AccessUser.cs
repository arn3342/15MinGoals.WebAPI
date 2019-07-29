using System;
using System.Collections.Generic;
using System.Text;
using Users.DbAccess.Interfaces;

namespace Users.DbAccess
{
    /// <summary>
    /// A class to access the user's login credentials & profile.
    /// </summary>
    public class AccessUser : IGlobalOperation
    {
        
        public void DeleteAll(object Query)
        {
            throw new NotImplementedException();
        }

        public void DeleteOne(object Query)
        {
            throw new NotImplementedException();
        }

        public void GetAll(object Query)
        {
            GetOne(2);
        }

        public void GetOne(object Query)
        {
            mongodb.getData(Convert.ToInt32(Query));
        }

        public void UpdateAll(object Query)
        {
            throw new NotImplementedException();
        }

        public void UpdateOne(object Query)
        {
           
        }
    }
}
