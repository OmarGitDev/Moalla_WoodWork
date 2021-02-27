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
    public class RHController : Controller
    {
        // GET: RH
        public ActionResult Index()
        {
            return View();
        }
        
public ActionResult CongesCalendar()
        {
            return View();
        }
       public ActionResult GetRHCalendarData()
        {
            List<CongesListModel> listCongesModel  = CongesBLL.GetFullVacationList();            
            return Json(listCongesModel, JsonRequestBehavior.AllowGet);

        }
        #region Personnes
        public static IList<SelectListItem> GetPersonsList()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem()
            {
                Text = "",
                Value = "0"
            });
            var persons = PersonnesBLL.GetAll();
            foreach (Personnes person in persons)
            {
                res.Add(new SelectListItem()
                {
                    Text = person.Nom+" "+ person.Prenom,
                    Value = person.ID.ToString()
                });
            }
            return res;
        }

        public ActionResult Person()
        {
            return View();
        }
        public JsonResult GetAllPersonnes()
        {
            List<Personnes> Personnes = PersonnesBLL.GetAll();
            List<PersonnesModel> persons = GenericModelMapper.GetModelList<PersonnesModel, Personnes>(Personnes);
            return Json(persons, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdatePerson(PersonnesModel PersonToAdd)
        {
            try
            {
                Personnes p = GenericModelMapper.GetModel<Personnes, PersonnesModel>(PersonToAdd);
                if (p.ID == 0)
                    PersonnesBLL.Insert(p);
                else
                    PersonnesBLL.Update(p);
                return Json(new TextValueModel("OK", "Person ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeletePerson(int ID)
        {
            //Person Person = Person_BLL.GetById(ID);
            PersonnesBLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenPersonnesEditor(int ID)
        {
            PersonnesModel Person = new PersonnesModel();
            if (ID != 0)
                Person = GenericModelMapper.GetModel<PersonnesModel, Personnes>(PersonnesBLL.GetById(ID));
            return PartialView("~/Views/RH/EditorTemplates/_PersonEditor.cshtml", Person);
        }
        #endregion
        #region Congés
        public ActionResult Conges()
        {
            return View();
        }
        public JsonResult GetAllConges()
        {

            List<CongesModel> Congess = CongesBLL.GetAllConges(); ;
            return Json(Congess, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateConges(CongesModel CongesToAdd)
        {
            try
            {
                Conges p = GenericModelMapper.GetModel<Conges, CongesModel>(CongesToAdd);
                p.DateTo = new DateTime(p.DateTo.Year, p.DateTo.Month, p.DateTo.Day, 23, 59, 59);
                if (p.ID == 0)
                    CongesBLL.Insert(p);
                else
                    CongesBLL.Update(p);
                return Json(new TextValueModel("OK", "Conges ajouté avec succés"), JsonRequestBehavior.AllowGet);
            }
            catch (DbException e)
            {
                return Json(new TextValueModel("KO", e.Message), JsonRequestBehavior.AllowGet); ;
            }
        }
        public ActionResult DeleteConges(int ID)
        {
            //Conges Conges = Conges_BLL.GetById(ID);
            CongesBLL.Delete(ID);
            return Content("");
        }
        public ActionResult OpenCongesEditor(int ID)
        {
            CongesModel Conges = new CongesModel();
            if (ID != 0)
                Conges = GenericModelMapper.GetModel<CongesModel, Conges>(CongesBLL.GetById(ID));
            return PartialView("~/Views/RH/EditorTemplates/_CongesEditor.cshtml", Conges);
        }
        #endregion
    }
}