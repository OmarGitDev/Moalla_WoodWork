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
    using System.ComponentModel.DataAnnotations;

    public class PieceModel
    {
        public int ID { get; set; }
        public string NumPiece { get; set; }
        public string TypePiece { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public string Statut { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }
        public double MontantTotal { get; set; }
        public double MontantFinal { get; set; }
        public bool Closed { get; set; }
        public bool Comptabilise { get; set; }
        public double RAS { get; set; }
        public bool RASClosed { get; set; }
        //Applicatif
        public Nullable<double> MontantReglee { get; set; }
        //[DisplayFormat(DataFormatString = "{0:#,##0.000#}",ApplyFormatInEditMode =true)]
        public Nullable<double> MontantARegler { get; set; }
        public int ReglementID { get; set; }
        public string Sens { get; set; }
        public string Category { get; set; }
        public string Reference { get; set; }
        public double? Solde { get; set; }
        public double? SoldeRAS { get; set; }
        public double? SoldeNet { get; set; }
    }
}
