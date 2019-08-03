﻿using System;
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
        /// meyhod to get just goals without activities by Profile_Id 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>returns A tuple of Boolean HasGoal and List of Goal List<Goal></returns>
        public async Task<(bool HasGoal, List<Goal> AllGoals)> GetGoals(string Profile_id)
        {
            Goal gl = new Goal();
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
        /// <param name="actvty"></param>
        /// <returns>return a bollean if issuccessful or not</returns>
        public async Task<bool> CreateActivity(string Goal_Id, Activity actvty)
        {
            bool IsSuccessful = false;

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
            try
            {
                await goals.UpdateOneAsync<Goal>(tg => tg.Goal_Id == new ObjectId(Goal_Id), updateGoal);
                IsSuccessful = true;
            }
            catch (Exception)
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


    }
}
