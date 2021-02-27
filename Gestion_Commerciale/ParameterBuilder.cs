using System;
using System.Collections.Generic;

namespace Gestion_Commerciale
{
    public  class ParameterBuilder
    {
        public static Dictionary<string, string> GetFactureClientParams(string CodeFacture,string MontantEnChiffres,string type, ref string FullReportName)
        {

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList.Add("NumPiece", CodeFacture);
            paramList.Add("TimbreFiscale", "600");
            paramList.Add("type", type);
            paramList.Add("MontantEnChiffres", MontantEnChiffres);
            
            FullReportName = "/P2CPDFReports/FactureClient";
            return (paramList);

        }
        public static Dictionary<string, string> GetReleveCompteParams(DateTime DateFromFilter, DateTime DateToFilter, int ClientFilter, ref string FullReportName)
        {

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList.Add("DateFrom", DateFromFilter.ToString());
            paramList.Add("DateTo", DateToFilter.ToString());
            paramList.Add("Client", ClientFilter.ToString());

            FullReportName = "/P2CPDFReports/ReleveCompte";
            return (paramList);

        }
        public static Dictionary<string, string> GetPricingClientParams(string PricingCode, string MontantEnChiffres, ref string FullReportName)
        {

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList.Add("NumPiece", PricingCode);
            paramList.Add("TimbreFiscale", "600");
            paramList.Add("MontantEnChiffres", MontantEnChiffres);

            FullReportName = "/P2CPDFReports/Pricing";
            return (paramList);

        }
        public static Dictionary<string, string> GetRASParams(string reglementID, ref string FullReportName)
        {

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList.Add("reglementID", reglementID);

            FullReportName = "/P2CPDFReports/RAS";
            return (paramList);

        }
        public static Dictionary<string, string> GetTraiteParams(string reglementID, string MontantEnChiffres, ref string FullReportName)
        {

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList.Add("reglementID", reglementID);
            paramList.Add("montantChiffres", MontantEnChiffres);           
            FullReportName = "/P2CPDFReports/Traite";
            return (paramList);

        }
    }
}