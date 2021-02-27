using System;

namespace TSD_DAL.Model
{
    public class GenericPieceModel
    {
        // Piece Vente
        public int? IDVente { get; set; }
        public Nullable<int> CodeClient { get; set; }
        public string NomClient { get; set; }
        // Piece Achat
        public int? IDAchat { get; set; }
        public Nullable<int> CodeFournisseur { get; set; }
        public string NomFournisseur { get; set; }

        // Piece 

        public int ID { get; set; }
        public string NumPiece { get; set; }
        public string TypePiece { get; set; }
        public string Libelle { get; set; }
        public double RAS { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public string Statut { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        public DateTime? LastEditTime { get; set; }
        public Nullable<double> MontantTotal { get; set; }
        public Nullable<double> MontantFinal { get; set; }
        public string Adresse { get; set; }
        public string MatriculeFiscal { get; set; }
        public bool Closed { get; set; }
        public bool Comptabilise { get; set; }
        // types piece
        public string LibelleTypePiece { get; set; }
        public string Sens { get; set; }

        public string Categorie { get; set; }
        public string Reference { get; set; }
        public double? Solde { get; set; }

        //public List<DetailsPieceModel> Details { get; set; }

    }
}
