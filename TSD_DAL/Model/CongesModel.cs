using System;

namespace TSD_DAL.Model
{
    public class CongesModel
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public Nullable<int> MotifId { get; set; }
        public System.DateTime DateFrom { get; set; }
        public System.DateTime DateTo { get; set; }
        public Nullable<int> PersonID { get; set; }
        public string Statut { get; set; }
        //applicatif
        public string MotifName { get; set; }
        public string PersonName { get; set; }
    }
}
