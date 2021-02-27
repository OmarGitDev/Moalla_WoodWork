using System;
using System.ComponentModel.DataAnnotations;

namespace TSD_DAL.Model
{
    public class JoursFeriesModel
    {
        public int ID { get; set; }
        //[DisplayFormat (ApplyFormatInEditMode =true, DataFormatString ="{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }
        public string Libelle { get; set; }
        public Nullable<int> NbrJours { get; set; }
    }
}
