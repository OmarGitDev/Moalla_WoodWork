using System;

namespace TSD_DAL.Model
{
    public class ReclamationsModel
    {
        public int ID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Reclamation { get; set; }
        public string Client { get; set; }

    }
}
