using System;
using System.Linq;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public static class SeriesBLL
    {
        public static string GenerateNewSerie(string TypePiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                SeriesTable series = BD.SeriesTable.FirstOrDefault();
                string Result = "";
                string CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
                switch (TypePiece)
                {
                    case "FFAC":
                    case "FNOC":
                        {
                            series.FFCounter += 1;
                            Result = "A" + CurrentYear + "-" + (series.FFCounter).ToString().PadLeft(4, '0');
                        }; break;
                    case "CFAC":
                    case "CNOC":
                        {
                            series.FCCounter += 1;
                            Result = CurrentYear + "-" + (series.FCCounter).ToString().PadLeft(4, '0');
                        }; break;
                    case "BLIV":
                        {
                            series.BLCounter += 1;
                            Result = "BL" + CurrentYear + "-" + (series.BLCounter).ToString().PadLeft(4, '0');
                        }; break;
                    case "BCOM":
                        {
                            series.BCCounter += 1;
                            Result = "BC" + CurrentYear + "-" + (series.BCCounter + 1).ToString().PadLeft(4, '0');
                        }; break;
                }
                return Result;
            }
        }
        public static void UpdateLastSerie(string TypePiece)
        {
            using (TSD_Gestion_CommercialeEntities BD = new TSD_Gestion_CommercialeEntities())
            {
                SeriesTable series = BD.SeriesTable.FirstOrDefault();

                string CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
                switch (TypePiece)
                {
                    case "FFAC":
                    case "FNOC":
                        {
                            series.FFCounter += 1;

                        }; break;
                    case "CFAC":
                    case "CNOC":
                        {
                            series.FCCounter += 1;

                        }; break;
                    case "BLIV":
                        {
                            series.BLCounter += 1;

                        }; break;
                    case "BCOM":
                        {
                            series.BCCounter += 1;

                        }; break;
                }
                BD.SaveChanges();
            }
        }
    }
}
