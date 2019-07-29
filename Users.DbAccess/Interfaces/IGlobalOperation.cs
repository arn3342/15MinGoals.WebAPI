using System;
using System.Collections.Generic;
using System.Text;

namespace Users.DbAccess.Interfaces
{
    public interface IGlobalOperation
    {
        void GetOne(object Query);
        void GetAll(object Query);
        void DeleteOne(object Query);
    }
}
