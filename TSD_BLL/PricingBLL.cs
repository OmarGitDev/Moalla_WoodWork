using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class PricingBLL : GenericBLL<Pricing>
    {
        public static List<PricingModel> GetAllValidPricingByClient(int codeClient, string Type)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DateTime currentDate = DateTime.Now;
                List<Pricing> d = BD.Pricing.Where(e => e.Statut == "VLD" && e.ClientID == codeClient && e.ValidFrom < currentDate && e.ValidUntil > currentDate && e.Type == Type).ToList();
                return (GenericModelMapper.GetModelList<PricingModel, Pricing>(d));
            }
        }
        public static List<PricingModel> GetPricingsByType(string Type)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<PricingModel> d = (from pr in BD.Pricing
                                        join cl in BD.Client on pr.ClientID equals cl.ID
                                        where (pr.Type == Type)
                                        select new PricingModel()
                                        {
                                            ID = pr.ID,
                                            CodePricing = pr.CodePricing,
                                            Libelle = pr.Libelle,
                                            ClientID = pr.ClientID,
                                            ValidFrom = pr.ValidFrom,
                                            ValidUntil = pr.ValidUntil,
                                            Statut = pr.Statut,
                                            CreatedOn = pr.CreatedOn,
                                            EditedOn = pr.EditedOn,
                                            EditedBy = pr.EditedBy,
                                            Type = pr.Type,
                                            MontantFinal = pr.ID,
                                            MontantTotal = pr.ID,
                                            NomClient = cl.OwnerName
                                        }).ToList();
                return d;
            }
        }
        public static Pricing GetByCodePricing(string CodePricing)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                return (BD.Pricing.Where(e => e.CodePricing == CodePricing).FirstOrDefault());
            }
        }
        public static int InsertPricingAndReturnID(Pricing pricing)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                BD.Pricing.Add(pricing);
                BD.SaveChanges();
                Pricing p = BD.Pricing.Where(e => e.CodePricing == pricing.CodePricing).FirstOrDefault();
                return (p.ID);
            }
        }
    }
}
