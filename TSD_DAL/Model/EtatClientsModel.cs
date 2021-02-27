using System;

namespace TSD_DAL.Model
{
    public class EtatClientsModel
    {
        public string OwnerName { get; set; }
        public Nullable<double> H_T { get; set; }
        public Nullable<double> TTC { get; set; }
        public Nullable<double> Réglé { get; set; }
        public Nullable<double> Solde { get; set; }
    }
}
