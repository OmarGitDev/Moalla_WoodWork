using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class Reglements_BLL : GenericBLL<Reglements>
    {
        public static bool CheckIfReferenceExist(string Reference, int ID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Reglements p = BD.Reglements.Where(e => e.Reference == Reference && e.ID != ID).FirstOrDefault();
                return p != null;
            }
        }
        public static Reglements GetReglementByReference(string Reglement)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                return BD.Reglements.Where(e => e.Reference == Reglement).FirstOrDefault();

            }
        }
        public static void updateClosed(int reglementID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Reglements r = BD.Reglements.Where(e => e.ID == reglementID).FirstOrDefault();
                List<MappingReglementPieces> mappingList = MappingReglementPiecesBLL.GetMappingsByReglement(reglementID).ToList();
                double amountSum = mappingList.Count() > 0 ? mappingList.Sum(e => e.Montant) : 0;
                if (Math.Abs(amountSum) == r.Montant)
                    r.closed = true;
                else r.closed = false;
                BD.SaveChanges();
            }

        }
        public static void SaveReglementChanges(int ReglementID, double Montant, DateTime DateReglement, int OwnerId, string Reference, string Remarques, DateTime? DateEcheance)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Reglements reglement = BD.Reglements.Where(e => e.ID == ReglementID).FirstOrDefault();
                if (reglement != null)
                {
                    reglement.Montant = Montant;
                    reglement.Reference = Reference;
                    reglement.DateEcheance = DateEcheance;
                    reglement.DateReglement = DateReglement;
                    reglement.OwnerId = OwnerId;
                    reglement.Remarques = Remarques;

                    BD.SaveChanges();
                }
            }
        }
        public static List<ReglementsModel> getOpenedReglementsByOwnerCode(int OwnerCode, string OwnerType)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<ReglementsModel> resList = GenericModelMapper.GetModelList<ReglementsModel, Reglements>(BD.Reglements.Where(e => e.OwnerId == OwnerCode && e.closed == false && e.OwnerType == OwnerType).ToList());
                foreach (var res in resList)
                {
                    List<MappingReglementPieces> mappingList = MappingReglementPiecesBLL.GetMappingsByReglement(res.ID).ToList();
                    double usedAmount = mappingList.Count() > 0 ? mappingList.Sum(e => e.Montant) : 0;
                    res.MontantRestant = res.Montant - Math.Abs(usedAmount);

                }
                return resList;
            }

        }
        public static List<ReglementsModel> getReglementsByFacture(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<ReglementsModel> res = (from r in BD.Reglements
                                             join m in BD.MappingReglementPieces on r.ID equals m.ReglementID
                                             join banc in BD.Banque on r.Banque equals banc.ID into bancs
                                             from banc in bancs.DefaultIfEmpty()

                                             join t in BD.TypeReglement on r.TypeReglement equals t.ID into types
                                             from t in types.DefaultIfEmpty()


                                             where m.PieceID == NumPiece
                                             select new ReglementsModel()
                                             {
                                                 ID = m.ID,
                                                 TypeReglement = r.TypeReglement,
                                                 Banque = r.Banque,
                                                 DateEcheance = r.DateEcheance,
                                                 DateReglement = r.DateReglement,
                                                 Remarques = r.Remarques,
                                                 Montant = r.Montant,
                                                 OwnerId = r.OwnerId,
                                                 MontantMapping = m.Montant,
                                                 NomBanque = banc.NomBanque,
                                                 LibelleTypeReglement = t.Libelle
                                             }).ToList();
                return (res);
            }
        }
        public static ReglementsModel getReglementsByCode(int CodeReglement)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                ReglementsModel res = (from r in BD.Reglements


                                       join t in BD.TypeReglement on r.TypeReglement equals t.ID into types

                                       from t in types.DefaultIfEmpty()
                                       join c in BD.CompteBancaire on r.compte equals c.ID into compts

                                       from c in compts.DefaultIfEmpty()

                                       where r.ID == CodeReglement
                                       select new ReglementsModel()
                                       {
                                           ID = r.ID,
                                           TypeReglement = r.TypeReglement,
                                           Banque = r.Banque,
                                           DateEcheance = r.DateEcheance,
                                           DateReglement = r.DateReglement,
                                           Remarques = r.Remarques,
                                           Montant = r.Montant,
                                           OwnerId = r.OwnerId,
                                           LibelleCompte  = c.Libelle,
                                           LibelleTypeReglement = t.Libelle
                                       }).FirstOrDefault();
                return (res);
            }
        }
        public static double GetTotalAmountReglementsVenteByClient(int client)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var res = (from r in BD.Reglements


                           where r.OwnerId == client && r.OwnerType == "C"
                           select new TextValueModel()
                           {
                               Val1 = r.Montant,
                               Text = r.Sens
                           }).ToList();

                double Sum = res.Sum(e => e.Val1 * int.Parse(e.Text)).Value;
                return Sum;
            }

        }
        public static double GetTotalAmountReglementsVenteInCurrentDay()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DateTime currentDay = DateTime.Now.Date;
                DateTime Tomorrow = DateTime.Now.Date.AddDays(1);
                var result = (from r in BD.Reglements

                              join c in BD.Client on r.OwnerId equals c.ID
                              where (r.DateReglement >= currentDay && r.DateReglement < Tomorrow) && r.OwnerType == "C"
                              select new TextValueModel()
                              {
                                  Val1 = r.Montant,
                                  Text = r.Sens
                              }).ToList();

                double res = result.Sum(e => int.Parse(e.Text) * e.Val1.Value);
                return res;
            }

        }
        public static double GetTotalAmountReglementsAchatInCurrentDay()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DateTime currentDay = DateTime.Now.Date;
                DateTime Tomorrow = DateTime.Now.Date.AddDays(1);
                var result = (from r in BD.Reglements

                              join c in BD.Client on r.OwnerId equals c.ID
                              where (r.DateReglement >= currentDay && r.DateReglement < Tomorrow) && r.OwnerType == "F"
                              select new TextValueModel()
                              {
                                  Val1 = r.Montant,
                                  Text = r.Sens
                              }).ToList();

                double res = result.Sum(e => int.Parse(e.Text) * e.Val1.Value);
                return res;
            }

        }

        public static List<ReglementsModel> GetAllReglementsClient(int OwnerFilter, string LibelleFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<ReglementsModel> res = (from r in BD.Reglements
                                             join c in BD.Client on r.OwnerId equals c.ID
                                             join banc in BD.Banque on r.Banque equals banc.ID into bancs
                                             from banc in bancs.DefaultIfEmpty()

                                             join t in BD.TypeReglement on r.TypeReglement equals t.ID into types
                                             from t in types.DefaultIfEmpty()
                                             where (OwnerFilter == 0 || r.OwnerId == OwnerFilter)
                                              && (String.IsNullOrEmpty(LibelleFilter) || r.Remarques.Contains(LibelleFilter))
                                            && (DateFromFilter == null || r.DateReglement >= DateFromFilter)
                                            && (DateToFilter == null || r.DateReglement < DateToFilter)
                                            && (MontantMinFilter == null || r.Montant >= MontantMinFilter)
                                            && (MontantMaxFilter == null || r.Montant < MontantMaxFilter)
                                             select new ReglementsModel()
                                             {
                                                 ID = r.ID,
                                                 TypeReglement = r.TypeReglement,
                                                 Banque = r.Banque,
                                                 DateEcheance = r.DateEcheance,
                                                 DateReglement = r.DateReglement,
                                                 Reference = r.Reference,
                                                 Montant = r.Montant,
                                                 OwnerId = r.OwnerId,
                                                 NomClient = c.OwnerName,
                                                 NomBanque = banc.NomBanque,
                                                 LibelleTypeReglement = t.Libelle,

                                                 compte = r.compte

                                             }).ToList();
                return (res);
            }
        }
        public static List<ReglementsModel> GetAllReglementsFournisseur(int OwnerFilter, string LibelleFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<ReglementsModel> res = (from r in BD.Reglements
                                             join f in BD.Fournisseur on r.OwnerId equals f.ID
                                             join banc in BD.Banque on r.Banque equals banc.ID into bancs
                                             from banc in bancs.DefaultIfEmpty()

                                             join t in BD.TypeReglement on r.TypeReglement equals t.ID into types
                                             from t in types.DefaultIfEmpty()
                                             where (OwnerFilter == 0 || r.OwnerId == OwnerFilter)
                                             && (String.IsNullOrEmpty(LibelleFilter) || r.Remarques.Contains(LibelleFilter))
                                           && (DateFromFilter == null || r.DateReglement >= DateFromFilter)
                                           && (DateToFilter == null || r.DateReglement < DateToFilter)
                                           && (MontantMinFilter == null || r.Montant >= MontantMinFilter)
                                           && (MontantMaxFilter == null || r.Montant < MontantMaxFilter)

                                             select new ReglementsModel()
                                             {
                                                 ID = r.ID,
                                                 TypeReglement = r.TypeReglement,
                                                 Banque = r.Banque,
                                                 DateEcheance = r.DateEcheance,
                                                 DateReglement = r.DateReglement,
                                                 Reference = r.Reference,
                                                 Remarques = r.Remarques,
                                                 OwnerId = r.OwnerId,
                                                 NomFournisseur = f.OwnerName,
                                                 NomBanque = banc.NomBanque,
                                                 LibelleTypeReglement = t.Libelle,

                                                 compte = r.compte
                                             }).ToList();
                return (res);
            }
        }
    }


}
