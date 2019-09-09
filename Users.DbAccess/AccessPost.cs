using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Users.Models;
using Users.DbAccess;
using Users.DbAccess.Tools;

namespace Users.DbAccess
{
    public class AccessPost
    {
        private MongoDbContext _dbContext;
        private IMongoCollection<Post> post;
        private IMongoCollection<User> user;
        private FilterBuilder fl = new FilterBuilder();
        public AccessPost(String ConnectionString)
        {
            _dbContext = new MongoDbContext(ConnectionString);
            post = _dbContext.Db().GetCollection<Post>(nameof(MongoDbContext.Collection.posts));
            user = _dbContext.Db().GetCollection<User>(nameof(MongoDbContext.Collection.user));
        }

        /// <summary>
        /// An async method to create newpost for the user
        /// </summary>
        /// <param name="postObj">Data obj that will be pass as body with the create request.</param>
        /// <param name="email">Unique identifier of the user</param>
        /// <returns></returns>

        public async Task<bool> CreatePost(Post postObj,string email,ObjectId goalId)
        {
            bool IsExists;
            IsExists = await user.Find(fl.ToFind<User>(nameof(email), email)).AnyAsync();
            if (IsExists)
            {
                Post p = new Post();
                p.Goal_Id = goalId;
                p.Post_Id = ObjectId.GenerateNewId();
                p.Post_Type = postObj.Post_Type;
                p.Post_Time = DateTime.Now;
                p.Privacy_Level = postObj.Privacy_Level;
                p.Post_Body = postObj.Post_Body;
                p.Post_Credential = postObj.Post_Credential;
                p.Post_Header = postObj.Post_Header;
                p.Inspire_Ids = ObjectId.GenerateNewId().ToString();
                p.Feedback_Ids = ObjectId.GenerateNewId().ToString();

                try
                {
                    await post.InsertOneAsync(p);
                }
                catch (Exception e)
                {
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}
