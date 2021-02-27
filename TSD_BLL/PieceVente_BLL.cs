using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;
namespace TSD_BLL
{
    public class PieceVente_BLL : GenericBLL<PieceVente>
    {
        public static int GetClientByNumPiece(string ID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = BD.PieceVente.Where(e => e.NumPieceVente == ID).FirstOrDefault();
                int codeClient = result == null ? 0 : result.CodeClient.Value;
                return codeClient;
            }

        }
        public static double GetTotalAmountBLInCurrentDay()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DateTime currentDay = DateTime.Now.Date;
                DateTime Tomorrow = DateTime.Now.Date.AddDays(1);
                var result = (from p in BD.Piece
                              join pv in BD.PieceAchat on p.NumPiece equals pv.NumPieceAchat

                              where (p.DateCreation >= currentDay && p.DateCreation < Tomorrow) && p.TypePiece == "BLIV"
                              select p.MontantFinal
                    ).ToList();
                double res = result.Sum();
                return res;
            }

        }
        public static double GetTotalAmountBCInCurrentDay()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DateTime currentDay = DateTime.Now.Date;
                DateTime Tomorrow = DateTime.Now.Date.AddDays(1);

                var result = (from p in BD.Piece
                              join pv in BD.PieceAchat on p.NumPiece equals pv.NumPieceAchat

                              where (p.DateCreation >= currentDay && p.DateCreation < Tomorrow) && p.TypePiece == "BCOM"
                              select p.MontantFinal
                    ).ToList();
                double res = result.Sum();
                return res;
            }

        }

        public static List<GenericPieceModel> GetAllPiecesVente()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece
                              join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
                              join c in BD.Client on pv.CodeClient equals c.ID
                              select new GenericPieceModel()
                              {
                                  CodeClient = pv.CodeClient,
                                  NomClient = c.OwnerName,
                                  ID = p.ID,
                                  NumPiece = p.NumPiece,
                                  TypePiece = p.TypePiece,
                                  Libelle = p.Libelle,
                                  DateCreation = p.DateCreation,
                                  Statut = p.Statut,
                                  CreatedBy = p.CreatedBy,
                                  EditedBy = p.EditedBy,
                                  LastEditTime = p.LastEditTime,
                                  MontantTotal = p.MontantTotal,
                                  MontantFinal = p.MontantFinal
                              }
                    ).ToList();
                return result;
            }

        }
        public static double GetTotalAmountPiecesVenteByClient(int client)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var res = (from p in BD.Piece
                           join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
                           join c in BD.Client on pv.CodeClient equals c.ID
                           join tp in BD.TypesPieces on p.TypePiece equals tp.TypeName
                           where c.ID == client && p.Statut == "VLD"
                           && (p.TypePiece == "CFAC" || p.TypePiece == "CNOC" || p.Comptabilise == true)
                           select new TextValueModel()
                           {
                               Val1 = p.MontantFinal,
                               Text = tp.Sens
                           }).ToList();

                double Sum = res.Sum(e => e.Val1 * int.Parse(e.Text)).Value;
                return Sum;
            }

        }
        //public static List<GenericPieceModel> GetMonthlyPiecesVenteByClient(int client,DateTime Date)
        //{
        //    using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
        //    {
        //        var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
        //        var res = (from p in BD.Piece
        //                   join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
        //                   join c in BD.Client on pv.CodeClient equals c.ID 
        //                   join mr in BD.MappingReglementPieces on p.NumPiece equals mr.PieceID into mappreg
        //                   from ed in mappreg.DefaultIfEmpty()
        //                   join mpp in BD.Reglements on ed.ReglementID equals mpp.ID into reg
        //                   from ed1 in reg.DefaultIfEmpty()
        //                   where c.ID == client && p.Statut == "VLD"
        //                   && (p.TypePiece == "CFAC" || p.TypePiece == "CNOC" || p.Comptabilise == true)
        //                   && (p.DateCreation > firstDayOfMonth && p.DateCreation <= Date)
        //                   select new GenericPieceModel()
        //                   {
        //                       CodeClient = pv.CodeClient,
        //                       NomClient = c.OwnerName,
        //                       ID = p.ID,
        //                       NumPiece = p.NumPiece,
        //                       TypePiece = p.TypePiece,
        //                       Libelle = p.Libelle,
        //                       DateCreation = p.DateCreation,
        //                       Statut = p.Statut,
        //                       CreatedBy = p.CreatedBy,
        //                       EditedBy = p.EditedBy,
        //                       LastEditTime = p.LastEditTime,
        //                       MontantTotal = p.MontantTotal,
        //                       MontantFinal = p.MontantFinal
        //                   }).ToList();
        //        return res;
        //    }

        //}
        public static double GetTotalAmountPiecesVenteInCurrentDay()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DateTime currentDay = DateTime.Now.Date;
                DateTime Tomorrow = DateTime.Now.Date.AddDays(1);
                var result = (from p in BD.Piece
                              join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
                              join tp in BD.TypesPieces on p.TypePiece equals tp.TypeName
                              join c in BD.Client on pv.CodeClient equals c.ID
                              where (p.DateCreation >= currentDay && p.DateCreation < Tomorrow) && (p.TypePiece == "CFAC" || p.TypePiece == "CNOC")
                              select new TextValueModel()
                              {
                                  Val1 = p.MontantFinal,
                                  Text = tp.Sens
                              }).ToList();

                double res = result.Sum(e => int.Parse(e.Text) * e.Val1.Value);
                return res;
            }

        }

        public static GenericPieceModel GetPieceVenteByNumPiece(string ID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece
                              join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
                              join c in BD.Client on pv.CodeClient equals c.ID
                              join ty in BD.TypesPieces on p.TypePiece equals ty.TypeName
                              where p.NumPiece == ID
                              select new GenericPieceModel()
                              {
                                  CodeClient = pv.CodeClient,
                                  NomClient = c.OwnerName,
                                  IDVente = pv.ID,
                                  ID = p.ID,
                                  NumPiece = p.NumPiece,
                                  TypePiece = p.TypePiece,
                                  Libelle = p.Libelle,
                                  DateCreation = p.DateCreation,
                                  Statut = p.Statut,
                                  CreatedBy = p.CreatedBy,
                                  EditedBy = p.EditedBy,
                                  LastEditTime = p.LastEditTime,
                                  MontantTotal = p.MontantTotal,
                                  MontantFinal = p.MontantFinal,
                                  Adresse = c.Adresse,
                                  MatriculeFiscal = c.MatriculeFiscal,
                                  LibelleTypePiece = ty.Libelle,
                                  Comptabilise = p.Comptabilise,
                                  RAS = p.RAS,
                                  Reference = p.Reference
                              }
                    ).FirstOrDefault();
                return result;
            }

        }
        public static void SaveCodeClientChanges(string NumPiece, int? CodeClient)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                PieceVente piece = BD.PieceVente.Where(e => e.NumPieceVente == NumPiece).FirstOrDefault();
                if (piece != null && piece.CodeClient != CodeClient)
                {
                    piece.CodeClient = CodeClient;
                    BD.SaveChanges();
                }
            }
        }

    }
}
