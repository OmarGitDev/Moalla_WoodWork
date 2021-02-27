using System.Linq;
using TSD_DAL.TSD_EDMX;
namespace TSD_BLL
{
    public class PersonnesBLL : GenericBLL<Personnes>
    {
        public static string GetFullNameByID(int ID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                string res = BD.Personnes.Where(e => e.ID == ID).Select(e => e.Prenom + " " + e.Nom).FirstOrDefault();
                return res;
            }
        }
    }
}
