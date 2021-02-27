using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;
namespace TSD_BLL
{
    public class CongesBLL : GenericBLL<Conges>
    {
        public static List<CongesListModel> GetFullVacationList()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var res = (from c in BD.Conges
                           join p in BD.Personnes on c.PersonID equals p.ID
                           join m in BD.MotifConge on c.MotifId equals m.ID
                           select new CongesListModel()
                           {
                               title = p.Prenom + " " + p.Nom,
                               starty = c.DateFrom.Year,
                               startM = c.DateFrom.Month,
                               startd = c.DateFrom.Day,
                               endy = c.DateTo.Year,
                               endM = c.DateTo.Month,
                               endd = c.DateTo.Day,
                               motif = m.Libelle
                           }).ToList();
                return res;
            }
        }
        public static int GetCongesInCurrentDay()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DateTime currentDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                List<Conges> conges = BD.Conges.Where(e => e.DateFrom <= currentDay && e.DateTo >= currentDay).ToList();
                return conges.Count();
            }
        }
        public static List<CongesModel> GetAllConges()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var res = (from c in BD.Conges
                           join p in BD.Personnes on c.PersonID equals p.ID
                           join m in BD.MotifConge on c.MotifId equals m.ID
                           select new CongesModel()
                           {
                               ID = c.ID,
                               Libelle = c.Libelle,
                               MotifId = m.ID,
                               PersonID = p.ID,
                               DateFrom = c.DateFrom,
                               DateTo = c.DateTo,
                               MotifName = m.Libelle,
                               Statut = c.Statut,
                               PersonName = p.Nom + " " + p.Prenom,

                           }).ToList();
                return res;
            }
        }

    }
}
