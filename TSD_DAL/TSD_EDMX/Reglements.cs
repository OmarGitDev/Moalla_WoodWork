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
    
    public partial class Reglements
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
    }
}
