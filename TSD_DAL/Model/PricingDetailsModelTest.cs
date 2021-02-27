using System;

namespace TSD_DAL.Model
{

    public class PricingDetailsModelTest
    {
        public int ID { get; set; }
        public string CodeDetail { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public int PricingID { get; set; }
        public double MontantTaxe { get; set; }
        public double MontantHorsTaxe { get; set; }
        public Nullable<double> MontantUnitaire { get; set; }
        public Nullable<int> Quantite { get; set; }
        public Nullable<double> MontantTotal { get; set; }
        public Nullable<double> Remise { get; set; }
        public Nullable<int> CodeTaxe { get; set; }
    }
}
