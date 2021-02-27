using System;

namespace TSD_DAL.Model
{
    public class ReglementsModel
    {
        public int ID { get; set; }
        public Nullable<int> TypeReglement { get; set; }
        public Nullable<int> Banque { get; set; }
        public Nullable<System.DateTime> DateEcheance { get; set; }
        public System.DateTime DateReglement { get; set; }
        public string Reference { get; set; }
        public string Remarques { get; set; }
        public double Montant { get; set; }
        public string OwnerType { get; set; }
        public string Sens { get; set; }
        public int OwnerId { get; set; }
        public Nullable<int> compte { get; set; }
        public bool closed { get; set; }
        public bool RAS { get; set; }
        //Applicatif
        public double? MontantMapping { get; set; }
        public string numPiece { get; set; }
        public string LibelleTypeReglement { get; set; }
        public string NomBanque { get; set; }
        public string NomClient { get; set; }
        public string NomFournisseur { get; set; }
        public string LibelleCompte { get; set; }
        public bool automaticAttach { get; set; }
        public double? MontantRestant { get; set; }
        public string ApplicatifSens { get; set; }
    }
}
