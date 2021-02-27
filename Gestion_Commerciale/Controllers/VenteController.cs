using Gestion_Commerciale.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSD_BLL;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace Gestion_Commerciale.Controllers
{
    [CustomAuthorize]
    public class VenteController : Controller
    {
        // GET: Vente
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DeleteClient(int ID)
        {
            //Client client = Client_BLL.GetById(ID);
            Client_BLL.Delete(ID);
            return Content("");
        }
        public ActionResult Passages()
        {
            return View();
        }
        public ActionResult GetPassagePerMonth(DateTime Month)
        {
            List<PassagesClientsModel> Passages = PassagesClientsBLL.GetPassagesPerMonth(Month);
            int MothDays = DateTime.DaysInMonth(Month.Year, Month.Month);
            List<ClientModel> clients = GenericModelMapper.GetModelList<ClientModel,Client>(Client_BLL.GetActifClients()).OrderBy(e=>e.Route).ToList();
            if(clients.Count()>0)
            {
                clients[0].NewRoute = true;

            }
            for(int i =1;i< clients.Count();i++)
            {
                clients[i].NewRoute = clients[i].Route != clients[i - 1].Route;
            }
            ViewBag.monthDays = MothDays;
            ViewBag.clients = clients;
            ViewBag.Month = Month;
            return PartialView("~/Views/Vente/PassagePerMonth.cshtml", Passages);
        }
        public ActionResult UpdateCaseStatus(int Client,DateTime Month,int Day,string Status)
        {
            DateTime DatePassage = new DateTime(Month.Year, Month.Month, Day);
            PassagesClients Pass = PassagesClientsBLL.GetPassageByClientAndDate(Client, DatePassage);
            if(Status == "VIDE")
            {
                if(Pass != null && Pass.ID != 0)
                {
                    PassagesClientsBLL.Delete(Pass.ID);
                }                
            }
            else
            {
                if(Pass == null || Pass.ID == 0)
                {
                    Pass = new PassagesClients();
                    Pass.ClientID = Client;
                    Pass.DatePassage = DatePassage;
                    Pass.Status = Status;
                    PassagesClientsBLL.Insert(Pass);
                }
                else
                {
                    Pass.Status = Status;
                    PassagesClientsBLL.Update(Pass);
                }

            }
            return Content("");
        }
        public Piece GenerateNewPieceFromDevis(Pricing Devis)
        {
            Piece NewPiece = new Piece();
            NewPiece.NumPiece = SeriesBLL.GenerateNewSerie("CFAC");
            NewPiece.Libelle = Devis.Libelle;
            NewPiece.MontantFinal = Devis.MontantFinal;
            NewPiece.MontantTotal = Devis.MontantTotal;
            NewPiece.Statut = "ECR";
            NewPiece.TypePiece = "CFAC";
            NewPiece.DateCreation = DateTime.Now;
            NewPiece.LastEditTime = DateTime.Now;
            NewPiece.Closed =false;
            Piece_BLL.Insert(NewPiece);
            SeriesBLL.UpdateLastSerie("CFAC");
            PieceVente NewPieceVente = new PieceVente();
            NewPieceVente.NumPieceVente = NewPiece.NumPiece;
            NewPieceVente.CodeClient = Devis.ClientID;
            PieceVente_BLL.Insert(NewPieceVente);
            return NewPiece;
        }
        public ActionResult ConvertDevisToInvoice(int PricingID)
        {
           Pricing Devis = PricingBLL.GetById(PricingID);
           
           Piece NewPiece = GenerateNewPieceFromDevis(Devis);
           List<PricingDetailsModel> DevisDetails = PricingDetails_BLL.GetAllDetailsByPricing(PricingID);
            foreach(var det in DevisDetails)
            {
                DetailsPiece NewDetail = new DetailsPiece();
                NewDetail.CodeDetailPiece = det.CodeDetail;
                NewDetail.Libelle = det.Libelle;
                NewDetail.MontantHorsTaxe = det.MontantHorsTaxe;
                NewDetail.MontantTaxe = det.MontantTaxe;
                NewDetail.MontantTotal = det.MontantTotal;
                NewDetail.Piece = NewPiece.NumPiece;
                NewDetail.Quantite = det.Quantite;
                NewDetail.Remise = det.Remise;
                NewDetail.MontantUnitaire = det.MontantUnitaire;
                NewDetail.CodeTaxe = det.CodeTaxe;
                DetailsPiece_BLL.Insert(NewDetail);
            }
            return Content(NewPiece.NumPiece);
        }
        public ActionResult DeleteAllDetailsPricing(int PricingID)
        {
            List<PricingDetailsModel> dpml = PricingDetails_BLL.GetAllDetailsByPricing(PricingID);
            foreach (PricingDetailsModel dpm in dpml)
            {
                PricingDetails_BLL.Delete(dpm.ID);
            }
            Pricing pricing = RefreshTotalPricing(PricingID);
            return Json(new TextValueModel("OK", "Opération réussite", pricing.MontantTotal, pricing.MontantFinal), JsonRequestBehavior.AllowGet);
        }
        public static IList<SelectListItem> GetPricingsByClient(int CodeClient,string Type)
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var Pricing = PricingBLL.GetAllValidPricingByClient(CodeClient,Type);
            foreach (var d in Pricing)
            {
                res.Add(new SelectListItem()
                {
                    Text = d.ID + "-" + d.Libelle,
                    Value = d.ID.ToString()
                });
            }
            return res;

        }
        public ActionResult AddOrUpdateClient(ClientModel ClientToAdd)
        {
            try
            {
                Client f = GenericModelMapper.GetModel<Client, ClientModel>(ClientToAdd);
                if (f.ID == 0)
                {
                    Client_BLL.Insert(f);
                    return Json(new TextValueModel("OK", "Client ajouté avec succés"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Client_BLL.Update(f);
                    return Json(new TextValueModel("OK", "Client modifié avec succés"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult OpenClientEditor(int ID)
        {
            ClientModel client = new ClientModel();
            if (ID != 0)
                client = GenericModelMapper.GetModel<ClientModel, Client>(Client_BLL.GetById(ID));
            else
            {
                client.IsActive = true;
                client.Exonere = false;
            }
            return PartialView("~/Views/Vente/EditorTemplates/_ClientEditor.cshtml", client);
        }
        
        public static IList<SelectListItem> GetClientsList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "Tous les Clients",
                Value = "0"
            });
            var clients = Client_BLL.GetAll();
            foreach (var f in clients)
            {
                res.Add(new SelectListItem()
                {
                    Text = f.OwnerName,
                    Value = f.ID.ToString()
                });
            }
            return res;
        }


        
        public ActionResult FactureVente ()
        {
            return View();
        }
        public ActionResult PricingList(string Type)
        {
            ViewBag.Type = Type;
            return View();
        }
        public ActionResult Bon_Commande()
        {
            return View();
        }
        public ActionResult Bon_Livraison()
        {
            return View();
        }
        public ActionResult Client()
        {
            return View();
        }
        public JsonResult GetAllClients()
        {
            List<Client> Clients = Client_BLL.GetAll();
            List<ClientModel> ClientsModelList = GenericModelMapper.GetModelList<ClientModel, Client>(Clients);
            return Json(ClientsModelList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllFacturesVente()
        {
            List<GenericPieceModel> gpm = PieceVente_BLL.GetAllPiecesVente();
            return Json(gpm, JsonRequestBehavior.AllowGet);
        }
        #region Pricing
        public ActionResult OpenPricingEditorToAdd(string Type)
        {
            PricingModel Pricing = new PricingModel();
            Pricing.Type = Type;
            return PartialView("~/Views/Vente/EditorTemplates/_PricingEditor.cshtml", Pricing);
        }
        public ActionResult SavePricingChanges(int ID, string Libelle, int? ClientID = 0,DateTime? ValidFrom = null, DateTime? ValidUntil=null)
        {
            Pricing Pricing = PricingBLL.GetById(ID);
            Pricing.Libelle = Libelle;
            Pricing.ClientID = ClientID;
            Pricing.ValidFrom = ValidFrom;
            Pricing.ValidUntil = ValidUntil;
            PricingBLL.Update(Pricing);
            return Json(new TextValueModel(Libelle, "", ClientID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPricings(string Type)
        {
            List<PricingModel> gpm = PricingBLL.GetPricingsByType(Type);

            return Json(gpm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddNewDetailPricing(PricingDetailsModel detailPricingModel)
        {

            DateTime date = DateTime.Now;
            string dateString = "DP" + date.Year.ToString().Substring(2, 2) + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString();
            PricingDetails detailPricing = new PricingDetails();
            detailPricing.CodeDetail = dateString;
            detailPricing.Quantite = detailPricingModel.Quantite;
            detailPricing.Libelle = detailPricingModel.Libelle;
            PricingDetails_BLL.Insert(detailPricing);
            return Content("");
        }

        public ActionResult DeletePricing(int ID)
        {
            List<PricingDetailsModel> PricingDetails = PricingDetails_BLL.GetAllDetailsByPricing(ID);
            foreach(var det in PricingDetails)
            {
                PricingDetails_BLL.Delete(det.ID);
            }
            PricingBLL.Delete(ID);
            return (Content(""));
        }
        public ActionResult PricingDetails(int ID)
        {
            PricingModel model = GenericModelMapper.GetModel<PricingModel,Pricing>( PricingBLL.GetById(ID));
            return View(model);
        }
        public string AddNewPricing(PricingModel pricingModel)
        {
            DateTime date = DateTime.Now;
            string Prefix = pricingModel.Type == "Tarifs" ? "T" :"D";
            string dateString = Prefix + date.Year.ToString().Substring(2, 2) + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString();
            Pricing pricing = new Pricing();
            pricing.ValidFrom = pricingModel.ValidFrom;
            pricing.ValidUntil = pricingModel.ValidUntil;
            pricing.ClientID = pricingModel.ClientID;
            pricing.CodePricing = dateString;
            pricing.Type = pricingModel.Type;
            pricing.EditedOn = DateTime.Now;
            pricing.CreatedOn = DateTime.Now;
            pricing.EditedBy = "user";
            pricing.CreatedBy = "user";
            pricing.Statut = "ECR";
            pricing.Libelle = pricingModel.Libelle;
           int id = PricingBLL.InsertPricingAndReturnID(pricing);
            return id.ToString();
        }
        public JsonResult GetAllPricingDetails(int ID)
        {
            List<PricingDetailsModel> PricingDetails = PricingDetails_BLL.GetAllDetailsByPricing(ID);
            return Json(PricingDetails, JsonRequestBehavior.AllowGet);
        }
        public static Pricing RefreshTotalPricing(int codePricing)
        {
            Pricing p = PricingBLL.GetById(codePricing);
            List<PricingDetailsModel> details = PricingDetails_BLL.GetAllDetailsByPricing(codePricing);
            p.MontantTotal = Math.Round(details.Sum(e => e.MontantHorsTaxe), 3);
            p.MontantFinal = Math.Round(details.Sum(e => (e.MontantHorsTaxe + e.MontantTaxe) * (1 - (e.Remise == null ? 0 : e.Remise.Value / 100))), 3);
            PricingBLL.Update(p);
            return p;

        }
        public ActionResult AddOrUpdatePricingDetails(PricingDetailsModel pricingDetailsModel)
        {
            try
            {
                PricingDetails p = GenericModelMapper.GetModel<PricingDetails, PricingDetailsModel>(pricingDetailsModel);
                p.Remise = p.Remise == null ? 0 : p.Remise;
                Taxes t = Taxes_BLL.GetById(p.CodeTaxe);
                if (t != null)
                {
                    p.MontantHorsTaxe = p.MontantUnitaire.Value * p.Quantite.Value;
                    p.MontantTaxe = p.MontantHorsTaxe * (t.Pourcentage.Value / 100);
                }
                if (p.ID == 0)
                    PricingDetails_BLL.Insert(p);
                else
                    PricingDetails_BLL.Update(p);
                Pricing pricing = RefreshTotalPricing(p.PricingID);
                return Json(new TextValueModel("OK", "Opération réussite", pricing.MontantTotal, pricing.MontantFinal), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult OpenEditorToFillPricing(string SelectedServices, int codePricing)
        {
            List<string> SelectedProuctList = CommonController.StarsSplitter(SelectedServices);
            List<ServicesModel> pml = new List<ServicesModel>();
            foreach (string Service in SelectedProuctList)
            {
                Services p = ServicesBLL.GetById(int.Parse(Service));
                ServicesModel pm = GenericModelMapper.GetModel<ServicesModel, Services>(p);
                pm.Qte = 1;
                pml.Add(pm);

            }
            ViewBag.Defaulttaxe = Taxes_BLL.getDefaultTaxeId();
            ViewBag.codePricing = codePricing;
            return (PartialView("~/Views/Vente/EditorTemplates/_FillPricingFromServices.cshtml", pml));
        }
        public PricingDetails PrepareDetailPricingFromService(ServicesModel ServiceModel)
        {

            PricingDetails dp = new PricingDetails();
            dp.PricingID = ServiceModel.codePricing;
            dp.Quantite = ServiceModel.Qte;
            dp.MontantUnitaire = ServiceModel.Montant;
            dp.Libelle = ServiceModel.Libelle;
            dp.CodeDetail = ServiceModel.Reference;
            dp.MontantHorsTaxe = ServiceModel.Montant.Value * ServiceModel.Qte;
            dp.MontantTaxe = 0;
            if (ServiceModel.taxe != 0)
            {
                Taxes tax = Taxes_BLL.GetById(ServiceModel.taxe);
                if (tax != null)
                {
                    dp.CodeTaxe = ServiceModel.taxe;
                    dp.MontantTaxe = tax.Pourcentage.Value * dp.MontantHorsTaxe / 100;
                }
            }

            dp.Remise = 0;
            dp.MontantTotal = Math.Round(((dp.MontantHorsTaxe + dp.MontantTaxe)), 3);
            return dp;
        }

        public ActionResult LinkServicesToPricing(List<ServicesModel> detailsService)
        {

            try
            {
                foreach (ServicesModel pm in detailsService)
                {
                    PricingDetailsModel ExistingDetail = PricingDetails_BLL.GetAllDetailsByPricing(pm.codePricing).Where(e => e.CodeDetail == pm.Reference && e.MontantUnitaire == pm.Montant ).FirstOrDefault();
                    if (ExistingDetail != null)
                    {

                        ExistingDetail.Quantite += pm.Qte;
                        ExistingDetail.MontantHorsTaxe = ExistingDetail.MontantUnitaire.Value * ExistingDetail.Quantite.Value;
                        if(ExistingDetail.CodeTaxe != 0)
                        {

                                Taxes tax = Taxes_BLL.GetById(ExistingDetail.CodeTaxe);
                                if (tax != null)
                                {
                                //dp.CodeTaxe = ServiceModel.taxe;
                                ExistingDetail.MontantTaxe = tax.Pourcentage.Value * ExistingDetail.MontantHorsTaxe / 100;
                                }
                            
                        }
                        ExistingDetail.MontantTotal = ExistingDetail.MontantHorsTaxe + ExistingDetail.MontantTaxe;
                        PricingDetails_BLL.Update(GenericModelMapper.GetModel<PricingDetails, PricingDetailsModel>(ExistingDetail));
                    }
                    else
                    {
                        PricingDetails dp = PrepareDetailPricingFromService(pm);
                        PricingDetails_BLL.Insert(dp);
                    }

                }
                Pricing pricing = RefreshTotalPricing(detailsService[0].codePricing);
                return Json(new TextValueModel("OK", "Opération réussite", pricing.MontantTotal, pricing.MontantFinal), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return (Content(e.Message));
            }
        }
        public ActionResult DeleteDetailsPricing(int ID)
        {
            PricingDetails d = PricingDetails_BLL.GetById(ID);
            PricingDetails_BLL.Delete(ID);
            Pricing pricing = RefreshTotalPricing(d.PricingID);
            return Json(new TextValueModel("OK", "Opération réussite", pricing.MontantTotal, pricing.MontantFinal), JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenDetailsPricingEditor(int ID,int PricingID)
        {
            PricingDetailsModel detail = new PricingDetailsModel();
            if (ID != 0)
                detail = GenericModelMapper.GetModel<PricingDetailsModel, PricingDetails>(PricingDetails_BLL.GetById(ID));
            else
            {
                detail.PricingID = PricingID;
                detail.MontantTaxe = 0;
                detail.MontantHorsTaxe = 0;
                detail.Quantite = 1;
                detail.CodeTaxe = 1;
                detail.Remise = 0;

            } Pricing pricing = PricingBLL.GetById(PricingID);
                ViewBag.TypePricing = pricing.Type;
            ViewBag.Defaulttaxe = Taxes_BLL.getDefaultTaxeId();
           
            return PartialView("~/Views/Vente/EditorTemplates/_PricingDetailsEditor.cshtml", detail);
        }
        #endregion

    }
}