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
                                                         join product in DB.Services on material.ProductID equals product.ID
                                                         where material.ReglementID == reglementId
                                                         select new MaterialReglementDetailsModel()
                                                         {
                                                             ID = material.ID,
                                                             ProductName = product.Libelle,
                                                             ReglementID = material.ReglementID,
                                                             Amount = material.Amount

                                                         }).ToList();
              return result;


            }
        }
    }
}
