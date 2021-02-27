

using Microsoft.Reporting.WebForms;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using TSD_BLL;
using TSD_DAL.Model;

namespace Gestion_Commerciale
{
    public class ReportGenerator
    {
        private Microsoft.Reporting.WebForms.ReportViewer mainReportViewer = new Microsoft.Reporting.WebForms.ReportViewer();

        public byte[] ExportSSRSReport(string reportFullPath, Dictionary<string, string> paramList, string reportExtension, string FileName = "")
        {
            mainReportViewer.ServerReport.ReportServerUrl = new Uri(@"http://DESKTOP-7LEQ32O/ReportServer");
            //mainReportViewer.ServerReport.ReportServerUrl = new Uri(@"http://best-technology/ReportServer");

            mainReportViewer.ServerReport.ReportPath = reportFullPath;
            List<ReportParameter> rParams = new List<ReportParameter>();
            foreach (KeyValuePair<string, string> Param in paramList)
            {
                ReportParameter rParameter = new ReportParameter(Param.Key);


                if (Param.Value == null)
                    rParameter.Values.Add(null);
                else if (!Param.Value.Contains("**"))
                    //single param
                    rParameter.Values.Add(Param.Value);
                else
                {
                    string[] separator = new string[1] { "**" };
                    string[] valueList = Param.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    rParameter.Values.AddRange(valueList);
                }
                rParams.Add(rParameter);
            }



            if (rParams.Count > 0)
                mainReportViewer.ServerReport.SetParameters(rParams);
            //mainReportViewer.RefreshReport();
            //string deviceInfo = string.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth></DeviceInfo>", "11.7in", "8.3in");
            byte[] FileData =  mainReportViewer.ServerReport.Render(reportExtension);
            return (FileData);
        }

        public string ImprimerRapportFacture(string CodeFacture, string Aurthority)
        {


            ReportGenerator rG = new ReportGenerator();
            string FullReportName = String.Empty;
            double MontantFacture = Piece_BLL.GetPieceAmount(CodeFacture);
            string TypePiece = Piece_BLL.GetPieceType(CodeFacture);
            string type = "";
            if (TypePiece == "CFAC")
                type = "F";
            else if (TypePiece == "BLIV")
                type = "BL";
            else
                type = "BC";
            int IntMontantFacture = (int)MontantFacture;
            string MontantEnChiffres = ConvertisseurChiffresLettres.converti(IntMontantFacture);
            string DecimalAmount = MontantFacture.ToString();
             if(DecimalAmount.Contains(","))
            {


                DecimalAmount = DecimalAmount.Split(',')[1];

                DecimalAmount = "0," + DecimalAmount;
                int Decimal = (int)(Double.Parse(DecimalAmount) * 1000);
                MontantEnChiffres = MontantEnChiffres + " dinars et " + ConvertisseurChiffresLettres.converti(Decimal) + " millimes";
            }  
            else
                MontantEnChiffres = MontantEnChiffres + " dinars";
            Dictionary<string, string> paramList = ParameterBuilder.GetFactureClientParams(CodeFacture, MontantEnChiffres,type, ref FullReportName);
            byte[] pdfDoc = rG.ExportSSRSReport(FullReportName, paramList, "Pdf");
            Guid FileGuid = Guid.NewGuid();
            //  PDFGenerator.MergePdfs(PdfDocs, Path.Combine(WorkingDirectory, FileGuid + ".pdf"));
           // System.IO.File.WriteAllBytes("C:\\Users\\USER\\AppData\\Local\\Temp\\" + FileGuid.ToString() + ".pdf", pdfDoc);

            System.IO.File.WriteAllBytes("C:\\Users\\LENOVO\\Temp\\" + FileGuid.ToString() + ".pdf", pdfDoc);
            //return "http://sgc2-p2c.com/Temps/" + FileGuid.ToString() + ".pdf";
            return "http://sgc-p2c.com/Temp/" + FileGuid.ToString() + ".pdf";


        }
        public string ImprimerReleveCompte(DateTime DateFromFilter, DateTime DateToFilter, int ClientFilter, string Aurthority)
        {


            ReportGenerator rG = new ReportGenerator();
            string FullReportName = String.Empty;

            Dictionary<string, string> paramList = ParameterBuilder.GetReleveCompteParams(DateFromFilter, DateToFilter, ClientFilter, ref FullReportName);
            byte[] pdfDoc = rG.ExportSSRSReport(FullReportName, paramList, "Pdf");
            Guid FileGuid = Guid.NewGuid();
            //  PDFGenerator.MergePdfs(PdfDocs, Path.Combine(WorkingDirectory, FileGuid + ".pdf"));
            // System.IO.File.WriteAllBytes("C:\\Users\\USER\\AppData\\Local\\Temp\\" + FileGuid.ToString() + ".pdf", pdfDoc);

            System.IO.File.WriteAllBytes("C:\\Users\\LENOVO\\Temp\\" + FileGuid.ToString() + ".pdf", pdfDoc);
            //return "http://sgc2-p2c.com/Temps/" + FileGuid.ToString() + ".pdf";
            return "http://sgc-p2c.com/Temp/" + FileGuid.ToString() + ".pdf";


        }
        
