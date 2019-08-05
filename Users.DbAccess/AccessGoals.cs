using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Users.Models;

namespace Users.DbAccess
{

    public class AccessGoals
    {
        private MongoDbContext _dbContext;
        private IMongoCollection<Goal> goals;
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
            goals = _dbContext.Db().GetCollection<Goal>(nameof(MongoDbContext.Collection.goals));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(bool, List<Goal>)> GetGoals(string Profile_id)
        {
            Goal gl = new Goal();
            var goalfilter = Builders<Goal>.Filter.Eq(x => x.Profile_Id, Profile_id);

            List<Goal> userGoals = await goals.Find(goalfilter)
                                       .Project<Goal>(Builders<Goal>.Projection.Exclude(g => g.Activities))
                                       .ToListAsync();
            if (userGoals.Count > 0)
                return (true, userGoals);
            return (false, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Profile_id"></param>
        /// <param name="Goal_Title"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccessful ,bool IsExist, Goal)> CreateGoal(string Profile_id, string Goal_Title)
        {
            Goal gl = new Goal();
            gl.Profile_Id = Profile_id;
            gl.Goal_Title = Goal_Title;

            bool IsExist = await goals.Find(g => g.Goal_Title == Goal_Title).AnyAsync();

            if (!IsExist)
            {
                await goals.InsertOneAsync(gl);
            }

            return (true, IsExist, gl);
        }


        public async Task<bool> CreateActivity(string Goal_Id, Activity actvty)
        {
            Goal gl = new Goal();
            Activity activity = new Activity();
            activity.Activity_Id = ObjectId.GenerateNewId();
            activity.Activity_Type = actvty.Activity_Type;
            activity.Content_Id = actvty.Content_Id;
            activity.Date = actvty.Date;
            activity.Time = actvty.Time;
            activity.Time_Invested = actvty.Time_Invested;

            var goalfilter = Builders<Goal>.Filter.Eq(x => x.Goal_Id, new ObjectId(Goal_Id));
            Goal targetGoal = await goals.Find(goalfilter).FirstOrDefaultAsync();
            var updateGoal = Builders<Goal>.Update.Push(g => g.Activities, activity);

            /// update.set for updating particular field

            await goals.UpdateOneAsync<Goal>(tg=>tg.Goal_Id == new ObjectId(Goal_Id), updateGoal);

            return (true);
        }


        public async Task<List<Activity>> GetActivityWithLimitAndSkip(string Goal_Id,int limit, int skip)
        {
            var proj = Builders<Goal>.Projection.Slice(g => g.Activities, skip, limit);
            var activites = await goals.Find(g => g.Goal_Id == new ObjectId(Goal_Id))
                .Project<Activity>(proj)
                .ToListAsync();

            return activites;
        }


    }
}
