using System;

namespace TSD_DAL.Models
{

    public class NotificationsModel
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Seen { get; set; }
        public Nullable<int> Count { get; set; }
        public string Value { get; set; }
        //Applicatif
        public string Url { get; set; }

    }
}