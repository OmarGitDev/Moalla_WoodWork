using System;

namespace TSD_DAL.Model
{
    public class ServicesModel
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public string Reference { get; set; }
        public Nullable<double> Montant { get; set; }
        public string numPiece { get; set; }
        public int codePricing { get; set; }
        public int Qte { get; set; }
        public int taxe { get; set; }
        public bool Vente { get; set; }
        public bool Achat { get; set; }
    }
}
