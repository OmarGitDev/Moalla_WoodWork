using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class PricingDetails_BLL : GenericBLL<PricingDetails>
    {
        public static List<PricingDetailsModel> GetAllDetailsByPricing(int ID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<PricingDetails> details = BD.PricingDetails.Where(p => p.PricingID == ID).ToList();
                return GenericModelMapper.GetModelList<PricingDetailsModel, PricingDetails>(details);
            }
        }


    }
}
