using System;

namespace TSD_DAL.Model
{
    public class PassagesClientsModel
    {
        public int ID { get; set; }
        public DateTime DatePassage { get; set; }
        public int ClientID { get; set; }
        public string Status { get; set; }
        //Applicatif
        public string ClientName { get; set; }
        public string ClientAdresse { get; set; }

    }
}
