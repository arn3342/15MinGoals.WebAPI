using System;
using System.Collections.Generic;
using System.Text;

namespace Users.DbAccess.Tools
{
    public class ValueChecker
    {
        public static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return ConvertObjectToString(t);
            }
        }
        public static string ConvertObjectToString(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();
        }
    }
}
