using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Users.BusinessLayer;
using Users.Models;

namespace Users.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserRepository ur;
        ProfileRepository pr;

        public UsersController()
        {
            ur = new UserRepository(Startup.ConnectionString);
            pr = new ProfileRepository(Startup.ConnectionString);
        }

        /// <summary>
        /// A method for the Rest Api GetUser Call.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet("GetUser")]
        public async Task<ActionResult<(bool UserExists, bool IsSuccessful, User user)>> GetUser(string email, string password)
        {
            if (email != null && password != null)
            {
                var entity = await ur.GetUser(email, password);
                return StatusCode(200, entity);
            }
            return StatusCode(404, "content not found");
        }

        /// <summary>
        /// A method for the Creating user request in the rest api call.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<(bool, string)>> CreateUser([FromBody]User user)
        {
            if (user != null)
            {
                await ur.CreateUser(user);
                return StatusCode(201, "Successfully created.");
            }

            return BadRequest();
        }

        [HttpPatch("UpdateProfile")]
        public async Task<ActionResult<bool>> UpdateProfile([FromBody]CombineUser combined)
        {
            await pr.EditProfile(combined.user, combined.newUser);
            return StatusCode(200, "Updated Successfully");
        }

        public class CombineUser
        {
            public User user { get; set; }
            public User newUser { get; set; }
        }
    }
}