        public string ImprimerRapportPricing(string CodePricing, string Aurthority)
        {


            ReportGenerator rG = new ReportGenerator();
            string FullReportName = String.Empty;
            int MontantFacture = Piece_BLL.GetPricingAmount(CodePricing);

            string MontantEnChiffres = ConvertisseurChiffresLettres.converti(MontantFacture);
            Dictionary<string, string> paramList = ParameterBuilder.GetPricingClientParams(CodePricing, MontantEnChiffres, ref FullReportName);
            byte[] pdfDoc = rG.ExportSSRSReport(FullReportName, paramList, "Pdf");
            Guid FileGuid = Guid.NewGuid();
            //  PDFGenerator.MergePdfs(PdfDocs, Path.Combine(WorkingDirectory, FileGuid + ".pdf"));
            // System.IO.File.WriteAllBytes("C:\\Users\\USER\\AppData\\Local\\Temp\\" + FileGuid.ToString() + ".pdf", pdfDoc);

            System.IO.File.WriteAllBytes("C:\\Users\\LENOVO\\Temp\\" + FileGuid.ToString() + ".pdf", pdfDoc);
            //return "http://sgc2-p2c.com/Temps/" + FileGuid.ToString() + ".pdf";
            return "http://sgc-p2c.com/Temp/" + FileGuid.ToString() + ".pdf";


        }
        public string ImprimerRapportReglement(string ReglementID, string Aurthority)
        {


            ReportGenerator rG = new ReportGenerator();
            string FullReportName = String.Empty;
            ReglementsModel Reg = Reglements_BLL.getReglementsByCode(int.Parse(ReglementID));
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            if (Reg.LibelleTypeReglement == "Traite")
            {
                int MontantReg = Piece_BLL.GetRegAmount(int.Parse(ReglementID));

                string MontantEnChiffres = ConvertisseurChiffresLettres.converti(MontantReg);
                paramList = ParameterBuilder.GetTraiteParams(ReglementID, MontantEnChiffres, ref FullReportName);
            }
            else
            {
                paramList = ParameterBuilder.GetRASParams(ReglementID, ref FullReportName);
            }
            byte[] pdfDoc = rG.ExportSSRSReport(FullReportName, paramList, "Pdf");
            Guid FileGuid = Guid.NewGuid();
            //  PDFGenerator.MergePdfs(PdfDocs, Path.Combine(WorkingDirectory, FileGuid + ".pdf"));
            // System.IO.File.WriteAllBytes("C:\\Users\\USER\\AppData\\Local\\Temp\\" + FileGuid.ToString() + ".pdf", pdfDoc);

            System.IO.File.WriteAllBytes("C:\\Users\\LENOVO\\Temp\\" + FileGuid.ToString() + ".pdf", pdfDoc);
            //return "http://sgc2-p2c.com/Temps/" + FileGuid.ToString() + ".pdf";
            return "http://sgc-p2c.com/Temp/" + FileGuid.ToString() + ".pdf";


        }

    }
}