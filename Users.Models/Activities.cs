using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Models
{
    public class Activity
    {
        public int Goal_Id { get; set; }
        public string Activity_Type { get; set; }
        public int Content_Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Time_Invested { get; set; }
    }

    
    
}
