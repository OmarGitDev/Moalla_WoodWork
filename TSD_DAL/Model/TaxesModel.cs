using System;

namespace TSD_DAL.Model
{
    public class TaxesModel
    {
        public int ID { get; set; }
        public string NomTaxe { get; set; }
        public Nullable<double> Pourcentage { get; set; }
        public bool IsDefault { get; set; }
    }
}
