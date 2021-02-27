using Gestion_Commerciale.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSD_BLL;
using TSD_DAL.TSD_EDMX;

namespace Gestion_Commerciale.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(SessionParameters.CurrentUserName))
                return RedirectToAction("LogOff", "Account");

            double totalBC = PieceVente_BLL.GetTotalAmountBCInCurrentDay();
            double totalBL = PieceVente_BLL.GetTotalAmountBLInCurrentDay();
            double totalFactures = PieceVente_BLL.GetTotalAmountPiecesVenteInCurrentDay();
            double totalFacturesFournissurs = PieceAchat_BLL.GetTotalAmountPiecesAchatInCurrentDay();
            double totalReglementsC = Reglements_BLL.GetTotalAmountReglementsVenteInCurrentDay();
            double totalReglementsF = Reglements_BLL.GetTotalAmountReglementsAchatInCurrentDay();
            int PersonInConge = CongesBLL.GetCongesInCurrentDay();
            int Passages = PassagesClientsBLL.GetPassageToDay();
            ViewBag.totalBC = totalBC;
            ViewBag.totalBL = totalBL;
            ViewBag.FactureClientAmount = totalFactures;
            ViewBag.totalFacturesFournissurs = totalFacturesFournissurs;
            ViewBag.totalReglementsAmountC = totalReglementsC;
            ViewBag.totalReglementsAmountF = totalReglementsF;
            ViewBag.totalFacturesFournissurs = totalFacturesFournissurs;
            ViewBag.Passages = Passages;
            return View();
        }
        public ActionResult UpdateCoordonnees(Coordonnees coordonnees)
        {
            try
            {
                Coordonnees_BLL.Update(coordonnees);
                return Content("");
            }
            catch(Exception e)
            {
                return Content("");
            }
        }
        public ActionResult Profil()
        {
            Coordonnees coordonnees = Coordonnees_BLL.GetAll().FirstOrDefault();
            return View(coordonnees);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}