using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class ServicesBLL : GenericBLL<Services>
    {
        public static List<ServicesModel> GetAllServicesByFilter(string libelle = "", string code = "")
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                var ServicesList = DB.Services.Where(e => (e.Libelle.ToLower().Contains(libelle.ToLower()) || String.IsNullOrEmpty(libelle)) && (e.Reference.ToLower().Contains(code.ToLower()) || String.IsNullOrEmpty(code))).ToList();
                return GenericModelMapper.GetModelList<ServicesModel, Services>(ServicesList);
            }
        }
    }
}
