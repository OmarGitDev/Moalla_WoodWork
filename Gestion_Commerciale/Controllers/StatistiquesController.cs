using Gestion_Commerciale.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSD_BLL;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace Gestion_Commerciale.Controllers
{
    [CustomAuthorize]
    public class StatistiquesController : Controller
    {
        // GET: Statistiques
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Clients()
        {
            return View();
        }
        public ActionResult ChiffreAffaires()
        {
            return View();
        }
        public ActionResult EtatClient()
        {
            return View();
        }
        public ActionResult ReleveDeCompte()
        {
            return View();
        }
        
        public ActionResult EngagementsFournisseurs()
        {
            return View();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashbord()
        {
            return View();
        }
        
        public ActionResult GetReglementsStatByClient(int ClientFilter)
        {
            List<int> Result = new List<int>();
            try
            {
                int ReglementsPercent = 0;
                int FacturesPercent = 0;
                double totalFactures = PieceVente_BLL.GetTotalAmountPiecesVenteByClient(ClientFilter);
                double totalReglements = Reglements_BLL.GetTotalAmountReglementsVenteByClient(ClientFilter);
                if(totalFactures != 0 || totalReglements != 0)
                {
                    ReglementsPercent = (int)Math.Truncate((totalReglements * 100 / (totalReglements + totalFactures)));
                    FacturesPercent = (int)Math.Truncate((totalFactures * 100 / (totalReglements + totalFactures)));
                }
                Result.Add(ReglementsPercent);
                Result.Add(FacturesPercent);
                

                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEtatClients(DateTime DateFromFilter,DateTime DateToFilter)
        {
            List<GET_ETAT_CLIENTS_Result> mappingModels = new List<GET_ETAT_CLIENTS_Result>();
            if(DateFromFilter.Year > 2018 && DateToFilter.Year > 2015)
                 mappingModels = Client_BLL.GetEtatClients(DateFromFilter, DateToFilter);
            return Json(mappingModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReleveDeComptes(DateTime DateFromFilter, DateTime DateToFilter,int ClientFilter)
        {
            List<GET_RELEVE_COMPTE_Result> mappingModels = new List<GET_RELEVE_COMPTE_Result>();
            if (DateFromFilter.Year > 2015 && DateToFilter.Year > 2015)
                mappingModels = Client_BLL.GetReleveCompte(DateFromFilter, DateToFilter, ClientFilter);

            return Json(mappingModels, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetEngagementsFournisseurs(DateTime DateFromFilter, DateTime DateToFilter)
        {
            List<GET_ENGAGEMENT_FOURNISSEURs_Result> mappingModels = new List<GET_ENGAGEMENT_FOURNISSEURs_Result>();
            if (DateFromFilter.Year > 2015 && DateToFilter.Year > 2015)
                mappingModels = Fournisseur_BLL.GetEngagementsFournisseurs(DateFromFilter, DateToFilter);
            return Json(mappingModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadFournitureData(DateTime DateFilter)
        {

            List<DashbordModel> mappingModels = GenericModelMapper.GetModelList< DashbordModel , GET_FournituresPerMonth_Result> (Client_BLL.LoadFournitureData(DateFilter));
            DashbordModel Total = new DashbordModel();
            Total.OwnerName = "Total";
            Total.Montant = mappingModels.Sum(e=>e.Montant);
            mappingModels.Add(Total);
            return Json(mappingModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadMarchandisesData(DateTime DateFilter)
        {

            List<DashbordModel> mappingModels = GenericModelMapper.GetModelList<DashbordModel, GET_MarchandisesPerMonth_Result>(Client_BLL.LoadMarchandisesData(DateFilter));
            DashbordModel Total = new DashbordModel();
            Total.OwnerName = "Total";
            Total.Montant = mappingModels.Sum(e => e.Montant);
            mappingModels.Add(Total);
            return Json(mappingModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadBeneficesData(DateTime DateFilter)
        {

            List<DashbordModel> mappingModels = GenericModelMapper.GetModelList<DashbordModel, GET_BeneficesPerMonth_Result>(Client_BLL.LoadBeneficesData(DateFilter));
            DashbordModel Total = new DashbordModel();
            Total.OwnerName = "Total";
            Total.Montant = mappingModels.Sum(e => e.Montant);
            mappingModels.Add(Total);

            return Json(mappingModels, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetChiffreAffaireMoisStats( int Annee)
        {
            List<string> Result = new List<string>();
            try
            {

                for (int i = 1; i <= 12; i++)
                {
                    DateTime DateFrom = new DateTime(Annee, i, 1);

                    DateTime DateTo = i == 12 ? new DateTime(Annee + 1, 1, 1) : new DateTime(Annee, i + 1, 1);


                    int res = (int)Math.Truncate(Piece_BLL.GetAllPiecesBetweenDates(DateFrom, DateTo));
                    Result.Add(res.ToString());
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetChiffreAffaireStats(int Annee)
        {
            List<string> Result = new List<string>();
            try
            {
                int res = 0;
                for (int i = 1; i <= 12; i++)
                {
                    DateTime DateFrom = new DateTime(Annee, i, 1);

                    DateTime DateTo = i == 12 ? new DateTime(Annee + 1, 1, 1) : new DateTime(Annee, i + 1, 1);


                    res += (int)Math.Truncate(Piece_BLL.GetAllPiecesBetweenDates(DateFrom, DateTo));
                    Result.Add(res.ToString());
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetPaymentStatByClient(int ClientFilter,int Annee)
        {
            List<string> Result = new List<string>();
            try
            {
                
                for(int i = 1; i<=12; i++)
                {
                    DateTime DateFrom = new DateTime(Annee, i, 1);
                    
                    DateTime DateTo = i == 12? new DateTime(Annee+1, 1, 1): new DateTime(Annee, i+1, 1);
                    

                    int res = (int)Math.Truncate(Piece_BLL.GetAllPiecesClientsBetweenDates(ClientFilter, DateFrom, DateTo));
                    Result.Add(res.ToString());
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }
        
            public static IList<SelectListItem> GetMonthsList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            res.Add(new SelectListItem()
                {
                    Text = "Janvier",
                    Value = "1"
                });
            res.Add(new SelectListItem()
            {
                Text = "Février",
                Value = "2"
            });
            res.Add(new SelectListItem()
            {
                Text = "Mars",
                Value = "3"
            });
            res.Add(new SelectListItem()
            {
                Text = "Avril",
                Value = "4"
            });
            res.Add(new SelectListItem()
            {
                Text = "Mai",
                Value = "5"
            });
            res.Add(new SelectListItem()
            {
                Text = "Juin",
                Value = "6"
            });
            res.Add(new SelectListItem()
            {
                Text = "Juillet",
                Value = "7"
            });
            res.Add(new SelectListItem()
            {
                Text = "Aout",
                Value = "8"
            });
            res.Add(new SelectListItem()
            {
                Text = "Septembre",
                Value = "9"
            });
            res.Add(new SelectListItem()
            {
                Text = "Octobre",
                Value = "10"
            });
            res.Add(new SelectListItem()
            {
                Text = "Novembre",
                Value = "11"
            });
            res.Add(new SelectListItem()
            {
                Text = "Décembre",
                Value = "12"
            });

            return res;
        }
        public static IList<SelectListItem> GetYearsList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            int CurrentYear = DateTime.Now.Year;
            int fromYear = CurrentYear - 20;
            int toYear = CurrentYear + 20;
            for(int i = fromYear; i< toYear; i++)
            { res.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            return res;
        }
    }
}