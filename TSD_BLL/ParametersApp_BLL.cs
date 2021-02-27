using System.Linq;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class ParametersApp_BLL : GenericBLL<Banque>
    {
        public static string GetParameterValue(string ParameterName)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Parameters param = BD.Parameters.Where(e => e.Nom == ParameterName).FirstOrDefault();
                if (param != null)
                {
                    return param.Valeur;
                }
                return "";
            }
        }
    }
}
