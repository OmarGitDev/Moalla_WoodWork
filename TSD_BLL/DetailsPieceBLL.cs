using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class DetailsPiece_BLL : GenericBLL<DetailsPiece>
    {
        public static List<DetailsPieceModel> GetDetailsByPiece(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<DetailsPiece> details = BD.DetailsPiece.Where(e => e.Piece == NumPiece).ToList();
                return (GenericModelMapper.GetModelList<DetailsPieceModel, DetailsPiece>(details));
            }
        }
        public static List<DetailsPieceModel> GetAllDetailsByPiece(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<DetailsPiece> details = BD.DetailsPiece.Where(e => e.Piece == NumPiece).ToList();
                return (GenericModelMapper.GetModelList<DetailsPieceModel, DetailsPiece>(details));
            }
        }
        public static DetailsPiece GetAutomaticLineByPiece(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DetailsPiece details = BD.DetailsPiece.Where(e => e.Piece == NumPiece).FirstOrDefault();
                return (details);
            }
        }

        public static int GetReservedCountByProduct(int ProductID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var res = (from dp in BD.DetailsPiece
                           join p in BD.Piece on dp.Piece equals p.NumPiece
                           where p.TypePiece == "CFAC" && p.Statut == "ECR" && dp.ProduitID == ProductID
                           select dp.Quantite).ToList().Sum();
                return res.Value;
            }
        }
    }
}
