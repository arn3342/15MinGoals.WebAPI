using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Models
{
    public class Feedback
    {
        public int Feedback_Id { get; set; }
        public int Profile_Id { get; set; }
        public string Feedback_Body { get; set; }
    }
}
