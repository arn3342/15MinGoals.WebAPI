using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Models
{
    /// <summary>
    /// Represents the login details of a user
    /// </summary>
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Profile Profile { get; set; }
        public bool IsFacebookAuth { get; set; }
        public bool IsLinkedInAuth { get; set; }
        public int[] Reference_Ids { get; set; }
        public int[] Post_Ids { get; set; }
    }
}
