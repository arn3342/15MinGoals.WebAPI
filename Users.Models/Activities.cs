using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Models
{
    public class Activities
    {
        public int Goal_Id { get; set; }
        public string Activity_Type { get; set; }
        public int Content_Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Time_Invested { get; set; }
    }

    public class ActivityType
    {
        public static string Watched
        {
            get { return Watched; }
            set { Watched = "watched"; }
        }
        public static string Completed
        {
            get { return Completed; }
            set { Completed = "completed"; }
        }
        public static string Listened
        {
            get { return Listened; }
            set { Listened = "listened"; }
        }
        public static string Read
        {
            get { return Read; }
            set { Read = "read"; }
        }
    }
}
