using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Models
{
    public class Post
    {
        [BsonId]
        public ObjectId Post_Id { get; set; }
        public ObjectId Goal_Id { get; set; }
        public string Post_Type { get; set; }
        public DateTime Post_Time { get; set; }
        public string Post_Header { get; set; }
        public string Post_Body { get; set; }
        public string Post_Credential { get; set; }
        public string Privacy_Level { get; set; }
        [BsonId]
        public ObjectId Feedback_Ids { get; set; }
        [BsonId]
        public ObjectId Inspire_Ids { get; set; }

        public void CreateNewPost()
        {
            Post post = new Post
            {
                Post_Type = PostType.Regular = "Regular"
            };
        }
    }

    /// <summary>
    /// This class is only defined to store required properties for a model/property.
    /// </summary>
    public class PostType
    {
        public static string Regular
        {
            get { return "Regular"; }
            set { Regular = "Regular"; }
        }
        public static string Goal
        {
            get { return "Goal"; }
            set { Goal = "Goal"; }
        }
        public static string Share
        {
            get { return "Share"; }
            set { Share = "Share"; }
        }
        public static string Short_Article
        {
            get { return "Short_Article"; }
            set { Short_Article = "Short_Article"; }
        }
    }
    // Create a class for PrivacyType
}
