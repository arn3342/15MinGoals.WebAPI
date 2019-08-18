using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Users.DbAccess.Tools;
using Users.Models;

namespace Users.DbAccess
{
    /// <summary>
    /// A class to access/create/modify a user's goals.
    /// </summary>
    public class AccessGoals
    {
        /// <summary>A <see cref="MongoDbContext"/>to connect to the database.</summary>
        private MongoDbContext _dbContext;
        /// <summary>A <see cref="IMongoCollection{Goal}"/> to hold retrieved <see cref="Goal"/>s</summary>
        private IMongoCollection<Goal> goals;
        /// <summary>A <see cref="IMongoCollection{Progress}"/> to hold retrieved <see cref="Progress"/>s</summary>
        public IMongoCollection<Progress> progresses;


        private enum ActivityType
        {
            /// <summary>Defines that the user has watched a video.</summary>
            watched,
            /// <summary>Defines that the user has completed a test.</summary>
            completed,
            /// <summary>Defines that the user has listened to an audiobook.</summary>
            listened,
            /// <summary>Defines that the user has read an article/e-book.</summary>
            read
        }

        private enum ProgressType
        {
            /// <summary>Defines the user's progress of a particular goal as beginner</summary>
            Beginner,
            /// <summary>Defines the user's progress of a particular goal as intermediate</summary>
            Intermediate,
            /// <summary>Defines the user's progress of a particular goal as professional</summary>
            Professional
        }

        /// <summary>
        /// <para>The constructor.</para> 
        /// <para>Initializes the class, creates a <see cref="MongoDbContext"/>, returns a <see cref="IMongoCollection{Goal}"/> and a <see cref="IMongoCollection{Progress}"/></para>
        /// </summary>
        /// <param name="ConnectionString">The connectionstring to connect to the database.</param>
        public AccessGoals(string ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
            goals = _dbContext.Db().GetCollection<Goal>(nameof(MongoDbContext.Collection.goals));
            progresses = _dbContext.Db().GetCollection<Progress>(nameof(MongoDbContext.Collection.progress));
        }

        /// <summary>
        /// An asynchronous method to retrieve all goals of a user.
        /// </summary>
        /// <param name="Profile_id">The unique identifier of a user's profile</param>
        /// <returns>Returns true if user has goals, <see cref="List{Goal}"/> containing all goals./returns>
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
        /// An asynchronous method to create a new goal of a user. 
        /// </summary>
        /// <param name="Profile_id">The unique identifier of a user's profile</param>
        /// <param name="Goal_Title">The title of the goal</param>
        /// <returns>Returns true if creation was successful, true if goal already exists, <see cref="Goal"/> as the newly created goal.</returns>
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
        /// An asynchronous method to create a new activity of a goal.
        /// </summary>
        /// <param name="Goal_Id">The unique identifier of the goal</param>
        /// <param name="activity">The <see cref="Activity"/> class</param>
        /// <returns>Returns true if the cration/insertion was successful.</returns>
        public async Task<bool> CreateActivity(string Goal_Id, Activity activity)
        {
            bool IsSuccessful = false;
            activity.Activity_Id = ObjectId.GenerateNewId();

            var goalfilter = Builders<Goal>.Filter.Eq(x => x.Goal_Id, new ObjectId(Goal_Id));
            Goal targetGoal = await goals.Find(goalfilter).FirstOrDefaultAsync();

            try
            {
                await goals.UpdateOneAsync(tg => tg.Goal_Id == new ObjectId(Goal_Id), FilterOperations.BuildUpdateFilter<Goal>(targetGoal, Child: activity, ArrayProperty: targetGoal.Activities));
                IsSuccessful = true;
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
            }
            return (IsSuccessful);
        }

        /// <summary>
        /// An asynchronous method to get selected activities related to a goal
        /// </summary>
        /// <param name="Goal_Id">The unique identifier of the goal</param>
        /// <param name="limit">The number of activities to return</param>
        /// <param name="skip">The total number of previously returned activities</param>
        /// <returns>Returns a <see cref="List{Activity}"/> containing all activities.</returns>
        public async Task<List<Activity>> GetSelectedActivities(string Goal_Id, int limit = 1, int skip = 0)
        {
            var proj = Builders<Goal>.Projection.Slice(g => g.Activities, skip, limit);
            var activites = await goals.Find(g => g.Goal_Id == new ObjectId(Goal_Id))
                .Project<Activity>(proj)
                .ToListAsync();

            return activites;
        }

        /// <summary>
        /// An asynchronous method to create/update the progress of a particular goal.
        /// </summary>
        /// <param name="Goal_Id">The unique identifier of the goal</param>
        /// <param name="CurrentCourse_Id">The unique identifier of the currently following course(optional)</param>
        /// <returns>Returns true if the creation was successful, true if updating was successful</returns>
        public async Task<(bool IsCreated, bool IsUpdated)> CreateOrUpdateProgress(string Goal_Id, string CurrentCourse_Id = "")
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


        public async Task<Progress> GetProgressOfGoal(string Goal_Id)
        {
            return await progresses.Find(p => p.Goal_Id == Goal_Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// A function to calculate the points of a goal.
        /// </summary>
        /// <param name="prevXp">Current xp poins of the goal</param>
        /// <returns>Returns total points as an <see cref="int"/></returns>
        private int SetGoalPoints(int prevXp)
        {
            int xp = prevXp + 10;
            return xp;
        }

        /// <summary>
        /// A function to determine/set a goal's level.
        /// </summary>
        /// <param name="xp">Current xp points of the goal</param>
        /// <returns>Returns the level of the goal as a <see cref="string"/></returns>
        /// <example>
        /// <code>
        /// int CurrentXp = 1200;
        /// string GoalLevel = SetGoalLevel(CurrentXp);
        /// </code>
        /// </example>
        private string SetGoalLevel(int xp)
        {
            if (xp < 500)
                return nameof(ProgressType.Beginner);
            else if (xp < 1500)
                return nameof(ProgressType.Intermediate);
            else
                return nameof(ProgressType.Professional);
        }
    }
}
