using System;
using System.ComponentModel.DataAnnotations;

namespace TSD_DAL.Model
{
    public class PersonnesModel
    {
        public int ID { get; set; }
        public string fullName { get; set; }
        [Required(ErrorMessage = "dfgdfgd")]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public Nullable<int> Fonction { get; set; }
        [Required]
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        [Required]
        public Nullable<int> SoldeCongeAnnuel { get; set; }
        public Nullable<double> SoldeCongeRestant { get; set; }
        public string AdresseEmail { get; set; }
        public Nullable<bool> IsActif { get; set; }
        [Required]
        public Nullable<System.DateTime> DateDebutContrat { get; set; }
        public Nullable<System.DateTime> DateFinContrat { get; set; }
        public Nullable<int> TypePaymentID { get; set; }
    }
}
