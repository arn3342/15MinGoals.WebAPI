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
    public class PostController : ControllerBase
    {
        PostRepository pr;
        public PostController()
        {
            pr = new PostRepository(Startup.ConnectionString);
        }
        [HttpGet("CreatePost")]
        public async Task<ActionResult<bool>> CreatePost(Post post,string emailId,ObjectId goalId)
        {
            if(emailId !=null && goalId!=null)
            {
                await pr.CreatedPost(post, emailId, goalId);
                return StatusCode(201, "Successfully Created");
            }
            return BadRequest();
        }
    }
}