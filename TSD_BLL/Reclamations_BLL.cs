using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class Reclamations_BLL : GenericBLL<Reclamations>
    {
        public static List<ReclamationsModel> GetReclamationsByClient(DateTime? DateFromFilter, DateTime? DateToFilter, int ClientFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {

                List<ReclamationsModel> result = (from r in BD.Reclamations
                                                  join c in BD.Client on r.ClientID equals c.ID
                                                  where (DateFromFilter == null || DateFromFilter <= r.Date)
                                                  && (DateToFilter == null || DateToFilter >= r.Date)
                                                  && (ClientFilter == 0 || ClientFilter == c.ID)
                                                  select new ReclamationsModel()
                                                  {
                                                      ID = r.ID,
                                                      Date = r.Date,
                                                      Client = c.OwnerName,
                                                      Reclamation = r.Reclamation

                                                  }).ToList().OrderByDescending(e => e.Date).ToList();
                return (result);
            }
        }
        public static List<ReclamationsModel> GetReclamationsByDate(DateTime? DateFromFilter, DateTime? DateToFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {

                List<ReclamationsModel> result = (from r in BD.Reclamations
                                                  join c in BD.Client on r.ClientID equals c.ID
                                                  where (DateFromFilter == null || DateFromFilter <= r.Date)
                                                  && (DateToFilter == null || DateToFilter >= r.Date)

                                                  select new ReclamationsModel()
                                                  {
                                                      ID = r.ID,
                                                      Date = r.Date,
                                                      Client = c.OwnerName,
                                                      Reclamation = r.Reclamation

                                                  }).ToList().OrderByDescending(e => e.Date).ToList();
                return (result);
            }
        }
    }
}
