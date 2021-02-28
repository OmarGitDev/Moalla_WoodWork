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
    public class StockController : Controller
    {
        // GET: Stock
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Services()
        {
            //NotificationsBLL.UpdateLastNotifByProductID(DefaultFilter.ToString());
            //ViewBag.DefaultFilter = DefaultFilter;
            return View();
        }
        public ActionResult Mouvements()
        {
            return View();
        }
        //     public JsonResult GetAllServices()
        //{
        //    List<Services> Services = ServicesBLL.GetAll();
        //    List<ServicesModel> ServicedModelList = GenericModelMapper.GetModelList<ServicesModel, Services>(Services);
        //    return Json(ServicedModelList, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetAllServices(string libelle="",string code="",bool? Vente = true, bool? Achat = true, bool? AchatAndVente = false)
        {
            List<ServicesModel> Services = ServicesBLL.GetAllServicesByFilter(libelle, code);
            if(AchatAndVente == false)
            {
                if(Vente == false)
                {
                    Services = Services.Where(e => e.Achat == true).ToList();
                }
                if (Achat == false)
                {
                    Services = Services.Where(e => e.Vente == true).ToList();
                }
            }
            else
            {
                Services = Services.Where(e => e.Vente == true && e.Achat == true).ToList();
            }
            return Json(Services, JsonRequestBehavior.AllowGet);
        }

        
        public static IList<SelectListItem> GetServicesList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var Service = ServicesBLL.GetAll();
            foreach (var f in Service)
            {
                res.Add(new SelectListItem()
                {
                    Text = f.Reference,
                    Value = f.ID.ToString()
                });
            }
            return res;
        }
        public static IList<SelectListItem> GetAchatServicesList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var Service = ServicesBLL.GetAchatServices();
            foreach (var f in Service)
            {
                res.Add(new SelectListItem()
                {
                    Text = f.Libelle,
                    Value = f.ID.ToString()
                });
            }
            return res;
        }

        public ActionResult AddOrUpdateService(ServicesModel ServiceToAdd)
        {
            try
            {

                Services p = GenericModelMapper.GetModel<Services, ServicesModel>(ServiceToAdd);
                if (p.ID == 0)
                    ServicesBLL.Insert(p);
                else
                {
                    ServicesBLL.Update(p);
                }
                return Json(new TextValueModel("OK", "Service ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteService(int ID)
        {
            //Service Service = Service_BLL.GetById(ID);
            ServicesBLL.Delete(ID);
            return Content("");
        }

        
        public ActionResult OpenServicesEditor(int ID)
        {
            ServicesModel Service = new ServicesModel();
            if (ID != 0)
                Service = GenericModelMapper.GetModel<ServicesModel, Services>(ServicesBLL.GetById(ID));
            return PartialView("~/Views/Stock/EditorTemplates/_ServiceEditor.cshtml", Service);
        }

        
    }
}