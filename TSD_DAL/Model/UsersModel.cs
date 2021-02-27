using System.Collections.Generic;

namespace DAL.Models
{
    public class UsersModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }

        // public List<RolesModel> Roles { get; set; }
        public List<RolesModel> AvailableRoles { get; set; }
        //this list has our default values 
        public List<RolesModel> SelectedRoles { get; set; }
        //this will retrieve the ids of movies selected in list when submitted
        public List<string> SubmittedRoles { get; set; }

    }
}
