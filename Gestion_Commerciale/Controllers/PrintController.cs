﻿
using Gestion_Commerciale.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSD_BLL;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace Gestion_Commerciale.Controllers
{
    [CustomAuthorize]
    public class PrintController : Controller
    {
        // GET: Print
        public ActionResult Index()
        {
            return View();
        }
        //  Data Source = BEST - TECHNOLOGY\\SGC00SQL; Initial Catalog = TSD_Gestion_Commerciale
        
        public JsonResult ImprimerRapportFactureClient(string CodeFacture)
        {
            //PrintAndArchiveOptions printOptions = new PrintAndArchiveOptions(true, false, false, true,
            //                                       SessionParameters.CurrentDataBaseSuffix,
            //                                       SessionParameters.SqlServerName,
            //                                       SessionParameters.CurrentUserProfile.UserName,
            //                                       SessionParameters.TemporaryPhysicalDirectoryPath,
            //                                       SessionParameters.TemporaryVirtualDirectoryName,
            //                                       Request.Url.Authority,
            //                                       SessionParameters.CurrentSerieDataBaseSuffix,
            //                                       String.Empty, SessionParameters.DataBasePrefix);
            ReportGenerator rg = new ReportGenerator();

            string Report = rg.ImprimerRapportFacture(CodeFacture, Request.Url.Authority);

                 return Json(new TextValueModel("OK", Report), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ImprimerReleveCompte(DateTime DateFromFilter, DateTime DateToFilter,int ClientFilter)
        {
            //PrintAndArchiveOptions printOptions = new PrintAndArchiveOptions(true, false, false, true,
            //                                       SessionParameters.CurrentDataBaseSuffix,
            //                                       SessionParameters.SqlServerName,
            //                                       SessionParameters.CurrentUserProfile.UserName,
            //                                       SessionParameters.TemporaryPhysicalDirectoryPath,
            //                                       SessionParameters.TemporaryVirtualDirectoryName,
            //                                       Request.Url.Authority,
            //                                       SessionParameters.CurrentSerieDataBaseSuffix,
            //                                       String.Empty, SessionParameters.DataBasePrefix);
            ReportGenerator rg = new ReportGenerator();

            string Report = rg.ImprimerReleveCompte(DateFromFilter, DateToFilter, ClientFilter, Request.Url.Authority);

            return Json(new TextValueModel("OK", Report), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ImprimerRapportPricing(string CodePricing)
        {
            //PrintAndArchiveOptions printOptions = new PrintAndArchiveOptions(true, false, false, true,
            //                                       SessionParameters.CurrentDataBaseSuffix,
            //                                       SessionParameters.SqlServerName,
            //                                       SessionParameters.CurrentUserProfile.UserName,
            //                                       SessionParameters.TemporaryPhysicalDirectoryPath,
            //                                       SessionParameters.TemporaryVirtualDirectoryName,
            //                                       Request.Url.Authority,
            //                                       SessionParameters.CurrentSerieDataBaseSuffix,
            //                                       String.Empty, SessionParameters.DataBasePrefix);
            ReportGenerator rg = new ReportGenerator();

            string Report = rg.ImprimerRapportPricing(CodePricing, Request.Url.Authority);

            return Json(new TextValueModel("OK", Report), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ImprimerRapportReglement(string ReglementID)
        {
            //PrintAndArchiveOptions printOptions = new PrintAndArchiveOptions(true, false, false, true,
            //                                       SessionParameters.CurrentDataBaseSuffix,
            //                                       SessionParameters.SqlServerName,
            //                                       SessionParameters.CurrentUserProfile.UserName,
            //                                       SessionParameters.TemporaryPhysicalDirectoryPath,
            //                                       SessionParameters.TemporaryVirtualDirectoryName,
            //                                       Request.Url.Authority,
            //                                       SessionParameters.CurrentSerieDataBaseSuffix,
            //                                       String.Empty, SessionParameters.DataBasePrefix);
            ReportGenerator rg = new ReportGenerator();

            string Report = rg.ImprimerRapportReglement(ReglementID, Request.Url.Authority);

            return Json(new TextValueModel("OK", Report), JsonRequestBehavior.AllowGet);
        }
        
        
        //public ActionResult PrintFactureClientDetails(string NumPiece)
        //{

        //    string Doit = string.Empty;
        //    string Adresse = string.Empty;
        //    string MatriculeFiscale = string.Empty;
        //    string NomClient = string.Empty;
        //    double? pourcentageTaxe = 0;
        //    double TotalHT = 0;
        //    double TotalTVA = 0;
        //   double MontantRemise = 0;

        //    Coordonnees coordonnees = Coordonnees_BLL.GetAll().FirstOrDefault();
        //    GenericPieceModel GnericPiece = PieceVente_BLL.GetPieceVenteByNumPiece(NumPiece);
        //    int CodeClient = GnericPiece.CodeClient.Value;
        //    Client clientModel = Client_BLL.GetById(CodeClient);       
        //    List<DetailsPieceModel> _Detail = DetailsPiece_BLL.GetDetailsByPiece(NumPiece);
        //    int DetilsCount = _Detail.Count();
        //   // if(DetilsCount)
        //    foreach (DetailsPieceModel detail in _Detail)
        //    {
        //        TotalHT += double.Parse(detail.MontantHorsTaxe.ToString());
        //        MontantRemise+= (detail.Remise ==null? 0: detail.Remise.Value*(detail.MontantHorsTaxe.Value+ detail.MontantTaxe.Value) /100);
        //        TotalTVA += detail.MontantTaxe == null ? 0 : detail.MontantTaxe.Value;
        //        pourcentageTaxe = detail.CodeTaxe==null|| detail.CodeTaxe == 0 ? 0: Taxes_BLL.GetById(detail.CodeTaxe.Value).Pourcentage;
        //    }


        //    dynamic dt = from Element in _Detail
        //                 select new
        //                 {

        //                     AdresseTSD = coordonnees.Adresse != null ? coordonnees.Adresse : string.Empty,
        //                     DescriptionTSD = coordonnees.Description != null ? coordonnees.Description : string.Empty,
        //                     EmailTSD = coordonnees.Email != null ? coordonnees.Email : string.Empty,
        //                     FaxTSD = coordonnees.Fax != null ? coordonnees.Fax : string.Empty,
        //                     MatriculeFiscaleTSD = coordonnees.MatriculeFiscale != null ? coordonnees.MatriculeFiscale : string.Empty,
        //                     NomTSD = coordonnees.Nom != null ? coordonnees.Nom : string.Empty,
        //                     TelTSD = coordonnees.TelFixe != null ? coordonnees.Tel != null ? coordonnees.Tel : string.Empty : coordonnees.Tel + " - " + coordonnees.TelFixe,
        //                     LibelleTypePiece = GnericPiece.LibelleTypePiece != null ? GnericPiece.LibelleTypePiece.ToString().Contains("Facture") ? "Facture" : GnericPiece.LibelleTypePiece.ToString() : string.Empty,
        //                     NumPiece = NumPiece != null ? NumPiece.ToString() : string.Empty,
        //                     Tel1 = clientModel.Tel2 != null ? clientModel.Tel1 != null ? clientModel.Tel1 : String.Empty : clientModel.Tel1 + " - " + clientModel.Tel2,
        //                     OwnerName = GnericPiece.NomClient != null ? GnericPiece.CodeClient.ToString() + " - " + GnericPiece.NomClient.ToString() : string.Empty,

        //                     Adresse = GnericPiece.Adresse != null ? GnericPiece.Adresse.ToString() : string.Empty,
        //                     MatriculeFiscal = GnericPiece.MatriculeFiscal != null ? GnericPiece.MatriculeFiscal.ToString() : string.Empty,
        //                     CodeDetailPiece = Element.CodeDetailPiece != null ? Element.CodeDetailPiece.ToString() : string.Empty,
        //                     libelleDetail = Element.Libelle != null ? Element.Libelle.ToString() : string.Empty,
        //                     Quantite = Element.Quantite != null ? Element.Quantite.ToString() : string.Empty,
        //                     Remise = Element.Remise != null ? Element.Remise.ToString() : string.Empty,
        //                     Pourcentage = pourcentageTaxe != null ? pourcentageTaxe.ToString() : string.Empty,
        //                     MontantUnitaire = Element.MontantUnitaire != null ? Element.MontantUnitaire.Value.ToString("N3") : string.Empty,
        //                     MontantHorsTaxe = Element.MontantHorsTaxe != null ? Element.MontantHorsTaxe.Value.ToString("N3") : string.Empty,
        //                     TotalBrut = TotalHT.ToString("N3"),
        //                     MontantRemise = MontantRemise.ToString("N3"),
        //                     MontantFinal = GnericPiece.MontantFinal != null? GnericPiece.MontantFinal.Value.ToString("N3") :String.Empty,
        //                     MontantTotal = GnericPiece.MontantTotal != null ? (GnericPiece.MontantTotal.Value - MontantRemise).ToString("N3") : String.Empty,
        //                     MontantTaxe = TotalTVA.ToString("N3"),







        //                 };//adresse_B
        //    //dynamic dt ;
        //    ReportDocument rptH = new ReportDocument();
        //    string FileName = Server.MapPath("/Reports/PieceVente.rpt");
        //    rptH.Load(FileName);
        //    rptH.SummaryInfo.ReportTitle = GnericPiece.LibelleTypePiece;
        //    rptH.SetDataSource(dt);

        //    //rptH.SetParameterValue("@NumPiece", NumPiece);
        //    //  rptH.SetParameterValue("@Category", GnericPiece.Categorie);

        //    Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    return File(stream, "application/pdf");
        //}
        //public ActionResult PrintPricingDetails(int CodePricing)
        // {

        //     string Doit = string.Empty;
        //     string Adresse = string.Empty;
        //     string MatriculeFiscale = string.Empty;
        //     string NomClient = string.Empty;

        //     double TotalHT = 0;
        //     double TotalTVA = 0;
        //     double MontantRemise = 0;
        //     double? pourcentageTaxe = 0;
        //     Coordonnees coordonnees = Coordonnees_BLL.GetAll().FirstOrDefault();
        //     Pricing pricing = PricingBLL.GetById(CodePricing);
        //     int CodeClient = pricing.ClientID.Value;
        //     Client clientModel = Client_BLL.GetById(CodeClient);
        //     List<PricingDetailsModel> _Detail = PricingDetails_BLL.GetAllDetailsByPricing(CodePricing);
        //     int DetilsCount = _Detail.Count();
        //     // if(DetilsCount)
        //     foreach (PricingDetailsModel detail in _Detail)
        //     {
        //         TotalHT += double.Parse(detail.MontantHorsTaxe.ToString());
        //         MontantRemise += (detail.Remise == null ? 0 : detail.Remise.Value * (detail.MontantHorsTaxe + detail.MontantTaxe) / 100);
        //         TotalTVA +=  detail.MontantTaxe;
        //         pourcentageTaxe = Taxes_BLL.GetById(detail.CodeTaxe).Pourcentage;
        //     }


        //     dynamic dt = from Element in _Detail
        //                  select new
        //                  {

        //                      AdresseTSD = coordonnees.Adresse != null ? coordonnees.Adresse : string.Empty,
        //                      DescriptionTSD = coordonnees.Description != null ? coordonnees.Description : string.Empty,
        //                      EmailTSD = coordonnees.Email != null ? coordonnees.Email : string.Empty,
        //                      FaxTSD = coordonnees.Fax != null ? coordonnees.Fax : string.Empty,
        //                      MatriculeFiscaleTSD = coordonnees.MatriculeFiscale != null ? coordonnees.MatriculeFiscale : string.Empty,
        //                      NomTSD = coordonnees.Nom != null ? coordonnees.Nom : string.Empty,
        //                      TelTSD = coordonnees.TelFixe != null ? coordonnees.Tel != null ? coordonnees.Tel : string.Empty : coordonnees.Tel + " - " + coordonnees.TelFixe,
        //                      LibelleTypePiece = pricing.Type != null ? pricing.Type=="Tarifs" ? "Tarif" : pricing.Type.ToString() : string.Empty,
        //                      CodePricing = pricing.CodePricing != null ? pricing.CodePricing.ToString() : string.Empty,
        //                      Tel1 = clientModel.Tel2 != null ? clientModel.Tel1 != null ? clientModel.Tel1 : String.Empty : clientModel.Tel1 + " - " + clientModel.Tel2,
        //                      OwnerName = clientModel.OwnerName != null ? clientModel.ID.ToString() + " - " + clientModel.OwnerName.ToString() : string.Empty,

        //                      Adresse = clientModel.Adresse != null ? clientModel.Adresse.ToString() : string.Empty,
        //                      MatriculeFiscal = clientModel.MatriculeFiscal != null ? clientModel.MatriculeFiscal.ToString() : string.Empty,
        //                      CodeDetail = Element.CodeDetail != null ? Element.CodeDetail.ToString() : string.Empty,
        //                      libelleDetail = Element.Libelle != null ? Element.Libelle.ToString() : string.Empty,
        //                      Quantite = Element.Quantite != null ? Element.Quantite.ToString() : string.Empty,
        //                      Remise = Element.Remise != null ? Element.Remise.ToString() : string.Empty,
        //                      Pourcentage = pourcentageTaxe != null ? pourcentageTaxe.ToString() : string.Empty,
        //                      MontantUnitaire = Element.MontantUnitaire != null ? Element.MontantUnitaire.Value.ToString("N3") : string.Empty,
        //                      MontantHorsTaxe =  Element.MontantHorsTaxe.ToString("N3") ,
        //                      TotalBrut = TotalHT.ToString("N3"),
        //                      MontantRemise = MontantRemise.ToString("N3"),
        //                      MontantFinal = pricing.MontantFinal.ToString("N3"),
        //                      MontantTotal = (pricing.MontantTotal- MontantRemise).ToString("N3"),
        //                      MontantTaxe = TotalTVA.ToString("N3"),







        //                  };//adresse_B
        //     //dynamic dt ;
        //     ReportDocument rptH = new ReportDocument();
        //     string FileName = Server.MapPath("/Reports/Pricing.rpt");

        //     rptH.Load(FileName);
        //     rptH.SummaryInfo.ReportTitle = pricing.Type;
        //     rptH.SetDataSource(dt);

        //     //rptH.SetParameterValue("@NumPiece", NumPiece);
        //     //  rptH.SetParameterValue("@Category", GnericPiece.Categorie);

        //     Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //     return File(stream, "application/pdf");
        // }
    }
}