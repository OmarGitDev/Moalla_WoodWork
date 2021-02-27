using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class CompteBancaire_BLL : GenericBLL<CompteBancaire>
    {
        public static List<CompteBancaireModel> GetAllComptesFournisseurs()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from cb in BD.CompteBancaire
                              join f in BD.Fournisseur on cb.Owner equals f.ID
                              join b in BD.Banque on cb.BanqueId equals b.ID into banc
                              from b in banc.DefaultIfEmpty()
                              where cb.TypeOwner == "Fournisseur"
                              //join c in BD.Client on cb.Owner equals c.ID into client
                              //from c in client.DefaultIfEmpty()

                              select new CompteBancaireModel()
                              {
                                  ID = cb.ID,
                                  OwnerName = f.OwnerName,
                                  NomBanque = b.NomBanque,
                                  RIB = cb.RIB,
                                  Libelle = cb.Libelle,
                                  Owner = f.ID,


                              }
                    ).ToList();
                return result;
            }

        }
        public static List<CompteBancaireModel> GetAllComptesClients()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from cb in BD.CompteBancaire
                              join c in BD.Client on cb.Owner equals c.ID
                              join b in BD.Banque on cb.BanqueId equals b.ID into banc
                              from b in banc.DefaultIfEmpty()

                                  //join c in BD.Client on cb.Owner equals c.ID into client
                                  //from c in client.DefaultIfEmpty()
                              where cb.TypeOwner == "Client"
                              select new CompteBancaireModel()
                              {
                                  ID = cb.ID,
                                  OwnerName = c.OwnerName,
                                  NomBanque = b.NomBanque,
                                  RIB = cb.RIB,
                                  Libelle = cb.Libelle,
                                  Owner = c.ID,


                              }
                    ).ToList();
                return result;
            }

        }
        public static List<CompteBancaireModel> GetAllComptesSTEModel()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from cb in BD.CompteBancaire
                              join b in BD.Banque on cb.BanqueId equals b.ID into banc
                              from b in banc.DefaultIfEmpty()

                                  //join c in BD.Client on cb.Owner equals c.ID into client
                                  //from c in client.DefaultIfEmpty()
                              where cb.TypeOwner == "STE"
                              select new CompteBancaireModel()
                              {
                                  ID = cb.ID,
                                  OwnerName = "STE",
                                  NomBanque = b.NomBanque,
                                  RIB = cb.RIB,
                                  Libelle = cb.Libelle
                              }
                    ).ToList();
                return result;
            }

        }
        public static List<CompteBancaire> GetAllComptesSTE()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = BD.CompteBancaire.Where(e => e.TypeOwner == "STE").ToList();
                return result;
            }

        }
        public static List<CompteBancaire> GetAllComptesByClient(int ClientCode)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = BD.CompteBancaire.Where(e => e.TypeOwner == "Client" && e.Owner == ClientCode).ToList();
                return result;
            }

        }
        public static List<CompteBancaire> GetAllComptesByFournisseur(int FournisseurCode)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = BD.CompteBancaire.Where(e => e.TypeOwner == "Fournisseur" && e.Owner == FournisseurCode).ToList();
                return result;
            }

        }

    }
}
