using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSD_BLL;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Data.Common;
using Gestion_Commerciale.Infrastructure;

namespace Gestion_Commerciale.Controllers
{
    [CustomAuthorize]
    public class AchatController : Controller
    {
        TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities();
        // GET: Achat
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddOrUpdateFournisseur(FournisseurModel fournisseurToAdd)
        {
            try
            {
                Fournisseur f = GenericModelMapper.GetModel<Fournisseur, FournisseurModel>(fournisseurToAdd);
                if(f.ID == 0)
                    Fournisseur_BLL.Insert(f);
                else
                    Fournisseur_BLL.Update(f);
                return Json(new TextValueModel("OK","Fournisseur ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO",e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public JsonResult GetAllFournisseurs(string TypeFournisseur = "")
        {
            List<Fournisseur> fournisseurs = Fournisseur_BLL.GetListFournisseursByType(TypeFournisseur);
            List<FournisseurModel>  fournisseursModelList = GenericModelMapper.GetModelList<FournisseurModel,Fournisseur>(fournisseurs);
            return Json(fournisseursModelList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Suivie ()
        {
            return View();
        }
        public ActionResult Fournisseur ()
        {
            return View();
        }
        //public ActionResult FactureAchat()
        //{
        //    return View("Facture",CommonController, "Achat");
        //}
        public ActionResult DeleteFournisseur(int ID)
        {
            //Fournisseur fournisseur = Fournisseur_BLL.GetById(ID);
            Fournisseur_BLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenFournisseurEditor(int ID)
        {
            FournisseurModel fournisseur = new FournisseurModel();
            if (ID != 0)
                fournisseur = GenericModelMapper.GetModel<FournisseurModel, Fournisseur>(Fournisseur_BLL.GetById(ID));
            else
            {
                fournisseur.IsActive = true;
                fournisseur.FType = "Marchandise";
            }
            return PartialView("~/Views/Achat/EditorTemplates/_FournisseurEditor.cshtml", fournisseur);
        }
        public static IList<SelectListItem> GetFournisseursList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var fournisseur = Fournisseur_BLL.GetAll();
            foreach (var f in fournisseur)
            {
                res.Add(new SelectListItem()
                {
                    Text = f.OwnerName,
                    Value = f.ID.ToString()
                });
            }
            return res;
        }

        //Print PDF
        //public ActionResult ImprimeFacture()
        //{

        //    List<Piece> _Piece = BD.Piece.ToList();

        //    dynamic dt = from Element in _Piece
        //                 select new
        //                 {
        //                     ID = Element.ID,
        //                     NumPiece = Element.NumPiece,
        //                     Libelle = Element.Libelle,
        //                     TypePiece = Element.TypePiece

        //                 };//adresse_B

        //    ReportDocument rptH = new ReportDocument();
        //    string FileName = Server.MapPath("/Reports/Piece.rpt");
        //    rptH.Load(FileName);
        //    rptH.SummaryInfo.ReportTitle = "Piece";
        //    rptH.SetDataSource(dt);
        //    Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    return File(stream, "application/pdf");
        //}

        //Export Exel

        public ContentResult Exporter()
        {

            List<Piece> ListePieceToExport = BD.Piece.ToList();
            var liste = from PO in ListePieceToExport
                        select new
                        {
                            NumeroPiece = PO.NumPiece,
                            TypePiece = PO.TypePiece
                        };

            GridView gv = new GridView();
            gv.DataSource = liste;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ListePieces.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return Content("");
        }
    } 
}