using BLL;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSD_BLL;
using TSD_DAL.TSD_EDMX;

namespace Gestion_Commerciale
{
    public static class SessionParameters
    {
        public static string CurrentUserName
        {
            get { return HttpContext.Current.Session["CurrentUserName"] as string; }
           
        }
        public static bool UserHasOneOfRoles(string Roles)
        {

                List<RolesModel> currentRoles = HttpContext.Current.Session["CurrentUserRoles"] as List<RolesModel>;
                if (currentRoles == null)
                    currentRoles = new List<RolesModel>();
            List<string> RolesString = Roles.Split(',').ToList();
            bool res = false;
            foreach(string RoleString in RolesString)
            {
                if(currentRoles.Select(e => e.Role).Contains(RoleString))
                {
                    res = true;
                }
            }
                return (currentRoles.Select(e => e.Role).Contains("admin") || res == true);

        }
        public static List<RolesModel> CurrentUserRoles
        {
            get { return HttpContext.Current.Session["CurrentUserRoles"] as List<RolesModel>; }
        }
        public static string CurrentUserID
        {
            get { return HttpContext.Current.Session["CurrentUserID"] as string; }

        }
        public static string CurrentSTEName
        {
            get { return HttpContext.Current.Session["CurrentSTEName"] as string; }

        }
        
        public static string CurrentFullName
        {
            get { return HttpContext.Current.Session["CurrentFullName"] as string; }

        }

        public static bool UserHasRole(string Role)
        {
            List<RolesModel> currentRoles = HttpContext.Current.Session["CurrentUserRoles"] as List<RolesModel>;
            if (currentRoles == null)
                currentRoles = new List<RolesModel>();
            return (currentRoles.Select(e => e.Role).Contains("admin") || currentRoles.Select(e => e.Role).Contains(Role));
             
                
        }

        public static bool UserHasExplicitlyRole(string Role)
        {
            List<RolesModel> currentRoles = HttpContext.Current.Session["CurrentUserRoles"] as List<RolesModel>;
            if (currentRoles == null)
                currentRoles = new List<RolesModel>();
            return (currentRoles.Select(e => e.Role).Contains(Role));
        }
        public static void SetSessionParameters(string UserName)
        {
            Users user = UsersBLL.GetResultByUserName(UserName);
            SetSessionRoles(user);
            SetSessionGenericData(user);
        }
        public static void SetSessionRoles(Users user)
        {
            List<RolesModel> roles = UserRolesMappingBLL.GetRolesByUser(user.ID);
            HttpContext.Current.Session["CurrentUserRoles"] = roles;
            //AuthorizeAttribute attr = HttpContext.Current.CurrentHandler;
            //attr.Roles
        }
        public static void SetSessionGenericData(Users user)
        {
            HttpContext.Current.Session["CurrentUserName"] = user.UserName;
            HttpContext.Current.Session["CurrentFullName"] = user.Prenom + " " + user.Nom;
            HttpContext.Current.Session["CurrentUserID"] = user.ID;
            Coordonnees c = Coordonnees_BLL.GetAll().FirstOrDefault();
            if(c != null)
            {
                HttpContext.Current.Session["CurrentSTEName"] = c.Nom;
            }
            else
            {
                HttpContext.Current.Session["CurrentSTEName"] = "";
            }
            
        }

    }
}