//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TSD_DAL.Model
{ 
    using System;
    using System.Collections.Generic;
    
    public class FournisseurModel
    {
    
        public int ID { get; set; }
        public string OwnerName { get; set; }
        public string Adresse { get; set; }
        public string CodePostal { get; set; }
        public string IBAN { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Fax { get; set; }
        public string MatriculeFiscal { get; set; }
        public bool IsActive { get; set; }
        public string FType { get; set; }
        public string Interlocuteur { get; set; }
        public string NumeroInterlocuteur { get; set; }
        public double RASValue { get; set; }

    }
}
