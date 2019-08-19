using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Users.Models
{
    public class Goal
    {
        [BsonId]
        public ObjectId Goal_Id { get; set; }
        public string Profile_Id { get; set; }
        public string Goal_Title { get; set; }
        public Activity[] Activity { get; set; }
        //public Goal()
        //{
        //    Activity = new Activity[];
        //}
    }
}