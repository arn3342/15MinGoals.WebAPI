using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Users.DbAccess;
using Users.Models;


namespace Users.BusinessLayer
{
    public class PostRepository
    {
        AccessPost ap;
        public PostRepository(string conStr)
        {
            ap = new AccessPost(conStr);

        }

        public async Task<bool> CreatedPost(Post postObj,string email,ObjectId goalId)
        {
            return await ap.CreatePost(postObj, email, goalId);
        }
    }
}
