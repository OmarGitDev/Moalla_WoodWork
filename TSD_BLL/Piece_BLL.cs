using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{

    public class Piece_BLL : GenericBLL<Piece>
    {
        public static GenericPieceModel GetGenericPiece(string NumPiece, string Type)
        {
            GenericPieceModel model = new GenericPieceModel();
            if (Type == "FFAC" || Type == "FNOC" || Type == "BLIV" || Type == "BCOM")
            {
                model = PieceAchat_BLL.GetPieceAchatByNumPiece(NumPiece);
            }
            else
            {
                model = PieceVente_BLL.GetPieceVenteByNumPiece(NumPiece);
            }
            return model;
        }

        public static double GetAllPiecesBetweenDates(DateTime DateFrom, DateTime DateTo)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var res = (from p in BD.Piece
                           join tp in BD.TypesPieces on p.TypePiece equals tp.TypeName
                           where p.Statut == "VLD" && (p.TypePiece == "CFAC" || p.TypePiece == "CNOC" || p.Comptabilise == true)
                           && p.DateCreation >= DateFrom && p.DateCreation < DateTo
                           select new TextValueModel()
                           {
                               Val1 = p.MontantFinal,
                               Text = tp.Sens
                           }).ToList();

                double Sum = res.Sum(e => e.Val1 * int.Parse(e.Text)).Value;
                return Sum;
            }
        }

        public static List<GenericPieceModel> GetAllPiecesFournisseurs(string Type, string CodeFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter, int? FournisseurFilter, string StatusFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece
                              join pa in BD.PieceAchat on p.NumPiece equals pa.NumPieceAchat
                              join f in BD.Fournisseur on pa.CodeFournisseur equals f.ID
                              where p.TypePiece == Type
                              && (String.IsNullOrEmpty(CodeFilter) || p.NumPiece.Contains(CodeFilter))
                              && (DateFromFilter == null || p.DateCreation >= DateFromFilter)
                              && (DateToFilter == null || p.DateCreation <= DateToFilter)
                              && (MontantMinFilter == null || p.MontantFinal >= MontantMinFilter)
                              && (MontantMaxFilter == null || p.MontantFinal <= MontantMaxFilter)
                              && (FournisseurFilter == 0 || f.ID == FournisseurFilter)
                              && (StatusFilter == "0" || p.Statut == StatusFilter)
                              select new GenericPieceModel()
                              {
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
                                  RAS = p.RAS
                              }
                    ).ToList();

                return result;
            }

        }
        public static double GetPieceSolde(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                double res = 0;
                List<MappingReglementPieces> mappings = BD.MappingReglementPieces.Where(e => e.PieceID == NumPiece).ToList();
                if (mappings.Count() > 0)
                    res = mappings.Sum(e => e.Montant);
                return res;
            }
        }
        public static List<PieceModel> GetAllPiecesFournisseursForReglement(string Type, string CodeFilter, string LibelleFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter, int FournisseurFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece
                              join pa in BD.PieceAchat on p.NumPiece equals pa.NumPieceAchat
                              join ty in BD.TypesPieces on p.TypePiece equals ty.TypeName
                              where p.TypePiece == Type && (String.IsNullOrEmpty(CodeFilter) || p.NumPiece.Contains(CodeFilter))
                              && (String.IsNullOrEmpty(LibelleFilter) || p.Libelle.Contains(LibelleFilter))
                              && (DateFromFilter == null || p.DateCreation >= DateFromFilter)
                              && (DateToFilter == null || p.DateCreation <= DateToFilter)
                              && (MontantMinFilter == null || p.MontantFinal >= MontantMinFilter)
                              && (MontantMaxFilter == null || p.MontantFinal <= MontantMaxFilter)
                              && pa.CodeFournisseur == FournisseurFilter
                              && p.Statut == "VLD"
                              && p.Closed == false
                              select new PieceModel()
                              {
                                  Sens = ty.Sens,
                                  Category = ty.Categorie,
                                  ID = p.ID,
                                  NumPiece = p.NumPiece,
                                  DateCreation = p.DateCreation,
                                  MontantFinal = p.MontantFinal,

                              }
                    ).ToList();
                List<PieceModel> pieceModel = new List<PieceModel>();
                foreach (var res in result)
                {
                    List<MappingReglementPieces> sommeMontantsRegle = BD.MappingReglementPieces.Where(e => e.PieceID == res.NumPiece).ToList();
                    if (sommeMontantsRegle.Count() == 0)
                        pieceModel.Add(res);
                    else if (sommeMontantsRegle.Sum(e => e.Montant) < res.MontantFinal)
                        pieceModel.Add(res);
                }
                return pieceModel;
            }

        }
        public static List<PieceModel> GetAllPiecesFournisseursForRASReglement(string Type, string CodeFilter, string LibelleFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter, int FournisseurFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece
                              join pa in BD.PieceAchat on p.NumPiece equals pa.NumPieceAchat
                              join ty in BD.TypesPieces on p.TypePiece equals ty.TypeName
                              where p.TypePiece == Type && (String.IsNullOrEmpty(CodeFilter) || p.NumPiece.Contains(CodeFilter))
                              && (String.IsNullOrEmpty(LibelleFilter) || p.Libelle.Contains(LibelleFilter))
                              && (DateFromFilter == null || p.DateCreation >= DateFromFilter)
                              && (DateToFilter == null || p.DateCreation <= DateToFilter)
                              && (MontantMinFilter == null || p.MontantFinal >= MontantMinFilter)
                              && (MontantMaxFilter == null || p.MontantFinal <= MontantMaxFilter)
                              && pa.CodeFournisseur == FournisseurFilter
                              && p.Statut == "VLD"
                              && p.RASClosed == false
                              select new PieceModel()
                              {
                                  Sens = ty.Sens,
                                  Category = ty.Categorie,
                                  ID = p.ID,
                                  NumPiece = p.NumPiece,
                                  DateCreation = p.DateCreation,
                                  MontantFinal = p.MontantFinal,

                              }
                    ).ToList();
                List<PieceModel> pieceModel = new List<PieceModel>();
                foreach (var res in result)
                {
                    List<MappingReglementPieces> sommeMontantsRegle = BD.MappingReglementPieces.Where(e => e.PieceID == res.NumPiece).ToList();
                    if (sommeMontantsRegle.Count() == 0)
                        pieceModel.Add(res);
                    else if (sommeMontantsRegle.Sum(e => e.Montant) < res.MontantFinal)
                        pieceModel.Add(res);
                }
                return pieceModel;
            }

        }

        public static Piece GetByNumPiece(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                return (BD.Piece.Where(e => e.NumPiece == NumPiece).FirstOrDefault());
            }
        }
        public static PieceModel GetPieceWithTypeByNumPiece(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                PieceModel res = (from piece in BD.Piece
                                  join t in BD.TypesPieces on piece.TypePiece equals t.TypeName
                                  where piece.NumPiece == NumPiece
                                  select new PieceModel
                                  {
                                      NumPiece = piece.NumPiece,
                                      ID = piece.ID,
                                      Sens = t.Sens,
                                      Category = t.Categorie,
                                      TypePiece = piece.TypePiece,
                                      DateCreation = piece.DateCreation,
                                      CreatedBy = piece.CreatedBy,
                                      EditedBy = piece.EditedBy,
                                      LastEditTime = piece.LastEditTime,
                                      MontantTotal = piece.MontantTotal,
                                      MontantFinal = piece.MontantFinal,
                                      Statut = piece.Statut,
                                      RAS = piece.RAS

                                  }
                          ).FirstOrDefault();
                return (res);
            }
        }
        public static List<GenericPieceModel> GetAllPiecesClientsByType(string Type, string CodeFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter, int? ClientFilter, string StatusFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece
                              join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
                              join c in BD.Client on pv.CodeClient equals c.ID
                              join ty in BD.TypesPieces on p.TypePiece equals ty.TypeName
                              where p.TypePiece == Type
                              && (String.IsNullOrEmpty(CodeFilter) || p.NumPiece.Contains(CodeFilter))
                              && (DateFromFilter == null || p.DateCreation >= DateFromFilter)
                              && (DateToFilter == null || p.DateCreation <= DateToFilter)
                              && (MontantMinFilter == null || p.MontantFinal >= MontantMinFilter)
                              && (MontantMaxFilter == null || p.MontantFinal <= MontantMaxFilter)
                              && (ClientFilter == 0 || ClientFilter == null || c.ID == ClientFilter)
                              && (StatusFilter == "0" || p.Statut == StatusFilter)
                              select new GenericPieceModel()
                              {
                                  Sens = ty.Sens,
                                  Categorie = ty.Categorie,
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
                                  RAS = p.RAS,
                                  Reference = p.Reference
                              }
                    ).ToList();
                return result;
            }

        }
        public static void updateClosed(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Piece p = BD.Piece.Where(e => e.NumPiece == NumPiece).FirstOrDefault();
                List<MappingReglementPiecesModel> mappingList = MappingReglementPiecesBLL.GetMappingsByPiece(NumPiece).ToList();

                double amountFacSum = mappingList.Where(e => !e.RAS).Count() > 0 ? mappingList.Where(e => !e.RAS).Sum(e => e.Montant) : 0;

                if (Math.Abs(amountFacSum) == p.MontantFinal)
                    p.Closed = true;
                else p.Closed = false;

                double amountRASSum = mappingList.Where(e => e.RAS).Count() > 0 ? mappingList.Where(e => e.RAS).Sum(e => e.Montant) : 0;

                if (Math.Abs(amountRASSum) == p.RAS)
                    p.RASClosed = true;
                else p.RASClosed = false;
                BD.SaveChanges();
            }

        }
        public static List<PieceModel> GetAllPiecesClientsForReglement(string Type, string CodeFilter, string LibelleFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter, int ClientFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece

                              join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
                              where (String.IsNullOrEmpty(CodeFilter) || p.NumPiece.Contains(CodeFilter))
                              && (String.IsNullOrEmpty(LibelleFilter) || p.Libelle.Contains(LibelleFilter))
                              && (DateFromFilter == null || p.DateCreation >= DateFromFilter)
                              && (DateToFilter == null || p.DateCreation <= DateToFilter)
                              && (MontantMinFilter == null || p.MontantFinal >= MontantMinFilter)
                              && (MontantMaxFilter == null || p.MontantFinal <= MontantMaxFilter)
                              && p.Statut == "VLD"
                              && p.TypePiece == Type
                              && (pv.CodeClient == ClientFilter)
                              && p.Closed == false
                              select new PieceModel()
                              {
                                  ID = p.ID,
                                  NumPiece = p.NumPiece,
                                  DateCreation = p.DateCreation,
                                  MontantFinal = p.MontantFinal,

                              }
                    ).ToList();
                List<PieceModel> pieceModel = new List<PieceModel>();
                foreach (var res in result)
                {
                    List<MappingReglementPieces> sommeMontantsRegle = BD.MappingReglementPieces.Where(e => e.PieceID == res.NumPiece).ToList();
                    if (sommeMontantsRegle.Count() == 0)
                        pieceModel.Add(res);
                    else if (sommeMontantsRegle.Sum(e => e.Montant) < res.MontantFinal)
                        pieceModel.Add(res);
                }
                return pieceModel;
            }

        }
        public static List<PieceModel> GetAllPiecesClientsForRASReglement(string Type, string CodeFilter, string LibelleFilter, DateTime? DateFromFilter, DateTime? DateToFilter, double? MontantMinFilter, double? MontantMaxFilter, int ClientFilter)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var result = (from p in BD.Piece

                              join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
                              where (String.IsNullOrEmpty(CodeFilter) || p.NumPiece.Contains(CodeFilter))
                              && (String.IsNullOrEmpty(LibelleFilter) || p.Libelle.Contains(LibelleFilter))
                              && (DateFromFilter == null || p.DateCreation >= DateFromFilter)
                              && (DateToFilter == null || p.DateCreation <= DateToFilter)
                              && (MontantMinFilter == null || p.MontantFinal >= MontantMinFilter)
                              && (MontantMaxFilter == null || p.MontantFinal <= MontantMaxFilter)
                              && p.Statut == "VLD"
                              && p.TypePiece == Type
                              && (pv.CodeClient == ClientFilter)
                              && p.RASClosed == false
                              select new PieceModel()
                              {
                                  ID = p.ID,
                                  NumPiece = p.NumPiece,
                                  DateCreation = p.DateCreation,
                                  MontantFinal = p.MontantFinal,

                              }
                    ).ToList();
                List<PieceModel> pieceModel = new List<PieceModel>();
                foreach (var res in result)
                {
                    List<MappingReglementPieces> sommeMontantsRegle = BD.MappingReglementPieces.Where(e => e.PieceID == res.NumPiece).ToList();
                    if (sommeMontantsRegle.Count() == 0)
                        pieceModel.Add(res);
                    else if (sommeMontantsRegle.Sum(e => e.Montant) < res.MontantFinal)
                        pieceModel.Add(res);
                }
                return pieceModel;
            }

        }
        public static void SaveFactureChanges(string NumPiece, string Libelle, double Remise, string Reference)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                Piece piece = BD.Piece.Where(e => e.NumPiece == NumPiece).FirstOrDefault();
                if (piece != null)
                {
                    piece.Libelle = Libelle;
                    piece.Reference = Reference;
                    BD.SaveChanges();
                }
            }
        }
        public static double GetAllPiecesClientsBetweenDates(int ClientFilter, DateTime DateFrom, DateTime DateTo)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                var res = (from p in BD.Piece
                           join pv in BD.PieceVente on p.NumPiece equals pv.NumPieceVente
                           join tp in BD.TypesPieces on p.TypePiece equals tp.TypeName
                           where p.Statut == "VLD" && (p.TypePiece == "CFAC" || p.TypePiece == "CNOC" || p.Comptabilise == true) && pv.CodeClient == ClientFilter
                           && p.DateCreation >= DateFrom && p.DateCreation < DateTo
                           select new TextValueModel()
                           {
                               Val1 = p.MontantFinal,
                               Text = tp.Sens
                           }).ToList();

                double Sum = res.Sum(e => e.Val1 * int.Parse(e.Text)).Value;
                return Sum;
            }
        }

        public static double GetPieceAmount(string codeFacture)
        {
            using (TSD_Gestion_CommercialeEntities tsd = new TSD_Gestion_CommercialeEntities())
            {
                Piece p = tsd.Piece.Where(elt => elt.NumPiece == codeFacture).FirstOrDefault();
                return Math.Round(p.MontantFinal, 3);
            }

        }
        public static int GetPricingAmount(string codePricing)
        {
            using (TSD_Gestion_CommercialeEntities tsd = new TSD_Gestion_CommercialeEntities())
            {
                Pricing p = tsd.Pricing.Where(elt => elt.CodePricing == codePricing).FirstOrDefault();
                return Convert.ToInt32(p.MontantFinal);
            }

        }
        public static int GetRegAmount(int ReglementID)
        {
            using (TSD_Gestion_CommercialeEntities tsd = new TSD_Gestion_CommercialeEntities())
            {
                Reglements p = tsd.Reglements.Where(elt => elt.ID == ReglementID).FirstOrDefault();
                return Convert.ToInt32(p.Montant);
            }

        }

        public static string GetPieceType(string codeFacture)
        {
            using (TSD_Gestion_CommercialeEntities tsd = new TSD_Gestion_CommercialeEntities())
            {
                Piece p = tsd.Piece.Where(elt => elt.NumPiece == codeFacture).FirstOrDefault();
                return p.TypePiece;
            }

        }

    }
}
