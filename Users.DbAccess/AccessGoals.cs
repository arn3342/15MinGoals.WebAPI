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
        public IMongoCollection<Progress> progresses;


        private enum ActivityType
        {
            watched,
            completed,
            listened,
            read
        }

        private enum ProgressType
        {
            Beginner,
            Intermidiate,
            Proffesional
        }


        public AccessGoals(string ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
            goals = _dbContext.Db().GetCollection<Goal>(nameof(MongoDbContext.Collection.goals));
            progresses = _dbContext.Db().GetCollection<Progress>(nameof(MongoDbContext.Collection.progress));
        }


        /// <summary>
        /// meyhod to get just goals without activities by Profile_Id 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>returns A tuple of Boolean HasGoal and List of Goal List<Goal></returns>
        public async Task<(bool HasGoal, List<Goal> AllGoals)> GetGoals(string Profile_id)
        {
            var goalfilter = Builders<Goal>.Filter.Eq(x => x.Profile_Id, Profile_id);
            bool HasGoal = false;

            List<Goal> userGoals = await goals.Find(goalfilter)
                                       .Project<Goal>(Builders<Goal>.Projection.Exclude(g => g.Activities))
                                       .ToListAsync();

            if (userGoals.Count > 0)
            {
                HasGoal = true;
                return (HasGoal, userGoals);
            }
            return (HasGoal, null);
        }
        /// <summary>
        /// Creating Goal using Profile_Id and Goal_Title. First find if the the Goal already exist or not then
        /// do the creation
        /// </summary>
        /// <param name="Profile_id"></param>
        /// <param name="Goal_Title"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccessful, bool IsExist, Goal NewGoal)> CreateGoal(string Profile_id, string Goal_Title)
        {
            Goal gl = new Goal();
            bool IsSuccessful = false; bool IsExist = false;

            IsExist = await goals.Find(g => g.Goal_Title == Goal_Title).AnyAsync();

            if (!IsExist)
            {
                gl.Profile_Id = Profile_id;
                gl.Goal_Title = Goal_Title;

                try
                {
                    await goals.InsertOneAsync(gl);
                    IsSuccessful = true;
                }
                catch (Exception)
                {
                    IsSuccessful = false;
                }
                // if isexist false and new Goal creat then it return that
                return (IsSuccessful, IsExist, gl);
            }
            // if isexist true then it return that
            return (IsSuccessful, IsExist, gl);
        }

        /// <summary>
        /// create Activity in Goal documnet as a nested array of Activity with unique activity id 
        /// </summary>
        /// <param name="Goal_Id"></param>
        /// <param name="activity"></param>
        /// <returns>return a bollean if issuccessful or not</returns>
        public async Task<bool> CreateActivity(string Goal_Id, Activity activity)
        {
            bool IsSuccessful = false;
            activity.Activity_Id = ObjectId.GenerateNewId();

            var goalfilter = Builders<Goal>.Filter.Eq(x => x.Goal_Id, new ObjectId(Goal_Id));
            Goal targetGoal = await goals.Find(goalfilter).FirstOrDefaultAsync();
            var updateGoal = Builders<Goal>.Update.Push(g => g.Activities, activity);

            /// update.set for updating particular field
            try
            {
                await goals.UpdateOneAsync(tg => tg.Goal_Id == new ObjectId(Goal_Id), updateGoal);
                IsSuccessful = true;
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
            }
            return (IsSuccessful);
        }

        /// <summary>
        /// get the selected activities by finding the goal and fetch activities with limit and skip
        /// </summary>
        /// <param name="Goal_Id"></param>
        /// <param name="limit"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public async Task<List<Activity>> GetSelectedActivities(string Goal_Id, int limit = 1, int skip = 0)
        {
            var proj = Builders<Goal>.Projection.Slice(g => g.Activities, skip, limit);
            var activites = await goals.Find(g => g.Goal_Id == new ObjectId(Goal_Id))
                .Project<Activity>(proj)
                .ToListAsync();

            return activites;
        }

        public async Task<(bool IsCreated, bool IsUpdated)> CreateUpdateProgress(string Goal_Id, string CurrentCourse_Id = "")
        {

            Progress progress = new Progress();
            progress = await progresses.Find(p => p.Goal_Id == Goal_Id).FirstOrDefaultAsync();

            if (progress == null)
            {
                progress = new Progress();
                progress.Progress_Id = ObjectId.GenerateNewId();
                progress.Goal_Id = Goal_Id;
                progress.CurrentCourse_Id = CurrentCourse_Id;
                progress.Xp_Points = SetGoalPoints(10);
                progress.Goal_Level = SetGoalLevel(progress.Xp_Points);

                await progresses.InsertOneAsync(progress);

                return (true,false);
            }
            else
            {
                progress.CurrentCourse_Id = CurrentCourse_Id;
                progress.Xp_Points = SetGoalPoints(progress.Xp_Points);
                progress.Goal_Level = SetGoalLevel(progress.Xp_Points);

                var updateProgress = Builders<Progress>.Update.Set(up=>up.CurrentCourse_Id , progress.CurrentCourse_Id)
                                                       .Set(up => up.Xp_Points, progress.Xp_Points)
                                                       .Set(up => up.Goal_Level, progress.Goal_Level);

                await progresses.UpdateOneAsync<Progress>(p => p.Progress_Id == progress.Progress_Id, updateProgress);
                return (false,true);
            }
        }

        private int SetGoalPoints(int prevXp)
        {
            int xp = prevXp + 10;
            return xp;
        }

        private string SetGoalLevel(int xp)
        {
            if (xp < 500)
                return nameof(ProgressType.Beginner);
            else if (xp < 1500)
                return nameof(ProgressType.Intermidiate);
            else
                return nameof(ProgressType.Proffesional);
        }
    }
}
