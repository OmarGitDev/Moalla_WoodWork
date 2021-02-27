using System.Linq;
using TSD_DAL.TSD_EDMX;
namespace TSD_BLL
{
    public class MotifCongeBLL : GenericBLL<MotifConge>
    {
        public static string GetMotifLibelleByID(int ID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                string res = BD.MotifConge.Where(e => e.ID == ID).Select(e => e.Libelle).FirstOrDefault();
                return res;
            }
        }
    }
}
