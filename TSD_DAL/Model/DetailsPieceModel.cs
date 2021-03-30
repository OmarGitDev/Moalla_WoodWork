using System;

namespace TSD_DAL.Model
{
    public class DetailsPieceModel
    {
        public int ID { get; set; }
        public string CodeDetailPiece { get; set; }
        public string Piece { get; set; }
        public string Libelle { get; set; }
        public Nullable<int> Quantite { get; set; }
        public Nullable<double> MontantUnitaire { get; set; }
        public Nullable<int> CodeTaxe { get; set; }
        public Nullable<double> MontantTaxe { get; set; }
        public Nullable<double> MontantHorsTaxe { get; set; }
        public Nullable<double> MontantTotal { get; set; }
        public Nullable<int> ProduitID { get; set; }
        public Nullable<double> Remise { get; set; }
        public double pourcentageTaxe { get; set; }

        public string pourcentageTaxeString { get; set; }
        public string RemiseString { get; set; }

    }
}
