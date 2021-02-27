using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class Client_BLL : GenericBLL<Client>
    {
        public static List<Client> GetActifClients()
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                return DB.Client.Where(e => e.IsActive == true).ToList();
            }
        }
        public static ClientModel GetByCodeFacture(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                var Client = (from pv in DB.PieceVente
                              join c in DB.Client on pv.CodeClient equals c.ID
                              where pv.NumPieceVente == NumPiece
                              select new ClientModel()
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
                                  Route = c.Route,
                                  Exonere = c.Exonere,
                                  Interlocuteur = c.Interlocuteur,
                                  TelephoneInterlocuteur = c.TelephoneInterlocuteur,
                                  DebutConvention = c.DebutConvention,
                                  FinConvention = c.FinConvention,
                                  NombrePassages = c.NombrePassages,
                                  TFExo = c.TFExo
                              }).FirstOrDefault();
                return Client;
            }
        }

        public static List<GET_ETAT_CLIENTS_Result> GetEtatClients(DateTime DateFromFilter, DateTime DateToFilter)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                return DB.GET_ETAT_CLIENTS(DateFromFilter, DateToFilter).ToList();
            }
        }
        public static List<GET_RELEVE_COMPTE_Result> GetReleveCompte(DateTime DateFromFilter, DateTime DateToFilter, int ClientFilter)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                return DB.GET_RELEVE_COMPTE(DateFromFilter, DateToFilter, ClientFilter).ToList();
            }
        }
        public static List<GET_FournituresPerMonth_Result> LoadFournitureData(DateTime Date)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                DateTime DateTo = Date.AddMonths(1).AddDays(-1);
                return DB.GET_FournituresPerMonth(Date, DateTo).ToList();
            }
        }
        public static List<GET_MarchandisesPerMonth_Result> LoadMarchandisesData(DateTime Date)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                DateTime DateTo = Date.AddMonths(1).AddDays(-1);
                return DB.GET_MarchandisesPerMonth(Date, DateTo).ToList();
            }
        }
        public static List<GET_BeneficesPerMonth_Result> LoadBeneficesData(DateTime Date)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                DateTime DateTo = Date.AddMonths(1).AddDays(-1);
                return DB.GET_BeneficesPerMonth(Date, DateTo).ToList();
            }
        }
    }
}
