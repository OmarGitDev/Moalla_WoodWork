using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.TSD_EDMX;
namespace TSD_BLL
{
    public class Fournisseur_BLL : GenericBLL<Fournisseur>
    {
        public static List<Fournisseur> GetListFournisseursByType(string TypeFournisseur)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                return (DB.Fournisseur.Where(e => e.FType == TypeFournisseur || String.IsNullOrEmpty(TypeFournisseur)).ToList());
            }
        }
        public static List<GET_ENGAGEMENT_FOURNISSEURs_Result> GetEngagementsFournisseurs(DateTime DateFromFilter, DateTime DateToFilter)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                return DB.GET_ENGAGEMENT_FOURNISSEURs(DateFromFilter, DateToFilter).ToList();
            }
        }
    }
}
