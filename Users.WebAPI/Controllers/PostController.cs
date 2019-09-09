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
        [HttpPost("CreatePost")]
        public async Task<ActionResult<bool>> CreatePost([FromBody] BuildPost bp)
        {
            if(bp.emailId !=null)
            {
                await pr.CreatedPost(bp.post, bp.emailId,bp.goalId);
                return StatusCode(201, "Successfully Created");
            }
            return BadRequest();
        }

        public class BuildPost
        {
            public Post post { get; set; }
            public string emailId { get; set; }
            public ObjectId goalId { get; set; }
        }
    }
}