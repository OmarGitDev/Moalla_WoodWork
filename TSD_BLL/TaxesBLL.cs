using System.Linq;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class Taxes_BLL : GenericBLL<Taxes>
    {
        public static int getDefaultTaxeId()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Taxes t = BD.Taxes.Where(e => e.IsDefault).FirstOrDefault();
                return (t == null ? 0 : t.ID);

            }
        }
        public static double GetDefaultTaxePourcentage()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Taxes t = BD.Taxes.Where(e => e.IsDefault).FirstOrDefault();
                return (t == null ? 0 : t.Pourcentage.Value);

            }
        }
    }
}
