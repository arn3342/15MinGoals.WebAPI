using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Users.BusinessLayer;
using Users.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<string>> GetGoals(string Profile_Id)
        {
            await goalRepository.GetGoals(Profile_Id= "5d41eab7956d543e2c31e8a1");
            return null;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(int profile_id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async void CreateGoal(string Profil_Id, string Goal_Title)
        {
            await goalRepository.CreateGoal("5d41eab7956d543e2c31e8a1", "Become a Jason Bourn");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }



        #region Activity
        [HttpPost]
        public async void CreateActivity(string Goal_Id,Activity act)
        {
            await goalRepository.CreateActivity(Goal_Id,act);
        }
        [HttpGet, Route(template: "GetSelectedActivities/{Goal_Id}/{limit}/{skip}")]
        public async Task<List<Activity>> GetSelectedActivities(string Goal_Id, int limit, int skip)
        {
            return await GetSelectedActivities(Goal_Id, limit, skip);
        }


        #endregion

    }
}
//{"_id":{"$oid":"5d41eab7956d543e2c31e8a1"},
//"Email":"Imtiyaz",
//"Password":"123456",
//"Profile":null,
//"IsFacebookAuth":false,
//"IsLinkedInAuth":false,
//"Reference_Ids":null,
//"Post_Ids":null}