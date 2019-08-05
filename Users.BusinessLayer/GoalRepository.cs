using System;
using System.Threading.Tasks;
using Users.DbAccess;
using Users.Models;
using System.Collections.Generic;


namespace Users.BusinessLayer
{
    public class GoalRepository
    {
        AccessGoals ag;

        public GoalRepository(string conStr)
        {
            ag = new AccessGoals(conStr);
        }


        public async Task<(bool IsSuccessful, List<Goal> ReturnedGoal)> GetGoals(string Profile_id)
        {
            if (!string.IsNullOrEmpty(Profile_id))
            {
                return await ag.GetGoals(Profile_id);
            }
            return (false, null);
        }

        public async Task<(bool IsSuccessful, bool IsExist, Goal)> CreateGoal(string Profile_id, string Goal_Title)
        {
            if(!string.IsNullOrEmpty(Profile_id) && !string.IsNullOrEmpty(Goal_Title))
            {
                return await ag.CreateGoal(Profile_id, Goal_Title);
            }
            return (false, false, null);
        }

        public async Task<bool> CreateActivity(string Goal_Id, Activity act)
        {
            return await ag.CreateActivity(Goal_Id,act);
        }

        public async Task<List<Activity>> GetSelectedActivities(string Goal_Id, int limit, int skip)
        {
            return await ag.GetSelectedActivities(Goal_Id, limit, skip);
        }


        #region Progress

        public async Task<(bool IsCreated, bool IsUpdated)> CreateUpdateProgress(string Goal_Id, string CurrentCourse_Id)
        {
            return await ag.CreateUpdateProgress(Goal_Id, CurrentCourse_Id);
        }


        #endregion


    }
}
