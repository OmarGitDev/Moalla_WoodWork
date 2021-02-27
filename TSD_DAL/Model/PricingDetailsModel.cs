using System;

namespace TSD_DAL.Model
{

    public class PricingDetailsModel
    {
        public int ID { get; set; }
        public string CodeDetail { get; set; }
        public string Libelle { get; set; }
        public Nullable<double> MontantUnitaire { get; set; }
        public Nullable<int> Quantite { get; set; }
        public Nullable<double> MontantTotal { get; set; }
        public Nullable<int> ProduitID { get; set; }
        public string Description { get; set; }
        public int PricingID { get; set; }
        public Nullable<double> Remise { get; set; }
        public Nullable<int> CodeTaxe { get; set; }
        public double MontantTaxe { get; set; }
        public double MontantHorsTaxe { get; set; }
        //Applicatif
        public string numPiece { get; set; }
        public string QteString { get; set; }
        public string pourcentageTaxe { get; set; }
        //public Nullable<double> remise { get; set; }
        // public int? taxe { get; set; }

    }
}
