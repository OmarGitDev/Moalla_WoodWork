using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class PassagesClientsBLL : GenericBLL<PassagesClients>
    {
        public static List<PassagesClientsModel> GetPassagesPerMonth(DateTime Month)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                DateTime LastMonth = Month.AddMonths(1);
                var res = (from ps in DB.PassagesClients
                           join cl in DB.Client on ps.ClientID equals cl.ID
                           where ps.DatePassage >= Month && ps.DatePassage < LastMonth
                           select new PassagesClientsModel()
                           {
                               ID = ps.ID,
                               DatePassage = ps.DatePassage,
                               ClientID = ps.ClientID,
                               Status = ps.Status,
                               ClientName = cl.OwnerName,
                               ClientAdresse = cl.Route,
                           }).ToList();
                return res;
            }
        }
        public static PassagesClients GetPassageByClientAndDate(int Client, DateTime DatePassage)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                return DB.PassagesClients.Where(e => e.ClientID == Client && e.DatePassage == DatePassage).FirstOrDefault();
            }
        }
        public static int GetPassageToDay()
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                return DB.PassagesClients.Where(e => e.DatePassage == DateTime.Today).Count();
            }
        }
    }
}
