//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TSD_DAL.TSD_EDMX
{
    using System;
    using System.Collections.Generic;
    
    public partial class DetailsPiece
    {
        public int ID { get; set; }
        public string CodeDetailPiece { get; set; }
        public string Piece { get; set; }
        public string Libelle { get; set; }
        public Nullable<int> Quantite { get; set; }
        public Nullable<double> MontantUnitaire { get; set; }
        public Nullable<double> MontantTaxe { get; set; }
        public Nullable<double> MontantHorsTaxe { get; set; }
        public Nullable<double> MontantTotal { get; set; }
        public Nullable<int> ProduitID { get; set; }
        public Nullable<double> Remise { get; set; }
        public double pourcentageTaxe { get; set; }
    }
}
