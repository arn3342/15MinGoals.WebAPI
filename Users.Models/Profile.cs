using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Models
{
    /// <summary>
    /// Represents the profile of a user.
    /// </summary>
    public class Profile
    {
        public int Profile_Id { get; set; }      
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Profile_Img_Url { get; set; }
        public string Default_Credential { get; set; }
        public string Country { get; set; }
        public string Best_Describes { get; set; }
        public string Education_Institute { get; set; }
        public string Work_Institute { get; set; }
        public bool Tour_Complete { get; set; }
        public int[] Connection_Ids { get; set; }
        public int[] Post_Ids { get; set; }
    }
}
