using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Models
{
    public class Progress
    {
        public int Goal_Id { get; set; }
        public string Goal_Level { get; set; }
        public int CurrentCourse_Id { get; set; }
        public int Xp_Points { get; set; }
    }
}
