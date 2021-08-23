using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
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
        public static FournisseurModel GetByCodeFacture(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                var Client = (from pv in DB.PieceAchat
                              join c in DB.Fournisseur on pv.CodeFournisseur equals c.ID
                              where pv.NumPieceAchat == NumPiece
                              select new FournisseurModel()
                              {
                                  ID = c.ID,
                                  OwnerName = c.OwnerName,
                                  Adresse = c.Adresse,
                                  CodePostal = c.CodePostal,
                                  IBAN = c.IBAN,
                                  Tel1 = c.Tel1,
                                  Tel2 = c.Tel2,
                                  Fax = c.Fax,
                                  MatriculeFiscal = c.MatriculeFiscal,
                                  IsActive = c.IsActive,                                  
                                  Interlocuteur = c.Interlocuteur,                                  
                                  RASValue = c.RASValue,
                                  FType = c.FType,
                                  NumeroInterlocuteur =c.NumeroInterlocuteur

    }).FirstOrDefault();
                return Client;
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
