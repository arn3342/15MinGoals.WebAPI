using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Profile Profile { get; set; }
        public bool IsFacebookAuth { get; set; }
        public bool IsLinkedInAuth { get; set; }
        public int[] Reference_Ids { get; set; }
        public int[] Post_Ids { get; set; }
    }
}
