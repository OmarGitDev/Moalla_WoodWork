using System.Collections.Generic;
using System.Linq;
using TSD_BLL;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace BLL
{
    public class UsersBLL : GenericBLL<Users>
    {
        public static TextValueModel GetByUserName(string UserName, string Password)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                Users res = DB.Users.Where(e => e.UserName == UserName && e.Password == Password).FirstOrDefault();
                if (res == null)
                {
                    return (new TextValueModel("", "", bool1: false, bool2: false) { });
                }
                else
                {
                    return (new TextValueModel("", "", bool1: res != null && res.ID != 0, bool2: res.IsActive) { });
                }

            }
        }
        public static Users GetResultByUserName(string UserName)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                Users res = DB.Users.Where(e => e.UserName == UserName).FirstOrDefault();
                return res;
            }
        }

        public static List<string> GetAdministratorUsers()
        {
            IEnumerable<int> AdministratorsRole = UserRolesMappingBLL.GetAll().Where(elt => elt.Roles.Role == "admin").ToList().Select(elt => elt.UserID);
            List<string> AdministratorUsers = UsersBLL.GetAll().Where(elt => AdministratorsRole.Contains(elt.ID))
                .ToList().Select(elt => elt.UserName).ToList();
            return AdministratorUsers;
        }

    }
}
