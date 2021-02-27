using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Models;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{

    public class NotificationsBLL : GenericBLL<Notifications>
    {
        public static DateTime? LastNotificationRefresh;
        public static void UpdateLastNotifByProductID(string ProductID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Notifications notif = BD.Notifications.Where(e => e.Value == ProductID && e.Seen == false && e.Type == "Produit").FirstOrDefault();
                if (notif != null)
                {
                    notif.Seen = true;
                    BD.SaveChanges();
                }
            }
        }
        public static void RefreshNotifications()
        {

            DateTime CurrentDay = DateTime.Now.Date;
            if (CurrentDay == LastNotificationRefresh)
            {
                return;
            }
            else
            {
                using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
                {
                    DateTime YesterDay = CurrentDay.AddDays(-1);
                    List<Notifications> Notifs = BD.Notifications.Where(e => e.Date <= YesterDay && e.Seen == false && (e.Type == "ChequePos" || e.Type == "ChequeNeg")).ToList();
                    if (Notifs.Count() > 0)
                    {
                        Notifs.ForEach(e => e.Seen = true);
                        BD.SaveChanges();

                    }

                    DateTime AfterFourDays = CurrentDay.AddDays(3);
                    List<Reglements> reg = BD.Reglements.Where(e => e.DateEcheance != null && e.DateEcheance.Value <= AfterFourDays && e.DateEcheance.Value >= CurrentDay).ToList();
                    List<Reglements> regPos = reg.Where(e => e.Sens == "1").ToList();
                    List<Reglements> regNeg = reg.Where(e => e.Sens == "-1").ToList();
                    if (regPos.Count() > 0)
                    {

                        foreach (var rp in regPos)
                        {

                            Notifications Exnotif = BD.Notifications.Where(e => e.Value == rp.ID.ToString() && e.Type == "ChequePos").FirstOrDefault();
                            if (Exnotif == null)
                            {


                                Notifications notif = new Notifications();
                                notif.Type = "ChequePos";
                                notif.Seen = false;
                                //notif.Count = regPos.Count();
                                notif.Value = rp.ID.ToString();
                                notif.title = "Chèque à reçevoir";
                                notif.Date = DateTime.Now;
                                notif.description = "Vous avez  un chèque à reçevoir dont la date d'échéance est proche ( " + rp.Reference + " )";
                                Insert(notif);
                            }
                        }

                    }
                    if (regNeg.Count() > 0)
                    {

                        foreach (var rp in regNeg)
                        {

                            Notifications Exnotif = BD.Notifications.Where(e => e.Value == rp.ID.ToString() && e.Type == "ChequeNeg").FirstOrDefault();
                            if (Exnotif == null)
                            {
                                Notifications notif = new Notifications();
                                notif.Type = "ChequeNeg";
                                notif.Seen = false;
                                //notif.Count = regPos.Count();
                                notif.Value = rp.ID.ToString();
                                notif.title = "Chèque à payer";
                                notif.Date = DateTime.Now;
                                notif.description = "Vous avez  un chèque à payer dont la date d'échéance est proche ( " + rp.Reference + " )";
                                Insert(notif);
                            }
                        }
                    }
                    LastNotificationRefresh = CurrentDay;
                }
            }
        }
        public static List<NotificationsModel> getAllNotificationsByDateFilter(DateTime? dateFrom)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<Notifications> result = BD.Notifications.Where(e => e.Date > dateFrom || dateFrom == null).ToList();
                return GenericModelMapper.GetModelList<NotificationsModel, Notifications>(result);
            }
        }
        public static List<NotificationsModel> GetNotSeenNotifications()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<NotificationsModel> result = (from n in BD.Notifications
                                                   join tn in BD.NotificationsType on n.Type equals tn.TypeName
                                                   where n.Seen == false
                                                   select new NotificationsModel()
                                                   {
                                                       ID = n.ID,
                                                       title = n.title,
                                                       description = n.description,
                                                       Type = n.Type,
                                                       Date = n.Date,
                                                       Seen = n.Seen,
                                                       Count = n.Count,
                                                       Value = n.Value,
                                                       Url = tn.DefaultURL,
                                                   }).ToList().OrderByDescending(e => e.Date).ToList();
                return (result);
            }
        }
    }
}
