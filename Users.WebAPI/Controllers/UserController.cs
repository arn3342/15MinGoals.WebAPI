using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Users.BusinessLayer;
using Users.Models;

namespace Users.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserRepository ur;

        public UserController()
        {
            ur = new UserRepository(Startup.ConnectionString);
        }

        public ActionResult Index()
        {
            return null;
        }
        // GET: api/User
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            User user = new User()
            {
                Email = "nabil3342@gmail.com",
                Password = "arn3342",
                Profile = new Profile
                {
                    First_Name = "Aousaf",
                    Last_Name = "Rashid"
                }
            };
            await ur.EditProfile(user);
            //await ur.GetUser(email: "Imtiyaz");
            //await ur.CreateUser(user);
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public void Get(int id)
        {
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
