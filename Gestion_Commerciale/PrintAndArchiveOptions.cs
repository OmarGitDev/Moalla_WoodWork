using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Commerciale
{
    public class PrintAndArchiveOptions
    {
        public PrintAndArchiveOptions(bool print, bool archive, bool hideLogo, bool showRemarks,
            string currentDataBasePrefix, string sqlServerName, string currentUserName,
            string temporaryPhysicalDirectoryPath, string temporaryVirtualDirectoryName,
            string authority, string currentSerieDataBaseSuffix, string archiveDirectory, string dbPrefix = "", string PDocSignerAutomaticDirectoryPath = "")
        {
            Print = print;
            Archive = archive;
            HideLogo = hideLogo;
            ShowRemarks = showRemarks;
            CurrentDataBasePrefix = currentDataBasePrefix;
            SqlServerName = sqlServerName;
            CurrentUserName = currentUserName;
            TemporaryPhysicalDirectoryPath = temporaryPhysicalDirectoryPath;
            TemporaryVirtualDirectoryName = temporaryVirtualDirectoryName;
            Authority = authority;
            CurrentSerieDataBaseSuffix = currentSerieDataBaseSuffix;
            ArchiveDirectory = archiveDirectory;
            DbPrefix = dbPrefix;
            pDocSignerAutomaticDirectoryPath = PDocSignerAutomaticDirectoryPath;
        }
        public bool Print { get; set; }
        public bool Archive { get; set; }
        public bool HideLogo { get; set; }
        public bool ShowRemarks { get; set; }
        public string CurrentDataBasePrefix { get; set; }
        public string SqlServerName { get; set; }
        public string CurrentUserName { get; set; }
        public string TemporaryPhysicalDirectoryPath { get; set; }
        public string TemporaryVirtualDirectoryName { get; set; }
        public string Authority { get; set; }
        public string CurrentSerieDataBaseSuffix { get; set; }
        public string ArchiveDirectory { get; set; }
        public string DbPrefix { get; set; }
        public string pDocSignerAutomaticDirectoryPath { get; set; }

    }
}
