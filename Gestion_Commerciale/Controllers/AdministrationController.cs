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
    public class AdministrationController : Controller
    {
        
        // GET: Administration
        public ActionResult Index()
        {
            return View();
        }
        #region banques
        public ActionResult Banque()
        {
            return View();
        }
        public static IList<SelectListItem> GetBanqueList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var Banque = Banque_BLL.GetAll();
            foreach (Banque jf in Banque)
            {
                res.Add(new SelectListItem()
                {
                    Text = jf.NomBanque,
                    Value = jf.ID.ToString()
                });
            }
            return res;
        }
        public JsonResult GetAllBanque()
        {
            List<Banque> Banque = Banque_BLL.GetAll();
            List<BanqueModel> BanqueModelsList = GenericModelMapper.GetModelList<BanqueModel, Banque>(Banque);
            return Json(BanqueModelsList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateBanque(BanqueModel BanqueToAdd)
        {
            try
            {
                Banque j = GenericModelMapper.GetModel<Banque, BanqueModel>(BanqueToAdd);
                if (j.ID == 0)
                {
                    Banque_BLL.Insert(j);
                    return Json(new TextValueModel("OK", "Jour férié ajouté avec succés"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Banque_BLL.Update(j);
                    return Json(new TextValueModel("OK", "Banque modifiée avec succés"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteBanque(int ID)
        {
            //Banque Banque = Banque_BLL.GetById(ID);
            Banque_BLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenBanqueEditor(int ID)
        {
            BanqueModel Banque = new BanqueModel();
            if (ID != 0)
                Banque = GenericModelMapper.GetModel<BanqueModel, Banque>(Banque_BLL.GetById(ID));
            return PartialView("~/Views/Administration/EditorTemplates/_BanqueEditor.cshtml", Banque);
        }
        #endregion
        #region types règlements
        public ActionResult TypeReglement()
        {
            return View();
        }
        public static IList<SelectListItem> GetTypeReglementList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var TypeReglement = TypeReglement_BLL.GetAll();
            foreach (TypeReglement jf in TypeReglement)
            {
                res.Add(new SelectListItem()
                {
                    Text = jf.Libelle,
                    Value = jf.ID.ToString()
                });
            }
            return res;
        }
        public JsonResult GetAllTypeReglement()
        {
            List<TypeReglement> TypeReglement = TypeReglement_BLL.GetAll();
            List<TypeReglementModel> TypeReglementModelsList = GenericModelMapper.GetModelList<TypeReglementModel, TypeReglement>(TypeReglement);
            return Json(TypeReglementModelsList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateTypeReglement(TypeReglementModel TypeReglementToAdd)
        {
            try
            {
                TypeReglement j = GenericModelMapper.GetModel<TypeReglement, TypeReglementModel>(TypeReglementToAdd);
                if (j.ID == 0)
                {
                    TypeReglement_BLL.Insert(j);
                    return Json(new TextValueModel("OK", "Jour férié ajouté avec succés"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TypeReglement_BLL.Update(j);
                    return Json(new TextValueModel("OK", "Type règlement modifié avec succés"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteTypeReglement(int ID)
        {
            //TypeReglement TypeReglement = TypeReglement_BLL.GetById(ID);
            TypeReglement_BLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenTypeReglementEditor(int ID)
        {
            TypeReglementModel TypeReglement = new TypeReglementModel();
            if (ID != 0)
                TypeReglement = GenericModelMapper.GetModel<TypeReglementModel, TypeReglement>(TypeReglement_BLL.GetById(ID));
            return PartialView("~/Views/Administration/EditorTemplates/_TypeReglementEditor.cshtml", TypeReglement);
        }
        #endregion
        #region Comptes bancaires 

        public ActionResult CompteBancaire()
        {
            return View();
        }
        public static IList<SelectListItem> GetCompteBancaireList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var CompteBancaire = CompteBancaire_BLL.GetAll();
            foreach (CompteBancaire jf in CompteBancaire)
            {
                res.Add(new SelectListItem()
                {
                    Text = jf.Libelle,
                    Value = jf.ID.ToString()
                });
            }
            return res;
        }
        public JsonResult GetAllCompteBancaire(string OwnerType)
        {
            List<CompteBancaireModel> CompteBancaire = new List<CompteBancaireModel>();
            switch (OwnerType)
            {
                case "Fournisseur":
                    {
                        CompteBancaire = CompteBancaire_BLL.GetAllComptesFournisseurs();
                    };break;
                case "Client":
                    {
                        CompteBancaire = CompteBancaire_BLL.GetAllComptesClients();
                    }; break;
                case "STE":
                    {
                        CompteBancaire = CompteBancaire_BLL.GetAllComptesSTEModel();
                    }; break;
            }
            
            return Json(CompteBancaire, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateCompteBancaire(CompteBancaireModel CompteBancaireToAdd)
        {
            try
            {
                CompteBancaire j = GenericModelMapper.GetModel<CompteBancaire, CompteBancaireModel>(CompteBancaireToAdd);
                if (j.ID == 0)
                {
                    CompteBancaire_BLL.Insert(j);
                    return Json(new TextValueModel("OK", "Jour férié ajouté avec succés"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    CompteBancaire_BLL.Update(j);
                    return Json(new TextValueModel("OK", "Compte bancaire modifié avec succés"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteCompteBancaire(int ID)
        {
            //CompteBancaire CompteBancaire = CompteBancaire_BLL.GetById(ID);
            CompteBancaire_BLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenCompteBancaireEditor(int ID,string OwnerType)
        {
            CompteBancaireModel CompteBancaire = new CompteBancaireModel();
            if (ID != 0)
                CompteBancaire = GenericModelMapper.GetModel<CompteBancaireModel, CompteBancaire>(CompteBancaire_BLL.GetById(ID));
            else
            CompteBancaire.TypeOwner = OwnerType;
            return PartialView("~/Views/Administration/EditorTemplates/_CompteBancaireEditor.cshtml", CompteBancaire);
        }
        #endregion
        #region Jours fériés
        public ActionResult JoursFeries()
        {
            return View();
        }
        public static IList<SelectListItem> GetJoursFeriesList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var JoursFeries = JoursFeries_BLL.GetAll();
            foreach (JoursFeries jf in JoursFeries)
            {
                res.Add(new SelectListItem()
                {
                    Text = jf.Libelle,
                    Value = jf.ID.ToString()
                });
            }
            return res;
        }
        public JsonResult GetAllJoursFeries()
        {
            List<JoursFeries> JoursFeries = JoursFeries_BLL.GetAll();
            List<JoursFeriesModel> joursFeriesModelsList = GenericModelMapper.GetModelList<JoursFeriesModel, JoursFeries>(JoursFeries);
            return Json(joursFeriesModelsList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateJoursFeries(JoursFeriesModel JoursFeriesToAdd)
        {
            try
            {
                JoursFeries j = GenericModelMapper.GetModel<JoursFeries, JoursFeriesModel>(JoursFeriesToAdd);
                if (j.ID == 0)
                {
                    JoursFeries_BLL.Insert(j);
                    return Json(new TextValueModel("OK", "Jour férié ajouté avec succés"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JoursFeries_BLL.Update(j);
                    return Json(new TextValueModel("OK", "Jour férié modifié avec succés"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteJoursFeries(int ID)
        {
            //JoursFeries JoursFeries = JoursFeries_BLL.GetById(ID);
            JoursFeries_BLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenJoursFeriesEditor(int ID)
        {
            JoursFeriesModel JoursFeries = new JoursFeriesModel();
            if (ID != 0)
                JoursFeries = GenericModelMapper.GetModel<JoursFeriesModel, JoursFeries>(JoursFeries_BLL.GetById(ID));
            return PartialView("~/Views/Administration/EditorTemplates/_JoursFeriesEditor.cshtml", JoursFeries);
        }
        #endregion
      
        #region Taxes
        public ActionResult Taxes()
        {
            return View();
        }
        public static IList<SelectListItem> GetTaxesList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var taxes = Taxes_BLL.GetAll();
            foreach (Taxes taxe in taxes)
            {
                res.Add(new SelectListItem()
                {
                    Text = taxe.NomTaxe,
                    Value = taxe.ID.ToString()
                });
            }
            return res;
        }
       
        public JsonResult GetAllTaxes()
        {
            List<Taxes> Taxes = Taxes_BLL.GetAll();
            List<TaxesModel> taxesModelList = GenericModelMapper.GetModelList<TaxesModel, Taxes>(Taxes);
            return Json(taxesModelList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateTaxes(TaxesModel TaxesToAdd)
        {
            try
            {
                Taxes t = GenericModelMapper.GetModel<Taxes, TaxesModel>(TaxesToAdd);
                if(t.IsDefault)
                {
                    List<Taxes> listTaxes = Taxes_BLL.GetAll().Where(e => e.ID != t.ID).ToList();
                    foreach(Taxes tax in listTaxes)
                    {
                        tax.IsDefault = false;
                        Taxes_BLL.Update(tax);
                    }
                }
                if (t.ID == 0)
                    Taxes_BLL.Insert(t);
                else
                    Taxes_BLL.Update(t);
                return Json(new TextValueModel("OK", "Taxe ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteTaxes(int ID)
        {
            //Taxes Taxes = Taxes_BLL.GetById(ID);
            Taxes_BLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenTaxesEditor(int ID)
        {
            TaxesModel Taxes = new TaxesModel();
            if (ID != 0)
                Taxes = GenericModelMapper.GetModel<TaxesModel, Taxes>(Taxes_BLL.GetById(ID));
            return PartialView("~/Views/Administration/EditorTemplates/_TaxesEditor.cshtml", Taxes);
        }
        #endregion
        #region Motifs congés

        public ActionResult MotifsConges()
        {
            return View();
        }
        public static IList<SelectListItem> GetMotifsCongesList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var motifsconge = MotifCongeBLL.GetAll();
            foreach (MotifConge motif in motifsconge)
            {
                res.Add(new SelectListItem()
                {
                    Text = motif.Libelle,
                    Value = motif.ID.ToString()
                });
            }
            return res;
        }
        public JsonResult GetAllMotifConge()
        {
            List<MotifConge> MotifConge = MotifCongeBLL.GetAll();
            List<MotifCongeModel> fournisseursModelList = GenericModelMapper.GetModelList<MotifCongeModel, MotifConge>(MotifConge);
            return Json(fournisseursModelList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateMotifConge(MotifCongeModel MotifCongeToAdd)
        {
            try
            {
                MotifConge p = GenericModelMapper.GetModel<MotifConge, MotifCongeModel>(MotifCongeToAdd);
                if (p.ID == 0)
                    MotifCongeBLL.Insert(p);
                else
                    MotifCongeBLL.Update(p);
                return Json(new TextValueModel("OK", "MotifConge ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteMotifConge(int ID)
        {
            //MotifConge MotifConge = MotifConge_BLL.GetById(ID);
            MotifCongeBLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenMotifCongeEditor(int ID)
        {
            MotifCongeModel MotifConge = new MotifCongeModel();
            if (ID != 0)
                MotifConge = GenericModelMapper.GetModel<MotifCongeModel, MotifConge>(MotifCongeBLL.GetById(ID));
            return PartialView("~/Views/Administration/EditorTemplates/_MotifCongeEditor.cshtml", MotifConge);
        }
        #endregion

        #region Historique des reclamations

        public ActionResult Reclamations()
        {
            return View();
        }
     
        public JsonResult GetAllReclamations(DateTime? DateFromFilter, DateTime? DateToFilter, int ClientFilter)
        {
            List<ReclamationsModel> ReclamationModelList = new List<ReclamationsModel>();
            if ((DateFromFilter == null || DateFromFilter.Value.Year > 2015) && (DateToFilter == null || DateToFilter.Value.Year > 2015) && ClientFilter != 0)
            {
                ReclamationModelList = Reclamations_BLL.GetReclamationsByClient(DateFromFilter, DateToFilter, ClientFilter);
            }
            else if((DateFromFilter == null || DateFromFilter.Value.Year > 2015) && (DateToFilter == null || DateToFilter.Value.Year > 2015) && ClientFilter == 0)
            {
                ReclamationModelList = Reclamations_BLL.GetReclamationsByDate(DateFromFilter, DateToFilter);

            }
            return Json(ReclamationModelList, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}