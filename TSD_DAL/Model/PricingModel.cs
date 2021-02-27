using System;

namespace TSD_DAL.Model
{
    public class PricingModel
    {
        public int ID { get; set; }
        public string CodePricing { get; set; }
        public string Libelle { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<System.DateTime> ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidUntil { get; set; }
        public string Statut { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
        public string EditedBy { get; set; }
        public string Type { get; set; }
        public double MontantFinal { get; set; }
        public double MontantTotal { get; set; }
        //Applicatif
        public string NomClient { get; set; }
    }
}
