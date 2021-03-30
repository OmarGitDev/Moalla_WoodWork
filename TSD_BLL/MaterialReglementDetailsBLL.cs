using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class MaterialReglementDetailsBLL : GenericBLL<MaterialReglementDetails>
    {
        public static List<MaterialReglementDetailsModel> GetMaterialDetailsByReglement(int reglementId)
        {
            using (TSD_Gestion_CommercialeEntities DB = new TSD_Gestion_CommercialeEntities())
            {
                List<MaterialReglementDetailsModel> result = (from material in DB.MaterialReglementDetails
                                                         
                                                         where material.ReglementID == reglementId
                                                         select new MaterialReglementDetailsModel()
                                                         {
                                                             ID = material.ID,
                                                             ReglementID = material.ReglementID,
                                                             Ammount = material.Ammount,
                                                             OwnerName = material.OwnerName,
                                                             VoucherNumber = material.VoucherNumber,

                                                         }).ToList();
              return result;


            }
        }
    }
}
