using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;
namespace TSD_BLL
{
    public class PieceAchat_BLL : GenericBLL<PieceAchat>
    {

        public static GenericPieceModel GetPieceAchatByNumPiece(string ID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece
                              join pa in BD.PieceAchat on p.NumPiece equals pa.NumPieceAchat
                              join f in BD.Fournisseur on pa.CodeFournisseur equals f.ID
                              join ty in BD.TypesPieces on p.TypePiece equals ty.TypeName
                              where p.NumPiece == ID
                              select new GenericPieceModel()
                              {
                                  Sens = ty.Sens,
                                  Categorie = ty.Categorie,
                                  CodeFournisseur = pa.CodeFournisseur,
                                  NomFournisseur = f.OwnerName,
                                  IDAchat = pa.ID,
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
                                  Adresse = f.Adresse,
                                  MatriculeFiscal = f.MatriculeFiscal,
                                  LibelleTypePiece = ty.Libelle,
                                  Comptabilise = p.Comptabilise,
                                  RAS = p.RAS,
                                  Reference = p.Reference
                              }
                    ).FirstOrDefault();
                return result;
            }

        }
        public static int GetFournisseurByNumPiece(string ID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = BD.PieceAchat.Where(e => e.NumPieceAchat == ID).FirstOrDefault();
                int codeFournisseur = result == null ? 0 : result.CodeFournisseur.Value;
                return codeFournisseur;
            }

        }
        public static List<Piece> GetPiecesByIDs(List<int> PiecesIDS)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<Piece> p = BD.Piece.Where(e => PiecesIDS.Contains(e.ID)).ToList();
                return (p);
            }
        }
        public static double GetTotalAmountPiecesAchatInCurrentDay()
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                DateTime currentDay = DateTime.Now.Date;
                DateTime Tomorrow = DateTime.Now.Date.AddDays(1);
                var result = (from p in BD.Piece
                              join pa in BD.PieceAchat on p.NumPiece equals pa.NumPieceAchat
                              join f in BD.Fournisseur on pa.CodeFournisseur equals f.ID
                              where (p.DateCreation >= currentDay && p.DateCreation < Tomorrow) && p.TypePiece == "FFAC"
                              select p.MontantFinal
                    ).ToList();
                double res = result.Sum();
                return res;
            }

        }

        public static void SaveCodeFournisseurChanges(string NumPiece, int? CodeFournisseur)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                PieceAchat piece = BD.PieceAchat.Where(e => e.NumPieceAchat == NumPiece).FirstOrDefault();
                if (piece != null && piece.CodeFournisseur != CodeFournisseur)
                {
                    piece.CodeFournisseur = CodeFournisseur;
                    BD.SaveChanges();
                }
            }
        }
        public static void DeleteGenericPiece(GenericPieceModel gmp)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<DetailsPiece> details = BD.DetailsPiece.Where(e => e.Piece == gmp.NumPiece).ToList();
                BD.DetailsPiece.RemoveRange(details);
                BD.SaveChanges();
                if (gmp.TypePiece == "FFAC")
                    BD.PieceAchat.Remove(BD.PieceAchat.Where(e => e.NumPieceAchat == gmp.NumPiece).FirstOrDefault());
                else
                    BD.PieceVente.Remove(BD.PieceVente.Where(e => e.NumPieceVente == gmp.NumPiece).FirstOrDefault());
                BD.SaveChanges();
                BD.Piece.Remove(BD.Piece.Where(e => e.NumPiece == gmp.NumPiece).FirstOrDefault());
            }
        }

    }
}
