using System.Diagnostics;
using Users.DbManagement.Models;
using BCrypt;
using MongoDB;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;

namespace Users.DbManagement.Manager
{
    public class UsersManager
    {
        /// <summary>
        /// <para>This method adds a new user. Make sure to pass a <paramref name="user"/> model. </para>
        /// <para>This method calls the GenerateHashOfPassword method to get the hash of the password.</para>
        /// </summary>
        /// <param name="user"></param>
        /// <example>
        /// 
        /// </example>
        public void AddUser (User user)
        {

        }
        /// <summary>
        /// Bla bla bla
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The name of the user</returns>
        public string DeleteUser (User user)
        {
            return null;
            //We delete the user
        }
        public void EditUser (User user)
        {
            //We edit the user
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawPassword"></param>
        /// <returns>The value of X</returns>
        public (string TimeTaken, string FinalPassword) GenereateHashOfPassword (string rawPassword)
        {
            var cost = 6;
            var sw = Stopwatch.StartNew();
            string finalPass = BCrypt.Net.BCrypt.HashPassword(rawPassword, workFactor: cost);
            sw.Stop();

            var timeTaken = sw.ElapsedMilliseconds;

            return (timeTaken.ToString(), finalPass);
        }

        public string TestingMongo()
        {
            return null;
        }
    }
}