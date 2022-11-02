using Database.ModelsClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{

    public class SESSION
    {
        public static bool IsHaveRun = false;

        public static string ZipFileName = string.Empty;

        public static string OriginalFileName = string.Empty;

        public static string FileName = string.Empty;

        public static string FullName = string.Empty;

        public static string FileLog = string.Empty;

        public static string LogDate = string.Empty;

        public static string CurrentTempFolder = string.Empty;

        public static string UserName = string.Empty;

        public static string RootName = string.Empty;

        public static ArrayList PLU_Alteration = new ArrayList();

        public static ParamMail parMail;

        public static zMail MailErrorLog;

        public static zMail MailArticle;

        public static zMail MailSales;

        public static zMail MailMovement;
    }
}