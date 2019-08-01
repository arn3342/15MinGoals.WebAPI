using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Users.Models;

namespace Users.DbAccess
{

    public class AccessGoals
    {
        private MongoDbContext _dbContext;

        private enum ActivityType
        {
            watched,
            completed,
            listened,
            read
        }


        public AccessGoals(string ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccessful, Goal ReturnedGoal)> LoadGoal(string email, string password = "")
        {

            IMongoCollection<Goal> goal = _dbContext.Db().GetCollection<Goal>(nameof(MongoDbContext.Collection.goals));
            bool IsExistingUser, IsLoginSuccess; IsExistingUser = IsLoginSuccess = false;
            User user = new User();

            /// rest of the code
            return (true, new Goal());
        }

        // Either we get one goal.
        // Or we get all the goals.
    }
}
