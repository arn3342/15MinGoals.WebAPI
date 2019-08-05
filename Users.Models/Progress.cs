using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Users.Models
{
    public class Progress
    {
        [BsonId]
        public ObjectId Progress_Id { get; set; }
        public string Goal_Id { get; set; }
        public string Goal_Level { get; set; }
        public string CurrentCourse_Id { get; set; }
        public int Xp_Points { get; set; }
    }
}
