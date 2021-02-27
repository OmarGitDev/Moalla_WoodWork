using System;

namespace TSD_DAL.Model
{
    public class CompteBancaireModel
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public Nullable<int> Owner { get; set; }
        public string TypeOwner { get; set; }
        public string RIB { get; set; }
        public bool isActive { get; set; }
        public Nullable<int> BanqueId { get; set; }
        //applicatif
        public string OwnerName { get; set; }

        public string NomBanque { get; set; }

    }
}
