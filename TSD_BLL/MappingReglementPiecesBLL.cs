using System;
using System.Collections.Generic;
using System.Linq;
using TSD_DAL.Model;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class MappingReglementPiecesBLL : GenericBLL<MappingReglementPieces>
    {
        public static bool CheckIfTotalAmmountIsExceed(string numPiece, double Montant, int reglementID, string sens, bool RAS)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {

                Montant = Montant * int.Parse(sens);
                List<MappingReglementPiecesModel> mappings = GetMappingsByPiece(numPiece).Where(e => e.RAS == RAS).ToList();
                Piece p = BD.Piece.Where(e => e.NumPiece == numPiece).FirstOrDefault();

                double MontantPiece = RAS ? p.RAS : p.MontantFinal;

                if (mappings.Count() > 0)
                {
                    if (reglementID == 0)
                        Montant += mappings.Sum(e => e.Montant);
                    else
                    {
                        MappingReglementPiecesModel m = mappings.Where(e => e.ReglementID == reglementID).FirstOrDefault();
                        m.Montant = 0;

                        Montant += mappings.Sum(e => e.Montant);
                    }

                }

                return MontantPiece < Math.Abs(Montant);
            }
        }
        public static double GetSumMappingsByPiece(string numPiece, bool RAS)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<MappingReglementPiecesModel> mappings = (from mmp in BD.MappingReglementPieces
                                                              join reg in BD.Reglements on mmp.ReglementID equals reg.ID
                                                              where mmp.PieceID == numPiece
                                                              where reg.RAS == RAS
                                                              select new MappingReglementPiecesModel
                                                              {
                                                                  ID = mmp.ID,
                                                                  ReglementID = mmp.ReglementID,
                                                                  PieceID = mmp.PieceID,
                                                                  Montant = mmp.Montant,
                                                                  RAS = reg.RAS
                                                              }).ToList();
                double sommeMontantsRegle = 0;
                if (mappings.Count > 0)
                {
                    sommeMontantsRegle = mappings.Sum(e => e.Montant);
                }
                return sommeMontantsRegle;
            }
        }

        public static double GetSumMappingsByReglement(int ReglementID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<MappingReglementPieces> mappings = BD.MappingReglementPieces.Where(e => e.ReglementID == ReglementID).ToList();
                double sommeMontantsRegle = 0;
                if (mappings.Count > 0)
                {
                    sommeMontantsRegle = mappings.Sum(e => e.Montant);
                }
                return sommeMontantsRegle;
            }
        }
        public static List<MappingReglementPieces> GetMappingsByReglement(int ReglementID)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<MappingReglementPieces> mappings = BD.MappingReglementPieces.Where(e => e.ReglementID == ReglementID).ToList();
                return mappings;
            }
        }
        public static List<MappingReglementPiecesModel> GetMappingsByPiece(string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                List<MappingReglementPiecesModel> mappings = (from mmp in BD.MappingReglementPieces
                                                              join reg in BD.Reglements on mmp.ReglementID equals reg.ID
                                                              where mmp.PieceID == NumPiece
                                                              select new MappingReglementPiecesModel
                                                              {
                                                                  ID = mmp.ID,
                                                                  ReglementID = mmp.ReglementID,
                                                                  PieceID = mmp.PieceID,
                                                                  Montant = mmp.Montant,
                                                                  RAS = reg.RAS
                                                              }).ToList();

                return mappings;
            }
        }
        public static MappingReglementPieces GetMappingsByReglementAndPiece(int ReglementID, string NumPiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                MappingReglementPieces mapping = BD.MappingReglementPieces.Where(e => e.ReglementID == ReglementID && e.PieceID == NumPiece).FirstOrDefault();
                return mapping;
            }
        }

    }
}
