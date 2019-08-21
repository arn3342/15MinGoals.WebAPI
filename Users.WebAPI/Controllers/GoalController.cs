using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Users.BusinessLayer;
using Users.Models;

namespace Users.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GoalController : Controller
    {
        GoalRepository goalRepository;
        public GoalController()
        {
            goalRepository = new GoalRepository(Startup.ConnectionString);
        }

        #region Goal
        [HttpGet("GetGoals")]
        public async Task<(ActionResult<bool>, IEnumerable<Goal>)> GetGoals(string Profile_Id)
        {
            (bool IsSuccessful, List<Goal> ReturnedGoal) result = await goalRepository.GetGoals(Profile_Id);
            if (result.IsSuccessful)
            {
                return (true, result.ReturnedGoal);
            }
            return (StatusCode(400, "No goals found"), null);
        }

        
        [HttpPost("CreateGoal")]
        public async void CreateGoal([FromBody]BuildGoal builder)
        {
            await goalRepository.CreateGoal(builder.Profile_Id, builder.Goal_Title);
        }
        #endregion

        #region Activity   

        [HttpGet("GetSelectedActivities")]
        public async Task<(ActionResult<bool>, Goal)> GetSelectedActivities(string Goal_id, int limit, int skip)
        {
            Goal result = await goalRepository.GetSelectedActivities("5d5aca9f6411583eccad3e0b", 1, 1);
            if (result != null)
            {
                return (StatusCode(201), result);
            }
            return (StatusCode(400, "No activities yet!"), null);
        }

        [HttpPost("CreateActivity")]
        public async void CreateActivity([FromBody]BuildActivity buildActivity)
        {
            await goalRepository.CreateActivity(buildActivity.Goal_Id, buildActivity.activity);
        }
        #endregion

        #region Progress
        [HttpPost("CreateOrUpdateProgress")]
        public async Task<(bool IsCreated, bool IsUpdated)> CreateOrUpdateProgress([FromBody]BuildGoalProgress buildGoalProgress)
        {
            return await goalRepository.CreateUpdateProgress(buildGoalProgress.Goal_Id, buildGoalProgress.CurrentCourse_Id);
        }

        [HttpGet("GetProgressOfGoal")]
        public async Task<(ActionResult<bool> HasProgress, Progress)> GetProgressOfGoal(string Goal_Id)
        {
            var result = await goalRepository.GetProgressOfGoal(Goal_Id);
            if (result != null)
            {
                return (StatusCode(200), result);
            }
            return (StatusCode(404, "No progress yet!"), result);
        }
        #endregion

        #region Required classes for FromBody
        public class BuildGoal
        {
            public string Profile_Id { get; set; }
            public string Goal_Title { get; set; }
        }

        public class BuildGoalProgress
        {
            public string Goal_Id { get; set; }
            public string CurrentCourse_Id { get; set; }
        }

        public class BuildActivity
        {
            public string Goal_Id { get; set; }
            public Activity activity { get; set; }
        }
        #endregion
    }
}
