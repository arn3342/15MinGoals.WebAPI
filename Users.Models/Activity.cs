using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Users.Models
{
    public class Activity
    {
        [BsonId]
        public ObjectId Activity_Id { get; set; }
        public string Activity_Type { get; set; }
        public string Content_Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Time_Invested { get; set; }
    }

    
    
}
