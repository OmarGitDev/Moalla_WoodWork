
using Gestion_Commerciale.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSD_BLL;
using TSD_DAL.Model;
using TSD_DAL.Models;
using TSD_DAL.TSD_EDMX;

namespace Gestion_Commerciale.Controllers
{
    [CustomAuthorize]
    public class CommonController : Controller
    {
        public ActionResult FactureDetails(string NumPiece, string Type)
        {
            try
            {
                GenericPieceModel model = Piece_BLL.GetGenericPiece(NumPiece, Type);
                if (model != null)
                    return View(model);
                else
                    return Content("KO");
            }
            catch (Exception e)
            {
                return Content("KO");
            }
        }
        public ActionResult AllNotifications()
        {
            return View();
        }
        public ActionResult GetReglementIDFormDetail(int ID)
        {
            MappingReglementPieces mapp = MappingReglementPiecesBLL.GetById(ID);
            return Content(mapp.ReglementID.ToString());
        }
        public ActionResult GetPieceType(string NumPiece)
        {
            try
            {

                Piece p = Piece_BLL.GetByNumPiece(NumPiece);
                if (p != null)
                    return Content(p.TypePiece);
                else
                    return Content("");
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        public JsonResult GetAllNotifications(DateTime? dateFrom)
        {
            List<NotificationsModel> notificationsList = NotificationsBLL.getAllNotificationsByDateFilter(dateFrom);

            return Json(notificationsList, JsonRequestBehavior.AllowGet);
        }
        public static List<NotificationsModel> GetNotSeenNotifications()
        {
            NotificationsBLL.RefreshNotifications();
            return NotificationsBLL.GetNotSeenNotifications();
        }
        public ActionResult SaveReglementsChanges(int ReglementID, double Montant, DateTime DateReglement, int OwnerId, string Reference, string Remarques, DateTime? DateEcheance, int Banque)
        {
            if (Montant == 0)
            {
                return Json(new TextValueModel("KO", "Veuillez vérifier le montant saisi!"), JsonRequestBehavior.AllowGet);
            }
            //bool RefExist = Reglements_BLL.CheckIfReferenceExist(Reference, ReglementID);
            //if (RefExist)
            //{
            //    return Json(new TextValueModel("KO", "Référence existe déjà!"), JsonRequestBehavior.AllowGet);
            //}
            Reglements reglement = Reglements_BLL.GetById(ReglementID);

            Reglements_BLL.SaveReglementChanges(ReglementID, Montant, DateReglement, OwnerId, Reference, Remarques, DateEcheance, Banque);
            Reglements_BLL.updateClosed(ReglementID);
            NotificationsBLL.LastNotificationRefresh = null;

            return Json(new TextValueModel("", ""), JsonRequestBehavior.AllowGet);
        }
        public ActionResult TransformToCFAC(string NumPiece)
        {
            Piece OldPiece = Piece_BLL.GetByNumPiece(NumPiece);
            Piece NewPiece = OldPiece;
            NewPiece.ID = 0;
            NewPiece.TypePiece = "CFAC";
            NewPiece.Statut = "ECR";
            NewPiece.DateCreation = DateTime.Now;
            NewPiece.LastEditTime = DateTime.Now;
            NewPiece.NumPiece = SeriesBLL.GenerateNewSerie("CFAC");
            Piece_BLL.Insert(NewPiece);
            SeriesBLL.UpdateLastSerie("CFAC");
            GenericPieceModel OldPieceV = PieceVente_BLL.GetPieceVenteByNumPiece(NumPiece);
            PieceVente newPieceV = new PieceVente();
            newPieceV.NumPieceVente = NewPiece.NumPiece;
            newPieceV.CodeClient = OldPieceV.CodeClient;
            PieceVente_BLL.Insert(newPieceV);
            List<DetailsPieceModel> Olddetails = DetailsPiece_BLL.GetDetailsByPiece(NumPiece);
            foreach (var OldDet in Olddetails)
            {
                DetailsPiece det = GenericModelMapper.GetModel<DetailsPiece, DetailsPieceModel>(OldDet);
                det.ID = 0;
                det.Piece = NewPiece.NumPiece;
                DetailsPiece_BLL.Insert(det);
            }
            return (Content(NewPiece.NumPiece));
        }
        public ActionResult SaveFactureChanges(string NumPiece, string Libelle, double? Remise = 0, int? CodeClient = 0, int? CodeFournisseur = 0, string Reference = "")
        {
            PieceModel Piece = Piece_BLL.GetPieceWithTypeByNumPiece(NumPiece);
            Piece_BLL.SaveFactureChanges(NumPiece, Libelle, Remise.Value, Reference);
            if (Piece.Category == "F")
                PieceAchat_BLL.SaveCodeFournisseurChanges(NumPiece, CodeFournisseur);
            else
                PieceVente_BLL.SaveCodeClientChanges(NumPiece, CodeClient);
            Piece_BLL.updateClosed(NumPiece);
            return Json(new TextValueModel(Libelle, "", CodeClient, CodeFournisseur, Remise), JsonRequestBehavior.AllowGet);
        }
        public static IList<SelectListItem> GetStatus()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "Tous",
                Value = "0"
            });
            res.Add(new SelectListItem()
            {
                Text = "En cours",
                Value = "ECR"
            });
            res.Add(new SelectListItem()
            {
                Text = "Validés",
                Value = "VLD"
            });
            res.Add(new SelectListItem()
            {
                Text = "Annulés",
                Value = "ANL"
            });
            return res;
        }
        public static IList<SelectListItem> GetProductTypes()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "Tous",
                Value = "0"
            });
            res.Add(new SelectListItem()
            {
                Text = "Achats",
                Value = "1"
            });
            res.Add(new SelectListItem()
            {
                Text = "Ventes",
                Value = "2"
            });
            res.Add(new SelectListItem()
            {
                Text = "Ventes et Achats",
                Value = "3"
            });
            return res;
        }
        
        public JsonResult GetFactureDetails(string NumPiece)
        {
            List<DetailsPieceModel> details = DetailsPiece_BLL.GetDetailsByPiece(NumPiece);
            foreach (var detail in details)
            {
                Taxes t = Taxes_BLL.GetById(detail.CodeTaxe);
                if (t != null)
                {
                    detail.pourcentageTaxe = t.Pourcentage.ToString() + " %";
                }
                detail.RemiseString = detail.Remise == 0 || detail.Remise == null ? "" : detail.Remise.ToString() + " %";
            }
            return Json(details, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteFacture(string NumPiece)
        {
            Piece Piece = Piece_BLL.GetByNumPiece(NumPiece);
            GenericPieceModel model = Piece_BLL.GetGenericPiece(NumPiece, Piece.TypePiece);
            PieceAchat_BLL.DeleteGenericPiece(model);
            return (Content(""));
        }
        public ActionResult ListPieces(string Type, int CurrentDate = 0)
        {
            switch (Type)
            {
                case "FF":
                    {
                        ViewBag.GridName = "Factures fournisseurs";
                    }; break;
                case "CF":
                    {
                        ViewBag.GridName = "Factures clients";
                    }; break;
                case "BL":
                    {
                        ViewBag.GridName = "Bons de livraison";
                    }; break;
                case "BC":
                    {
                        ViewBag.GridName = "Bons de commandes";
                    }; break;
            }
            ViewBag.CurrentDate = CurrentDate;
            return View(model: Type);
        }
        [HttpPost]
        public string AddNewPiece(GenericPieceModel pieceModel)
        {
            DateTime date = DateTime.Now;
            //string Prefix = pieceModel.TypePiece == "FFAC" ? "FF" : pieceModel.TypePiece == "FNOC" ? "FN" : pieceModel.TypePiece == "CNOC" ? "CN" : pieceModel.TypePiece == "CFAC" ? "FC" : pieceModel.TypePiece == "BCOM" ? "BC" : pieceModel.TypePiece == "BLIV" ? "BL" : "";
            //string dateString = Prefix + date.Year.ToString().Substring(2, 2) + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString();
            Piece piece = new Piece();
            piece.NumPiece = SeriesBLL.GenerateNewSerie(pieceModel.TypePiece);
            //if(piece.NumPiece.Contains("A20"))
            //{
            //    throw new Exception("Erreur lors de la création de la pièce, veuillez contacter le service technique");
            //}
            piece.TypePiece = pieceModel.TypePiece;
            piece.LastEditTime = DateTime.Now;
            piece.DateCreation = DateTime.Now;
            piece.EditedBy = "user";
            piece.CreatedBy = "user";
            piece.Statut = "ECR";
            piece.Libelle = pieceModel.Libelle;
            Piece_BLL.Insert(piece);
            SeriesBLL.UpdateLastSerie(pieceModel.TypePiece);
            switch (pieceModel.TypePiece)
            {
                case "CFAC":

                case "CNOC":
                    {
                        PieceVente pieceVente = new PieceVente();
                        pieceVente.CodeClient = pieceModel.CodeClient;
                        pieceVente.NumPieceVente = piece.NumPiece;
                        PieceVente_BLL.Insert(pieceVente);

                    }; break;
                case "FFAC":
                case "BLIV":
                case "BCOM":
                case "FNOC":
                    {
                        PieceAchat pieceAchat = new PieceAchat();
                        pieceAchat.CodeFournisseur = pieceModel.CodeFournisseur;
                        pieceAchat.NumPieceAchat = piece.NumPiece;
                        PieceAchat_BLL.Insert(pieceAchat);

                    }; break;
            }
            return piece.NumPiece;
        }

        public ActionResult DeleteDetailPiece(int ID)
        {
            DetailsPiece_BLL.Delete(ID);
            return Content("");
        }
        //bug suppression detail piece
        public static Piece RefreshTotalPiece(string NumPiece)
        {

            Piece p = Piece_BLL.GetByNumPiece(NumPiece);
            List<DetailsPieceModel> details = DetailsPiece_BLL.GetDetailsByPiece(NumPiece);
            double MontantTotal = Math.Round(details.Sum(e => e.MontantHorsTaxe.Value), 3);
            double MontantFinal = 0;
            p.RAS = 0;

            if (p.TypePiece == "CFAC" || p.TypePiece == "CNOC")
            {
                ClientModel client = Client_BLL.GetByCodeFacture(NumPiece);

                if (client.Exonere)
                    MontantFinal = Math.Round(details.Sum(e => e.MontantHorsTaxe.Value), 3);
                else
                    MontantFinal = Math.Round(details.Sum(e => e.MontantTotal.Value), 3);
                MontantFinal = client.TFExo ? MontantFinal : MontantFinal + Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.TF)) / 1000;
            }
            else
            {
                MontantFinal = Math.Round(details.Sum(e => e.MontantTotal.Value), 3) + Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.TF)) / 1000;


            }
            if (MontantFinal >= Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RASLimite)))
            {
                p.RAS = MontantFinal * Math.Round((Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RAS)))) / 100;
            }
            p.MontantTotal = MontantTotal;
            p.MontantFinal = MontantFinal;
            Piece_BLL.Update(p);
            Piece_BLL.updateClosed(NumPiece);
            return p;

        }
        //public static double RefreshAutomaticLigne(string NumPiece, double MontantFinal)
        //{
        //    DetailsPiece RASDetail =  DetailsPiece_BLL.GetAutomaticLineByPiece(NumPiece);
        //    if(RASDetail != null)
        //    {
        //        if(MontantFinal < Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RASLimite)))
        //        {
        //            DetailsPiece_BLL.Delete(RASDetail.ID);
        //            return 0;
        //        }
        //        else
        //        {
        //            RASDetail.CodeDetailPiece = "RAS";
        //            RASDetail.MontantTotal = Math.Round((Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RAS)) * MontantFinal) / 100,3);
        //            RASDetail.IsAutomaticlyGenerated = true;
        //            RASDetail.Piece = NumPiece;
        //            RASDetail.Quantite = 1;
        //            RASDetail.Libelle = ParametersApp_BLL.GetParameterValue(Constants.RASLibelle);
        //           // MontantFinal += MontantFinal * Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RASLimite));
        //            DetailsPiece_BLL.Update(RASDetail);
        //            return RASDetail.MontantTotal.Value;
        //        }
        //    }
        //    else
        //    {
        //        if(MontantFinal > Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RASLimite)))
        //        {
        //            RASDetail = new DetailsPiece();
        //            RASDetail.CodeDetailPiece = "RAS";
        //            RASDetail.MontantTotal = Math.Round((Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RAS)) * MontantFinal )/ 100,3);
        //            RASDetail.IsAutomaticlyGenerated = true;
        //            RASDetail.Piece = NumPiece;
        //            RASDetail.Quantite = 1;
        //            RASDetail.Libelle = (string)ParametersApp_BLL.GetParameterValue(Constants.RASLibelle);
        //            //MontantFinal += MontantFinal * Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RASLimite));
        //            DetailsPiece_BLL.Insert(RASDetail);
        //            return RASDetail.MontantTotal.Value;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //}n 

        public ActionResult AddOrUpdateDetailsPiece(DetailsPieceModel DetailsPieceToAdd)
        {
            try
            {
                DetailsPiece p = GenericModelMapper.GetModel<DetailsPiece, DetailsPieceModel>(DetailsPieceToAdd);
                p.Remise = p.Remise == null ? 0 : p.Remise;
                p.MontantHorsTaxe = p.MontantUnitaire * p.Quantite;
                Taxes t = Taxes_BLL.GetById(p.CodeTaxe);
                p.MontantTaxe = 0;
                if (t != null)
                {
                    p.MontantTaxe = p.MontantHorsTaxe * (t.Pourcentage / 100);
                }
                p.MontantTotal = Math.Round(((p.MontantHorsTaxe + p.MontantTaxe) * 1 - (p.Remise == null ? 0 : p.Remise.Value / 100)).Value, 3);
                if (p.ID == 0)
                    DetailsPiece_BLL.Insert(p);
                else
                    DetailsPiece_BLL.Update(p);
                Piece piece = RefreshTotalPiece(p.Piece);
                return Json(new TextValueModel("OK", "Opération réussite", piece.MontantTotal, piece.MontantFinal, piece.RAS), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteDetailsPiece(int ID)
        {
            DetailsPiece d = DetailsPiece_BLL.GetById(ID);
            DetailsPiece_BLL.Delete(ID);
            Piece piece = RefreshTotalPiece(d.Piece);
            return Json(new TextValueModel("OK", "Opération réussite", piece.MontantTotal, piece.MontantFinal, piece.RAS), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAllDetailsPiece(string numPiece)
        {
            List<DetailsPieceModel> dpml = DetailsPiece_BLL.GetDetailsByPiece(numPiece);
            foreach (DetailsPieceModel dpm in dpml)
            {
                DetailsPiece_BLL.Delete(dpm.ID);
            }
            Piece piece = RefreshTotalPiece(numPiece);
            return Json(new TextValueModel("OK", "Opération réussite", piece.MontantTotal, piece.MontantFinal, piece.RAS), JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenDetailsPieceEditor(int ID, string NumPiece)
        {
            bool isExonere = false;
            Piece piece = Piece_BLL.GetByNumPiece(NumPiece);
            if (piece.TypePiece == "CFAC" || piece.TypePiece == "CNOC")
            {
                int Client = PieceVente_BLL.GetClientByNumPiece(NumPiece);
                Client c = Client_BLL.GetById(Client);
                if (c.Exonere)
                {
                    isExonere = true;
                }
            }
            DetailsPieceModel detail = new DetailsPieceModel();
            if (ID != 0)
                detail = GenericModelMapper.GetModel<DetailsPieceModel, DetailsPiece>(DetailsPiece_BLL.GetById(ID));
            else
            {

                //detail.CodeTaxe
                detail.Piece = NumPiece;
                detail.Remise = 0;
                detail.Quantite = 1;
            }

            ViewBag.Defaulttaxe = isExonere ? 0 : Taxes_BLL.getDefaultTaxeId();
            return PartialView("~/Views/Common/EditorTemplates/_PieceDetailsEditor.cshtml", detail);
        }
        public ActionResult OpenMaterialReglementEditor(int id,int reglementID)
        {
            MaterialReglementDetails materialsReglement = new MaterialReglementDetails();
            if (id != 0)
                materialsReglement = MaterialReglementDetailsBLL.GetById(id);
            else
                materialsReglement.ReglementID = reglementID;
            MaterialReglementDetailsModel resultModel = GenericModelMapper.GetModel<MaterialReglementDetailsModel, MaterialReglementDetails>(materialsReglement);
            return PartialView("~/Views/Common/EditorTemplates/_MaterialReglementEditor.cshtml", resultModel);
        }
        public ActionResult AddOrUpdateMaterialReglement(MaterialReglementDetailsModel materialReglementDetailsModel)
        {
            try
            {
                MaterialReglementDetails materialReglementDetails = GenericModelMapper.GetModel<MaterialReglementDetails, MaterialReglementDetailsModel>(materialReglementDetailsModel);
                Reglements reglements = Reglements_BLL.GetById(materialReglementDetailsModel.ReglementID);
                List<MaterialReglementDetailsModel> detailsListByReglement = MaterialReglementDetailsBLL.GetMaterialDetailsByReglement(materialReglementDetailsModel.ReglementID);
                if (reglements.Montant < (detailsListByReglement.Sum(e => e.Amount)+ materialReglementDetailsModel.Amount))
                {
                    return Json(new TextValueModel("KO", "La somme des montants dépasse le montant du règlement"), JsonRequestBehavior.AllowGet);
                }
                if (materialReglementDetails.ID == 0)
                    MaterialReglementDetailsBLL.Insert(materialReglementDetails);
                else
                    MaterialReglementDetailsBLL.Update(materialReglementDetails);

                return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteMaterialReglement(int ID)
        {


            MaterialReglementDetailsBLL.Delete(ID);
            return Content("");
        }
        public static IList<SelectListItem> GetAllTaxesList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var taxes = Taxes_BLL.GetAll();
            foreach (var f in taxes)
            {
                res.Add(new SelectListItem()
                {
                    Text = f.NomTaxe,
                    Value = f.ID.ToString()
                });
            }
            return res;
        }


        public ActionResult OpenPieceEditorToAdd(string Type)
        {
            GenericPieceModel Piece = new GenericPieceModel();
            Piece.TypePiece = Type;
            return PartialView("~/Views/Common/EditorTemplates/_PieceEditor.cshtml", Piece);
        }
        public List<PieceModel> GetListPiecesForReglement(string Type, string CodeFilter = "", string LibelleFilter = "", DateTime? DateFromFilter = null, DateTime? DateToFilter = null, double? MontantMinFilter = null, double? MontantMaxFilter = null, int ClientFilter = 0, int FournisseurFilter = 0)
        {
            List<PieceModel> gpm = new List<PieceModel>();
            switch (Type)
            {
                case "FFAC":
                case "FNOC":
                case "BCOM":
                case "BLIV":
                    {
                        gpm = Piece_BLL.GetAllPiecesFournisseursForReglement(Type, CodeFilter, LibelleFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter, FournisseurFilter);
                    }; break;
                case "CFAC":

                case "CNOC":
                    {
                        gpm = Piece_BLL.GetAllPiecesClientsForReglement(Type, CodeFilter, LibelleFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter, ClientFilter);
                    }; break;
            }
            return gpm;
        }
        public List<PieceModel> GetListPiecesForRASReglement(string Type, string CodeFilter = "", string LibelleFilter = "", DateTime? DateFromFilter = null, DateTime? DateToFilter = null, double? MontantMinFilter = null, double? MontantMaxFilter = null, int ClientFilter = 0, int FournisseurFilter = 0)
        {
            List<PieceModel> gpm = new List<PieceModel>();
            switch (Type)
            {
                case "FFAC":
                case "FNOC":
                case "BCOM":
                case "BLIV":
                    {
                        gpm = Piece_BLL.GetAllPiecesFournisseursForRASReglement(Type, CodeFilter, LibelleFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter, FournisseurFilter);
                    }; break;
                case "CFAC":

                case "CNOC":
                    {
                        gpm = Piece_BLL.GetAllPiecesClientsForRASReglement(Type, CodeFilter, LibelleFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter, ClientFilter);
                    }; break;
            }
            return gpm;
        }
        public JsonResult GetAllMaterialDetailsByReglement(int reglementID)
        {
            List<MaterialReglementDetailsModel> result = MaterialReglementDetailsBLL.GetMaterialDetailsByReglement(reglementID).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAllPiecesForReglements(string Type, string CodeFilter, string LibelleFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter, int ClientFilter, int FournisseurFilter, bool RAS = false)
        {
            List<PieceModel> gpm = new List<PieceModel>();
            if (!RAS)
            {
                gpm = GetListPiecesForReglement(Type, CodeFilter, LibelleFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter, ClientFilter, FournisseurFilter);
            }
            else
            {
                gpm = GetListPiecesForRASReglement(Type, CodeFilter, LibelleFilter, DateFromFilter, DateToFilter, Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RASLimite)), MontantMaxFilter, ClientFilter, FournisseurFilter);
            }
            gpm.ForEach(e => e.Solde = e.MontantFinal - Math.Abs(MappingReglementPiecesBLL.GetSumMappingsByPiece(e.NumPiece, RAS)));
            gpm = gpm.Where(e => e.Statut == "VLD" && e.Solde != 0).ToList();
            //gpm.ForEach(e => e.Statut = e.Statut == "VLD" ? "Validée" : e.Statut == "ECR" ? "En cours" : "Annulée");
            return Json(gpm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllMappingsForReglements(int ReglementID)
        {
            List<MappingReglementPieces> mappingModels = MappingReglementPiecesBLL.GetMappingsByReglement(ReglementID);
            return Json(mappingModels, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllPieces(string Type, string CodeFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter, int? ClientFilter, int? FournisseurFilter, string etat, string StatusFilter = "0")
        {
            List<GenericPieceModel> gpm = new List<GenericPieceModel>();
            switch (Type)
            {
                case "FFAC":
                case "FNOC":
                case "BCOM":
                case "BLIV":
                    {
                        gpm = Piece_BLL.GetAllPiecesFournisseurs(Type, CodeFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter, FournisseurFilter, StatusFilter);
                    }; break;
                case "CFAC":

                case "CNOC":
                    {
                        gpm = Piece_BLL.GetAllPiecesClientsByType(Type, CodeFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter, ClientFilter, StatusFilter);
                    }; break;
            }
            gpm.ForEach(e => e.SoldeNet = e.MontantFinal - e.RAS - Math.Abs(MappingReglementPiecesBLL.GetSumMappingsByPiece(e.NumPiece, false)));
            gpm.ForEach(e => e.SoldeRAS = e.RAS - Math.Abs(MappingReglementPiecesBLL.GetSumMappingsByPiece(e.NumPiece, true)));
            gpm.ForEach(e => e.Solde = e.SoldeNet  + e.SoldeRAS);
            gpm.ForEach(e => e.Statut = e.Statut == "VLD" ? "Validée" : e.Statut == "ECR" ? "En cours" : "Annulée");
            if (etat == "Ouvert")
            {
                gpm = gpm.Where(elt => elt.Solde != 0).ToList();
            }
            return Json(gpm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddNewDetailPiece(DetailsPieceModel detailPieceModel)
        {

            DateTime date = DateTime.Now;
            string dateString = "DP" + date.Year.ToString().Substring(2, 2) + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString();
            DetailsPiece detailPiece = new DetailsPiece();
            detailPiece.CodeDetailPiece = dateString;
            detailPiece.Quantite = detailPieceModel.Quantite;
            detailPiece.Libelle = detailPieceModel.Libelle;
            DetailsPiece_BLL.Insert(detailPiece);
            return Content("");
        }
        public JsonResult GetAllPricingDetailsSelector(int PricingSelector)
        {
            List<PricingDetailsModel> PricingDetails = PricingDetails_BLL.GetAllDetailsByPricing(PricingSelector);
            return Json(PricingDetails, JsonRequestBehavior.AllowGet);
        }
        public static List<string> StarsSplitter(string concatenatedList)
        {
            List<string> separator = new List<string>();
            separator.Add("**");
            return (concatenatedList.Split(separator.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        public ActionResult OpenEditorToFillMappings(string SelectedItems, int ReglementID)
        {

            List<string> SelectedItemsList = StarsSplitter(SelectedItems);
            List<int> SelectedItemsListInt = SelectedItemsList.Select(e => int.Parse(e)).ToList();
            List<Piece> pieces = PieceAchat_BLL.GetPiecesByIDs(SelectedItemsListInt).OrderBy(e => e.DateCreation).ToList();
            List<PieceModel> pml = new List<PieceModel>();
            Reglements r = Reglements_BLL.GetById(ReglementID);
            List<MappingReglementPieces> mappings = MappingReglementPiecesBLL.GetMappingsByReglement(ReglementID);
            double mappSum = 0;
            if (mappings.Count() > 0)
            {
                mappSum = mappings.Sum(e => e.Montant);
            }
            double montant = r.Montant - Math.Abs(mappSum);
            foreach (Piece piece in pieces)
            {
                PieceModel pm = GenericModelMapper.GetModel<PieceModel, Piece>(piece);
                pm.MontantReglee = Math.Abs(MappingReglementPiecesBLL.GetSumMappingsByPiece(pm.NumPiece, r.RAS));
                if (r.RAS)
                {
                    pm.MontantARegler = pm.RAS - pm.MontantReglee <= montant ? pm.RAS - pm.MontantReglee : montant;
                    montant -= pm.MontantARegler.Value;
                }
                else
                {

                    pm.MontantFinal -= pm.RAS;
                    pm.MontantARegler = pm.MontantFinal - pm.MontantReglee <= montant ? pm.MontantFinal - pm.MontantReglee : montant;
                    montant -= pm.MontantARegler.Value;
                }
                pm.ReglementID = ReglementID;
                pml.Add(pm);

            }
            ViewBag.MontantTotal = r.Montant;
            ViewBag.SumPieces = r.Montant - montant;

            return (PartialView("~/Views/Common/EditorTemplates/_AddReglemntsMappingFromPiece.cshtml", pml));
        }
        public ActionResult OpenEditorToFillFacture(string SelectedServices, string numPiece)
        {
            bool isExonere = false;
            Piece piece = Piece_BLL.GetByNumPiece(numPiece);
            if (piece.TypePiece == "CFAC" || piece.TypePiece == "CNOC")
            {
                int Client = PieceVente_BLL.GetClientByNumPiece(numPiece);
                Client c = Client_BLL.GetById(Client);
                if (c.Exonere)
                {
                    isExonere = true;
                }
            }
            List<string> SelectedProuctList = StarsSplitter(SelectedServices);
            List<ServicesModel> pml = new List<ServicesModel>();
            foreach (string Service in SelectedProuctList)
            {
                Services p = ServicesBLL.GetById(int.Parse(Service));
                ServicesModel pm = GenericModelMapper.GetModel<ServicesModel, Services>(p);
                pm.Qte = 1;
                pml.Add(pm);

            }
            ViewBag.Defaulttaxe = isExonere ? 0 : Taxes_BLL.getDefaultTaxeId();
            ViewBag.numPiece = numPiece;
            ViewBag.TypePiece = piece.TypePiece;
            return (PartialView("~/Views/Common/EditorTemplates/_FillInvoiceFromServices.cshtml", pml));
        }

        public ActionResult SetInaccounted(string NumPiece)
        {
            try
            {
                Piece p = Piece_BLL.GetByNumPiece(NumPiece);
                p.Comptabilise = false;
                Piece_BLL.Update(p);
                return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SetAccounted(string NumPiece)
        {
            try
            {
                Piece p = Piece_BLL.GetByNumPiece(NumPiece);
                p.Comptabilise = true;
                Piece_BLL.Update(p);
                return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult OpenEditorToFillFactureFromPricing(string SelectedPricing, string numPiece)
        {
            bool isExonere = false;
            Piece piece = Piece_BLL.GetByNumPiece(numPiece);
            if (piece.TypePiece == "CFAC" || piece.TypePiece == "CNOC")
            {
                int Client = PieceVente_BLL.GetClientByNumPiece(numPiece);
                Client c = Client_BLL.GetById(Client);
                if (c.Exonere)
                {
                    isExonere = true;
                }
            }
            List<string> SelectedPricingDetailsList = StarsSplitter(SelectedPricing);
            List<PricingDetailsModel> dds = new List<PricingDetailsModel>();
            foreach (string dd in SelectedPricingDetailsList)
            {
                PricingDetails d = PricingDetails_BLL.GetById(int.Parse(dd));
                PricingDetailsModel dModel = GenericModelMapper.GetModel<PricingDetailsModel, PricingDetails>(d);
                dModel.Remise = 0;
                dds.Add(dModel);
            }
            ViewBag.Defaulttaxe = isExonere ? 0 : Taxes_BLL.getDefaultTaxeId();
            ViewBag.numPiece = numPiece;
            return (PartialView("~/Views/Common/EditorTemplates/_FillInvoiceFromPricing.cshtml", dds));
        }
        public ActionResult LinkPricingToInvoice(string numPiece, int PricingId)
        {
            DetailsPiece dpiece = new DetailsPiece();
            List<PricingDetailsModel> pdl = PricingDetails_BLL.GetAllDetailsByPricing(PricingId);
            foreach (PricingDetailsModel pd in pdl)
            {
                dpiece.CodeDetailPiece = pd.CodeDetail;
                dpiece.MontantUnitaire = pd.MontantUnitaire;
                dpiece.Libelle = pd.Libelle;
                dpiece.Quantite = pd.Quantite;
                dpiece.MontantTotal = pd.MontantTotal;
                dpiece.Piece = numPiece;
                DetailsPiece_BLL.Insert(dpiece);
            }
            return (Content(""));
        }
        public DetailsPiece PrepareDetailPieceFromService(ServicesModel ServiceModel)
        {

            DetailsPiece dp = new DetailsPiece();
            dp.Piece = ServiceModel.numPiece;
            dp.Quantite = ServiceModel.Qte;
            dp.MontantUnitaire = ServiceModel.Montant;
            dp.Libelle = ServiceModel.Libelle;
            dp.CodeDetailPiece = ServiceModel.Reference;
            dp.MontantHorsTaxe = ServiceModel.Montant * ServiceModel.Qte;
            dp.MontantTaxe = 0;
            if (ServiceModel.taxe != 0)
            {
                Taxes tax = Taxes_BLL.GetById(ServiceModel.taxe);
                if (tax != null)
                {
                    dp.CodeTaxe = ServiceModel.taxe;
                    dp.MontantTaxe = tax.Pourcentage * dp.MontantHorsTaxe / 100;
                }
            }

            dp.Remise = 0;
            dp.MontantTotal = Math.Round(((dp.MontantHorsTaxe + dp.MontantTaxe)).Value, 3);
            return dp;
        }

        public ActionResult LinkServicesToInvoice(List<ServicesModel> detailsService)
        {

            try
            {
                string NumPiece = detailsService[0].numPiece;
                ClientModel client = Client_BLL.GetByCodeFacture(NumPiece);
                //if (detailsService.Count()>0)
                //{

                //}
                foreach (ServicesModel pm in detailsService.Where(e => e.Montant != null))
                {
                    DetailsPieceModel ExistingDetail = DetailsPiece_BLL.GetDetailsByPiece(pm.numPiece).Where(e => e.CodeDetailPiece == pm.Reference && e.MontantUnitaire == pm.Montant).FirstOrDefault();
                    if (ExistingDetail != null)
                    {

                        ExistingDetail.Quantite += pm.Qte;
                        ExistingDetail.MontantTaxe = 0;
                        ExistingDetail.MontantHorsTaxe = ExistingDetail.MontantUnitaire * ExistingDetail.Quantite;
                        if (ExistingDetail.CodeTaxe != 0 && client != null)
                        {

                            Taxes tax = Taxes_BLL.GetById(ExistingDetail.CodeTaxe);
                            if (tax != null)
                            {
                                //dp.CodeTaxe = ServiceModel.taxe;
                                ExistingDetail.MontantTaxe = client.Exonere ? 0 : tax.Pourcentage.Value * ExistingDetail.MontantHorsTaxe / 100;
                            }

                        }

                        ExistingDetail.MontantTotal = ExistingDetail.MontantHorsTaxe + ExistingDetail.MontantTaxe;
                        DetailsPiece_BLL.Update(GenericModelMapper.GetModel<DetailsPiece, DetailsPieceModel>(ExistingDetail));
                    }
                    else
                    {
                        DetailsPiece dp = PrepareDetailPieceFromService(pm);
                        DetailsPiece_BLL.Insert(dp);
                    }

                }
                Piece piece = RefreshTotalPiece(detailsService[0].numPiece);
                return Json(new TextValueModel("OK", "Opération réussite", piece.MontantTotal, piece.MontantFinal, piece.RAS), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return (Content(e.Message));
            }
        }

        public DetailsPiece PrepareDetailPieceFromPricing(PricingDetailsModel pdm)
        {
            DetailsPiece dp = new DetailsPiece();
            dp.Quantite = pdm.Quantite;
            dp.MontantUnitaire = pdm.MontantUnitaire;
            dp.MontantHorsTaxe = pdm.MontantUnitaire * pdm.Quantite;
            dp.Libelle = pdm.Libelle;
            dp.Piece = pdm.numPiece;
            dp.CodeDetailPiece = pdm.CodeDetail;
            dp.Remise = pdm.Remise;
            dp.CodeTaxe = pdm.CodeTaxe;
            Taxes t = Taxes_BLL.GetById(pdm.CodeTaxe);
            dp.MontantTaxe = 0;
            if (t != null)
                dp.MontantTaxe = dp.MontantHorsTaxe * (t.Pourcentage / 100);
            dp.MontantTotal = Math.Round(((dp.MontantHorsTaxe + dp.MontantTaxe) * 1 - (dp.Remise == null ? 0 : dp.Remise.Value / 100)).Value, 3);


            return dp;
        }

        public ActionResult AddReglementMappings(List<PieceModel> Pieces)
        {

            try
            {
                Reglements r = Reglements_BLL.GetById(Pieces[0].ReglementID);

                List<MappingReglementPieces> ReglementMappings = MappingReglementPiecesBLL.GetMappingsByReglement(Pieces[0].ReglementID);
                foreach (PieceModel pm in Pieces.Where(e => e.MontantARegler.Value > 0))
                {
                    MappingReglementPieces PieceMapping = ReglementMappings.Where(e => e.PieceID == pm.NumPiece).FirstOrDefault();
                    if (PieceMapping != null)
                    {
                        PieceMapping.Montant += pm.MontantARegler.Value;
                        MappingReglementPiecesBLL.Update(PieceMapping);

                    }
                    else
                    {
                        MappingReglementPieces mapping = new MappingReglementPieces();
                        mapping.ReglementID = pm.ReglementID;
                        mapping.PieceID = pm.NumPiece;
                        mapping.Montant = int.Parse(r.Sens) * pm.MontantARegler.Value;
                        MappingReglementPiecesBLL.Insert(mapping);
                    }
                    Piece_BLL.updateClosed(pm.NumPiece);
                }
                Reglements_BLL.updateClosed(r.ID);
                return (Content("OK"));
            }
            catch (Exception e)
            {
                return (Content(e.Message));
            }
        }
        public ActionResult LinkPricingDetailsToInvoice(List<PricingDetailsModel> detailsPricing)
        {

            try
            {
                foreach (PricingDetailsModel pm in detailsPricing)
                {
                    DetailsPieceModel ExistingDetail = DetailsPiece_BLL.GetDetailsByPiece(pm.numPiece).Where(e => e.CodeDetailPiece == pm.CodeDetail && e.Remise == pm.Remise && e.MontantUnitaire == pm.MontantUnitaire && pm.CodeTaxe == e.CodeTaxe).FirstOrDefault();
                    if (ExistingDetail != null)
                    {

                        ExistingDetail.Quantite += pm.Quantite;
                        ExistingDetail.MontantHorsTaxe = ExistingDetail.MontantUnitaire * ExistingDetail.Quantite;
                        DetailsPiece_BLL.Update(GenericModelMapper.GetModel<DetailsPiece, DetailsPieceModel>(ExistingDetail));
                    }
                    else
                    {

                        DetailsPiece dp = PrepareDetailPieceFromPricing(pm);
                        DetailsPiece_BLL.Insert(dp);
                    }
                }
                RefreshTotalPiece(detailsPricing[0].numPiece);
                return (Content("OK"));
            }
            catch (Exception e)
            {
                return (Content(e.Message));
            }
        }

        public ActionResult ChangeFactureStatus(string numPiece, string status)
        {
            string ErrorMSG = "";
            Piece p = Piece_BLL.GetByNumPiece(numPiece);
            List<DetailsPieceModel> dpl = new List<DetailsPieceModel>();
            switch (status)
            {
                case "ECR":
                case "ANL":
                    {
                        if (p.Statut == "VLD")
                        {
                            p.Comptabilise = false;
                        }

                        break;
                    }
                case "VLD":
                    {


                        break;
                    }
            }
            if (ErrorMSG == "")
            {
                p.LastEditTime = DateTime.Now;
                p.Statut = status;
                Piece_BLL.Update(p);
                return (Content("OK"));
            }
            else
                return Content(ErrorMSG);
        }

        public JsonResult GetReglementsFacture(string NumPiece)
        {
            List<ReglementsModel> details = Reglements_BLL.getReglementsByFacture(NumPiece);
            return Json(details, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenReglementsGridSelector(string NumPiece, string TypePiece)
        {

            int ownerCode = 0;
            string OwnerType = "";
            switch (TypePiece)
            {
                case "FFAC":
                case "FNOC":
                case "BLIV":
                case "BCOM":
                    {
                        ownerCode = PieceAchat_BLL.GetFournisseurByNumPiece(NumPiece);
                        OwnerType = "F";
                    }; break;
                case "CFAC":
                case "CNOC":

                    {
                        OwnerType = "C";
                        ownerCode = PieceVente_BLL.GetClientByNumPiece(NumPiece);
                    }; break;

            }
            List<ReglementsModel> listReglements = Reglements_BLL.getOpenedReglementsByOwnerCode(ownerCode, OwnerType);
            listReglements.ForEach(e => e.ApplicatifSens = TypePiece == "FFAC" || TypePiece == "CNOC" ? e.Sens == "-1" ? "1" : "-1" : e.Sens);

            return PartialView("~/Views/Common/EditorTemplates/_ReglementsGridSelector.cshtml", listReglements.OrderBy(e => e.Sens).ToList());
        }
        public ActionResult AddOrUpdateReglements(ReglementsModel ReglementsToAdd)
        {
            try
            {

                if (ReglementsToAdd.Montant == 0)
                {
                    return Json(new TextValueModel("KO", "Veuillez vérifier le montant saisi!"), JsonRequestBehavior.AllowGet);
                }
                Reglements p = new Reglements();
                if (ReglementsToAdd.ID == 0)
                {
                    TypeReglement typereg = TypeReglement_BLL.GetById(ReglementsToAdd.TypeReglement);
                    ReglementsToAdd.RAS = typereg.Libelle == "Retenue à la source";
                    PieceModel piece = Piece_BLL.GetPieceWithTypeByNumPiece(ReglementsToAdd.numPiece);
                    if (piece.Category == "F")
                    {
                        GenericPieceModel pa = PieceAchat_BLL.GetPieceAchatByNumPiece(ReglementsToAdd.numPiece);
                        ReglementsToAdd.OwnerId = pa.CodeFournisseur.Value;
                    }
                    else
                    {
                        GenericPieceModel pv = PieceVente_BLL.GetPieceVenteByNumPiece(ReglementsToAdd.numPiece);
                        ReglementsToAdd.OwnerId = pv.CodeClient.Value;
                    }
                    bool IsExceed = MappingReglementPiecesBLL.CheckIfTotalAmmountIsExceed(ReglementsToAdd.numPiece, ReglementsToAdd.Montant, ReglementsToAdd.ID, ReglementsToAdd.Sens, ReglementsToAdd.RAS);
                    if (IsExceed)
                        return Json(new TextValueModel("KO", "Le montant des réglements dépasse le montant de la facture"), JsonRequestBehavior.AllowGet);
                    p = GenericModelMapper.GetModel<Reglements, ReglementsModel>(ReglementsToAdd);

                    Reglements_BLL.Insert(p);
                    NotificationsBLL.LastNotificationRefresh = null;
                    MappingReglementPieces mapping = new MappingReglementPieces();
                    mapping.PieceID = ReglementsToAdd.numPiece;
                    mapping.ReglementID = p.ID;
                    mapping.Montant = p.Montant * int.Parse(p.Sens);
                    MappingReglementPiecesBLL.Insert(mapping);

                }

                else
                {
                    //Reglements_BLL.Update(p);

                    MappingReglementPieces mapping = MappingReglementPiecesBLL.GetById(ReglementsToAdd.ID);
                    Reglements reg = Reglements_BLL.GetById(mapping.ReglementID);
                    p.ID = reg.ID;
                    double mappingsMontant = MappingReglementPiecesBLL.GetSumMappingsByReglement(reg.ID);

                    if (reg.Montant < Math.Abs(mappingsMontant - mapping.Montant + (int.Parse(ReglementsToAdd.Sens) * ReglementsToAdd.Montant)))
                    {
                        return Json(new TextValueModel("KO", "Le montant maximum de ce reglement est " + (reg.Montant - (Math.Abs(mappingsMontant - mapping.Montant)))), JsonRequestBehavior.AllowGet);
                    }
                    mapping.Montant = ReglementsToAdd.Montant * int.Parse(ReglementsToAdd.Sens);
                    MappingReglementPiecesBLL.Update(mapping);
                    //MappingReglementPiecesBLL.Delete(mappingToDelete.ID);

                }
                Reglements_BLL.updateClosed(p.ID);
                Piece_BLL.updateClosed(ReglementsToAdd.numPiece);
                return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteReglements(int ID)
        {
            MappingReglementPieces mapp = MappingReglementPiecesBLL.GetById(ID);
            Reglements reglement = Reglements_BLL.GetById(mapp.ReglementID);
            List<MappingReglementPieces> mappings = MappingReglementPiecesBLL.GetMappingsByReglement(mapp.ReglementID);

            //MappingReglementPieces mapping = mappings.Where(e => e.PieceID == numPiece).FirstOrDefault();
            MappingReglementPiecesBLL.Delete(ID);
            Reglements_BLL.updateClosed(reglement.ID);
            Piece_BLL.updateClosed(mapp.PieceID);
            if (mappings.Count() == 1 && mapp.Montant == reglement.Montant)
                Reglements_BLL.Delete(mapp.ReglementID);
            return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAllReglements(string numPiece)
        {
            List<ReglementsModel> dpml = Reglements_BLL.getReglementsByFacture(numPiece);
            foreach (ReglementsModel dpm in dpml)
            {
                Reglements_BLL.Delete(dpm.ID);
            }
            return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenReglementsEditor(int ID, string NumPiece)
        {
            ReglementsModel reglement = new ReglementsModel();
            PieceModel p = Piece_BLL.GetPieceWithTypeByNumPiece(NumPiece);

            if (ID != 0)
            {
                MappingReglementPieces mapping = MappingReglementPiecesBLL.GetById(ID);
                if (mapping != null)
                {
                    reglement = GenericModelMapper.GetModel<ReglementsModel, Reglements>(Reglements_BLL.GetById(mapping.ReglementID));
                    reglement.Montant = Math.Abs(mapping.Montant);
                }
            }
            else
            {
                double MontantFinal = p.MontantFinal - p.RAS;
                List<MappingReglementPiecesModel> mappings = MappingReglementPiecesBLL.GetMappingsByPiece(NumPiece).Where(e => !e.RAS).ToList();
                if (mappings.Count() > 0)
                    reglement.Montant = MontantFinal - mappings.Sum(e => e.Montant);
                else
                    reglement.Montant = MontantFinal;
                reglement.OwnerType = p.Category;
                reglement.Sens = p.Sens;
                reglement.numPiece = NumPiece;
                reglement.DateReglement = DateTime.Now;
            }

            return PartialView("~/Views/Common/EditorTemplates/_ReglementsEditor.cshtml", reglement);
        }
        public ActionResult ExternalReglements(string owner, int CurrentDate)
        {
            switch (owner)
            {
                case "F":
                    {
                        ViewBag.GridName = "Règlements fournisseurs";
                    }; break;
                case "C":
                    {
                        ViewBag.GridName = "Règlements clients";
                    }; break;

            }
            ViewBag.CurrentDate = CurrentDate;
            return View(model: owner);
        }
        public ActionResult GetReglementIDByRef(string Reglement)
        {
            try
            {
                Reglements reg = Reglements_BLL.GetReglementByReference(Reglement);
                if (reg != null)
                    return Content(reg.ID.ToString());
                return Content("");
            }
            catch (Exception e)
            {
                return Content("");
            }
        }
        public ActionResult GetExternalReglements(string Owner, int OwnerFilter, string LibelleFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter)
        {
            List<ReglementsModel> externalReglements = new List<ReglementsModel>();
            switch (Owner)
            {
                case "F":
                    {
                        externalReglements = Reglements_BLL.GetAllReglementsFournisseur(OwnerFilter, LibelleFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter);
                        break;
                    }
                case "C":
                    {
                        externalReglements = Reglements_BLL.GetAllReglementsClient(OwnerFilter, LibelleFilter, DateFromFilter, DateToFilter, MontantMinFilter, MontantMaxFilter);
                        break;
                    }
            }
            return Json(externalReglements, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenExternalReglementsPiecesList(int ID)
        {
            return (PartialView("~/Views/Common/EditorTemplates/_PiecesListForReglement.cshtml", ID));
        }
        public ActionResult OpenExternalReglementsEditor(string OwnerType)
        {
            ReglementsModel reglement = new ReglementsModel();
            reglement.OwnerType = OwnerType;
            reglement.Montant = 0;
            reglement.DateReglement = DateTime.Now;
            reglement.automaticAttach = false;
            switch (OwnerType)
            {
                case "C":
                    reglement.Sens = "1";
                    break;
                case "F":
                    reglement.Sens = "-1";
                    break;
            }
            return PartialView("~/Views/Common/EditorTemplates/_ExternalReglementEditor.cshtml", reglement);
        }
        public static IList<SelectListItem> GetTypesReglementsList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var typeReglement = TypeReglement_BLL.GetAll();
            foreach (var t in typeReglement)
            {
                res.Add(new SelectListItem()
                {
                    Text = t.Libelle,
                    Value = t.ID.ToString()
                });
            }
            return res;
        }

        public ActionResult AddNewExternalReglement(ReglementsModel reglementModel)
        {
            //bool RefExist = Reglements_BLL.CheckIfReferenceExist(reglementModel.Reference, reglementModel.ID);
            //if (RefExist)
            //{
            //    return Json(new TextValueModel("KO", "Référence existe déjà!"), JsonRequestBehavior.AllowGet);
            //}
            DateTime date = DateTime.Now;
            TypeReglement typereg = TypeReglement_BLL.GetById(reglementModel.TypeReglement);
            reglementModel.RAS = typereg.Libelle == "Retenue à la source";
            Reglements reglement = GenericModelMapper.GetModel<Reglements, ReglementsModel>(reglementModel);

            Reglements_BLL.Insert(reglement);
            NotificationsBLL.LastNotificationRefresh = null;

            if (reglementModel.automaticAttach)
            {
                if (!reglementModel.RAS)
                {
                    List<PieceModel> gpm = GetListPiecesForReglement(reglement.OwnerType + "FAC", ClientFilter: reglement.OwnerId, FournisseurFilter: reglement.OwnerId).OrderBy(e => e.DateCreation).ToList();
                    double montant = reglement.Montant;
                    foreach (PieceModel pm in gpm)
                    {
                        if (montant > 0)
                        {
                            MappingReglementPieces mapping = new MappingReglementPieces();
                            mapping.PieceID = pm.NumPiece;
                            mapping.ReglementID = reglement.ID;
                            double MontantReglee = MappingReglementPiecesBLL.GetSumMappingsByPiece(pm.NumPiece, false);
                            mapping.Montant = int.Parse(reglement.Sens) * pm.MontantFinal - MontantReglee <= montant ? pm.MontantFinal - MontantReglee : montant;
                            montant -= pm.MontantFinal - MontantReglee <= montant ? pm.MontantFinal - MontantReglee : montant;
                            MappingReglementPiecesBLL.Insert(mapping);
                            Piece_BLL.updateClosed(pm.NumPiece);
                        }
                    }
                    Reglements_BLL.updateClosed(reglement.ID);
                }
                else
                {
                    List<PieceModel> gpm = GetListPiecesForRASReglement(reglement.OwnerType + "FAC", ClientFilter: reglement.OwnerId, FournisseurFilter: reglement.OwnerId, MontantMinFilter: Double.Parse(ParametersApp_BLL.GetParameterValue(Constants.RASLimite))).OrderBy(e => e.DateCreation).ToList();
                    double montant = reglement.Montant;
                    foreach (PieceModel pm in gpm)
                    {
                        if (montant > 0)
                        {
                            MappingReglementPieces mapping = new MappingReglementPieces();
                            mapping.PieceID = pm.NumPiece;
                            mapping.ReglementID = reglement.ID;
                            double MontantReglee = MappingReglementPiecesBLL.GetSumMappingsByPiece(pm.NumPiece, true);
                            mapping.Montant = int.Parse(reglement.Sens) * pm.RAS - MontantReglee <= montant ? pm.RAS - MontantReglee : montant;
                            montant -= pm.RAS - MontantReglee <= montant ? pm.RAS - MontantReglee : montant;
                            MappingReglementPiecesBLL.Insert(mapping);
                            Piece_BLL.updateClosed(pm.NumPiece);
                        }
                    }
                    Reglements_BLL.updateClosed(reglement.ID);

                }
            }

            return Json(new TextValueModel("OK", reglement.ID.ToString()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExternalReglementDetails(int ExternalReglementID)
        {
            Reglements reglement = Reglements_BLL.GetById(ExternalReglementID);
            ReglementsModel reglementModel = GenericModelMapper.GetModel<ReglementsModel, Reglements>(reglement);
            return View(reglementModel);
        }

        public static IList<SelectListItem> GetBanquesList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var banques = Banque_BLL.GetAll();
            foreach (var f in banques)
            {
                res.Add(new SelectListItem()
                {
                    Text = f.NomBanque,
                    Value = f.ID.ToString()
                });
            }
            return res;
        }
        public static IList<SelectListItem> GetComptesList(string OwnerType, string Sens, int OwnerID)
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var Comptes = new List<CompteBancaire>();
            if ((OwnerType == "C" && Sens == "1") || (OwnerType == "F" && Sens == "-1"))
            {
                Comptes = CompteBancaire_BLL.GetAllComptesSTE();
            }
            else if (OwnerType == "C")
            {
                Comptes = CompteBancaire_BLL.GetAllComptesByClient(OwnerID);
            }
            else if (OwnerType == "F")
            {
                Comptes = CompteBancaire_BLL.GetAllComptesByFournisseur(OwnerID);
            }
            foreach (var f in Comptes)
            {
                res.Add(new SelectListItem()
                {
                    Text = f.Libelle,
                    Value = f.ID.ToString()
                });
            }
            return res;
        }
        public ActionResult GetPartialViewComptes(string OwnerType, string Sens, int OwnerID)
        {
            ViewBag.OwnerType = OwnerType;
            ViewBag.Sens = Sens;
            ViewBag.OwnerID = OwnerID;
            return PartialView("~/Views/Common/EditorTemplates/_PartialViewComptes.cshtml");
        }
        public double GetDefaultRASAmmount(string numPiece)
        {
            Piece piece = Piece_BLL.GetByNumPiece(numPiece);
            List<MappingReglementPiecesModel> mappings = MappingReglementPiecesBLL.GetMappingsByPiece(numPiece).Where(e => e.RAS).ToList();
            if(mappings.Count() > 0)
                return 0;
            if (piece != null)
                return piece.RAS;
            return 0;
        }
        public double GetDefaultAmmount(string numPiece)
        {
            Piece piece = Piece_BLL.GetByNumPiece(numPiece);
            if (piece != null)
            {
                double MontantFinal = piece.MontantFinal - piece.RAS;
                List<MappingReglementPiecesModel> mappings = MappingReglementPiecesBLL.GetMappingsByPiece(numPiece).Where(e => !e.RAS).ToList();
                if (mappings.Count() > 0)
                    return MontantFinal - mappings.Sum(e => e.Montant);
                else
                    return MontantFinal;
            }

            return 0;
        }

        public ActionResult ImportFromExisingReglement(int selectedReglement, string NumPiece)
        {
            try
            {
                Reglements r = Reglements_BLL.GetById(selectedReglement);
                PieceModel piece = Piece_BLL.GetPieceWithTypeByNumPiece(NumPiece);
                //Montatnt restant
                //bool IsExceed = MappingReglementPiecesBLL.CheckIfTotalAmmountIsExceed(NumPiece, r.Montant, selectedReglement, r.Sens);
                //if (IsExceed)
                //    return Json(new TextValueModel("KO", "Le montant des réglements dépasse le montant de la facture"), JsonRequestBehavior.AllowGet);
                double MontantPieceRestnt = piece.MontantFinal - Math.Abs(MappingReglementPiecesBLL.GetSumMappingsByPiece(NumPiece, false));
                double MontantReglementRestnt = r.Montant - Math.Abs(MappingReglementPiecesBLL.GetSumMappingsByReglement(selectedReglement));
                //if already exist a mapping : add new mapping line 
                MappingReglementPieces ExistingMapping = MappingReglementPiecesBLL.GetMappingsByReglementAndPiece(selectedReglement, NumPiece);
                if (ExistingMapping != null)
                {
                    ExistingMapping.Montant += MontantPieceRestnt <= MontantReglementRestnt ? MontantPieceRestnt * int.Parse(r.Sens) : MontantReglementRestnt * int.Parse(r.Sens);
                    MappingReglementPiecesBLL.Update(ExistingMapping);
                }
                else
                {
                    MappingReglementPieces mapping = new MappingReglementPieces();
                    mapping.PieceID = NumPiece;
                    mapping.ReglementID = selectedReglement;
                    mapping.Montant = MontantPieceRestnt <= MontantReglementRestnt ? MontantPieceRestnt * int.Parse(r.Sens) : MontantReglementRestnt * int.Parse(r.Sens);
                    MappingReglementPiecesBLL.Insert(mapping);
                }

                Reglements_BLL.updateClosed(r.ID);
                Piece_BLL.updateClosed(NumPiece);
                return Json(new TextValueModel("OK", "Opération réussite"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }


    }


}