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
    
    public partial class Pricing
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
    }
}