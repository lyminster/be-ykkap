using Database.ConfigClass;
using Database.ModelsClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleJobTMS.Service
{
    internal class clsUtility
    {
        public class Document
        {
            public static void WriteTEXT(ArrayList arrText, string pathFile)
            {
                StreamWriter streamWriter = null;
                streamWriter = new StreamWriter(pathFile);
                for (int i = 0; arrText.Count > i; i++)
                {
                    if (arrText[i].ToString() != string.Empty)
                    {
                        streamWriter.WriteLine(arrText[i]);
                    }
                }
                streamWriter.Close();
                arrText.Clear();
            }
        }

        public class clsFolder
        {
            public static bool CreateTempFolder(string path, string keyName)
            {
                try
                {
                    string text = "\\temp_" + keyName + "\\";
                    if (Directory.Exists(path + text))
                    {
                        Directory.Delete(path + text, recursive: true);
                    }
                    Directory.CreateDirectory(path + text);
                    DirectoryInfo directoryInfo = new DirectoryInfo(path + text);
                    directoryInfo.Attributes &= ~FileAttributes.ReadOnly;
                    DirectorySecurity accessControl = directoryInfo.GetAccessControl();
                    accessControl.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
                    directoryInfo.SetAccessControl(accessControl);
                    SESSION.CurrentTempFolder = path + text;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public static void ClearTempFolder()
            {
                if (Directory.Exists(SESSION.CurrentTempFolder))
                {
                    Directory.Delete(SESSION.CurrentTempFolder, recursive: true);
                    SESSION.CurrentTempFolder = string.Empty;
                }
            }

            public static void RenameFICH(FileInfo fi, string ARJName, string path, string sPrefix)
            {
                SESSION.FullName = fi.FullName;
                string empty = string.Empty;
                string empty2 = string.Empty;
                string text = string.Empty;
                string text2 = string.Empty;
                string empty3 = string.Empty;
                empty = "FICH333_";
                empty2 = "FICH333C_";
                using (FileStream stream = new FileStream(SESSION.FullName, FileMode.Open))
                {
                    using StreamReader streamReader = new StreamReader(stream);
                    string empty4 = string.Empty;
                    while ((empty4 = streamReader.ReadLine()) != null)
                    {
                        if (empty4.Substring(22, 1) == "R" || empty4.Substring(22, 1) == "F" || empty4.Substring(22, 1) == "O" || empty4.Substring(22, 1) == "L" || empty4.Substring(22, 1) == "T" || empty4.Substring(22, 1) == "N" || empty4.Substring(22, 1) == "d" || empty4.Substring(22, 3) == "CMK")
                        {
                            text = empty4.ToString().Substring(0, 4);
                            text2 = empty4.ToString().Substring(12, 6);
                            break;
                        }
                        if (empty4.Substring(22, 1) == "Z")
                        {
                            text = empty4.ToString().Substring(0, 4);
                            text2 = "NO_TRX";
                            break;
                        }
                    }
                }
                empty3 = ((!sPrefix.Contains("FICH333C")) ? (empty + text + "." + text2 + "_" + ARJName) : (empty2 + text + "." + text2 + "_" + ARJName));
                if (File.Exists(path + empty3))
                {
                    File.Delete(path + empty3);
                    File.Move(fi.FullName, path + empty3);
                }
                else
                {
                    File.Move(fi.FullName, path + empty3);
                }
            }

            public static void Send2Store(FileInfo fi, string ARJName, string path)
            {
                if (File.Exists(path + fi.Name + "_" + ARJName))
                {
                    File.Delete(path + fi.Name + "_" + ARJName);
                    File.Move(fi.FullName, path + fi.Name + "_" + ARJName);
                }
                else
                {
                    File.Move(fi.FullName, path + fi.Name + "_" + ARJName);
                }
            }

            public static void Send2Backup(FileInfo fi, string path)
            {
                if (File.Exists(path + "BackUp_" + fi.Name))
                {
                    File.Delete(path + "BackUp_" + fi.Name);
                    File.Move(fi.FullName, path + "BackUp_" + fi.Name);
                }
                else
                {
                    File.Move(fi.FullName, path + "BackUp_" + fi.Name);
                }
            }

            public static void SendARJ2Backup(FileInfo fi, string path, string remarkARJ)
            {
                if (File.Exists(path + "BackUp_" + fi.Name + remarkARJ))
                {
                    File.Delete(path + "BackUp_" + fi.Name + remarkARJ);
                    File.Move(fi.FullName, path + "BackUp_" + fi.Name + remarkARJ);
                }
                else
                {
                    File.Move(fi.FullName, path + "BackUp_" + fi.Name + remarkARJ);
                }
            }

            public static void Send2Trash(FileInfo fi, string path)
            {
                if (File.Exists(path + "Transh_" + fi.Name))
                {
                    File.Delete(path + "Transh_" + fi.Name);
                    File.Move(fi.FullName, path + "Transh_" + fi.Name);
                }
                else
                {
                    File.Move(fi.FullName, path + "Transh_" + fi.Name);
                }
            }
        }

        public class Mail
        {
            public static bool Send(ParamMail parMail, zMail mail)
            {
                Attachment attachment = null;
                MailAddress mailAddress = null;
                MailMessage mailMessage = null;
                SmtpClient smtpClient = null;
                bool result = false;
                if (mail.sReceiversTo.Length > 0)
                {
                    try
                    {
                        mailAddress = new MailAddress(parMail.sSender, parMail.sSenderAlias);
                        mailMessage = new MailMessage();
                        mailMessage.From = mailAddress;
                        mailMessage.Body = mail.sBodyMessage;
                        mailMessage.Subject = mail.sSubject;
                        string[] sReceiversTo = mail.sReceiversTo;
                        foreach (string text in sReceiversTo)
                        {
                            if (text != string.Empty)
                            {
                                mailMessage.To.Add(new MailAddress(text));
                            }
                        }
                        if (mail.sReceiversCC != null && mail.sReceiversCC.Length > 0)
                        {
                            sReceiversTo = mail.sReceiversCC;
                            foreach (string text in sReceiversTo)
                            {
                                if (text != string.Empty)
                                {
                                    mailMessage.CC.Add(new MailAddress(text));
                                }
                            }
                        }
                        if (mail.sReceiversBCC != null && mail.sReceiversBCC.Length > 0)
                        {
                            sReceiversTo = mail.sReceiversBCC;
                            foreach (string text in sReceiversTo)
                            {
                                if (text != string.Empty)
                                {
                                    mailMessage.Bcc.Add(new MailAddress(text));
                                }
                            }
                        }
                        if (mail.sAttachment != null)
                        {
                            sReceiversTo = mail.sAttachment;
                            foreach (string text2 in sReceiversTo)
                            {
                                if (text2 != null && text2 != string.Empty)
                                {
                                    attachment = new Attachment(text2);
                                    mailMessage.Attachments.Add(attachment);
                                }
                            }
                        }
                        if (mailMessage.To.Count > 0)
                        {
                            smtpClient = new SmtpClient();
                            smtpClient.Host = parMail.sServerHost;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new NetworkCredential(parMail.sUserCredential, parMail.sPasswordCredential);
                            smtpClient.Send(mailMessage);
                        }
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        if (smtpClient != null)
                        {
                            smtpClient = null;
                        }
                        if (mailAddress != null)
                        {
                            mailAddress = null;
                        }
                        if (attachment != null)
                        {
                            attachment.Dispose();
                            attachment = null;
                        }
                        if (mailMessage != null)
                        {
                            mailMessage.Dispose();
                            mailMessage = null;
                        }
                    }
                }
                return result;
            }
        }

        public class Default
        {
            public class Text
            {
                public const string Enter = " \r\n";
            }

            public const string DEF_DateTime = "1900-01-01 00:00:00";
        }

        public class ManipulateString
        {
            public static string Format(string DefString, int TxtLenght, string Value)
            {
                string empty = string.Empty;
                string text = string.Empty;
                int num = 0;
                num = TxtLenght - Value.Length;
                for (int i = 0; i < num; i++)
                {
                    text += DefString;
                }
                return text + Value;
            }

            public static string EmptySpace(int Lenght)
            {
                string empty = string.Empty;
                string text = string.Empty;
                for (int i = 0; i < Lenght; i++)
                {
                    text += " ";
                }
                return text;
            }
        }
        public static SystemConfig systemConfig;
        public static string ReadXML(string parName )
        {
           
            string result = "";
            if (parName == "AppName") result = systemConfig.AppName;
            if (parName == "SourceFTP") result = systemConfig.SourceFTP;
            if (parName == "SourcePath") result = systemConfig.SourcePath;
            if (parName == "BackupPath") result = systemConfig.BackupPath;
            if (parName == "BackupFTP") result = systemConfig.BackupFTP;
            if (parName == "TrashPath") result = systemConfig.TrashPath;
            if (parName == "UnZipperLoc") result = systemConfig.UnZipperLoc;
            if (parName == "ReduceDecimal") result = systemConfig.ReduceDecimal;
            if (parName == "Server") result = systemConfig.Server;
            if (parName == "Database") result = systemConfig.Database;
            if (parName == "User") result = systemConfig.User;
            if (parName == "Password") result = systemConfig.Password;
            if (parName == "sServerHost") result = systemConfig.sServerHost;
            if (parName == "sUserCredential") result = systemConfig.sUserCredential;
            if (parName == "sPasswordCredential") result = systemConfig.sPasswordCredential;
            if (parName == "sSender") result = systemConfig.sSender;
            if (parName == "sSenderAlias") result = systemConfig.sSenderAlias;
            if (parName == "sSubject_Err") result = systemConfig.sSubject_Err;
            if (parName == "HeaderMessage_Err") result = systemConfig.HeaderMessage_Err;
            if (parName == "FooterMessage_Err") result = systemConfig.FooterMessage_Err;
            if (parName == "sReceiversTo_Err") result = systemConfig.sReceiversTo_Err;
            if (parName == "sReceiversCC_Err") result = systemConfig.sReceiversCC_Err;
            if (parName == "sReceiversBCC_Err") result = systemConfig.sReceiversBCC_Err;
            if (parName == "sSubject_art") result = systemConfig.sSubject_art;
            if (parName == "HeaderMessage_art") result = systemConfig.HeaderMessage_art;
            if (parName == "FooterMessage_art") result = systemConfig.FooterMessage_art;
            if (parName == "sReceiversTo_art") result = systemConfig.sReceiversTo_art;
            if (parName == "sReceiversCC_art") result = systemConfig.sReceiversCC_art;
            if (parName == "sReceiversCC_art") result = systemConfig.sReceiversCC_art;
            if (parName == "sReceiversBCC_art") result = systemConfig.sReceiversBCC_art;
            if (parName == "sSubject_sls") result = systemConfig.sSubject_sls;
            if (parName == "HeaderMessage_sls") result = systemConfig.HeaderMessage_sls;
            if (parName == "FooterMessage_sls") result = systemConfig.FooterMessage_sls;
            if (parName == "sReceiversTo_sls") result = systemConfig.sReceiversTo_sls;
            if (parName == "sReceiversCC_sls") result = systemConfig.sReceiversCC_sls;
            if (parName == "sReceiversBCC_sls") result = systemConfig.sReceiversBCC_sls;
            if (parName == "sSubject_mvt") result = systemConfig.sSubject_mvt;
            if (parName == "HeaderMessage_mvt") result = systemConfig.HeaderMessage_mvt;
            if (parName == "FooterMessage_mvt") result = systemConfig.FooterMessage_mvt;
            if (parName == "sReceiversTo_mvt") result = systemConfig.sReceiversTo_mvt;
            if (parName == "sReceiversCC_mvt") result = systemConfig.sReceiversCC_mvt;
            if (parName == "sReceiversBCC_mvt") result = systemConfig.sReceiversBCC_mvt;
            
            return result;
        }

        public static bool ProcessesRunning(string ProcessName)
        {
            if (Process.GetProcessesByName(ProcessName).Length > 1)
            {
                return true;
            }
            return false;
        }

        public static ArrayList GetZipFiles(string SourceFTP, string Extention)
        {
            ArrayList arrayList = new ArrayList();
            string empty = string.Empty;
            if (SourceFTP != string.Empty)
            {
                string[] files = Directory.GetFiles(SourceFTP, "*." + Extention, SearchOption.AllDirectories);
                FileInfo fileInfo = null;
                string[] array = files;
                foreach (string text in array)
                {
                    empty = MovRename(text);
                    File.Move(text, empty);
                    fileInfo = new FileInfo(empty);
                    arrayList.Add(fileInfo);
                }
            }
            return arrayList;
        }

        public static ArrayList GetFICHFiles(string SourcePath)
        {
            ArrayList arrayList = new ArrayList();
            string empty = string.Empty;
            if (Directory.GetFiles(SourcePath).Length != 0)
            {
                string[] files = Directory.GetFiles(SourcePath, "FICH333*.*", SearchOption.AllDirectories);
                FileInfo fileInfo = null;
                string[] array = files;
                foreach (string fileName in array)
                {
                    fileInfo = new FileInfo(fileName);
                    arrayList.Add(fileInfo);
                }
            }
            return arrayList;
        }

        public static ArrayList GetFICHCOMFiles(string SourcePath)
        {
            ArrayList arrayList = new ArrayList();
            string empty = string.Empty;
            if (Directory.GetFiles(SourcePath).Length != 0)
            {
                string[] files = Directory.GetFiles(SourcePath, "FICH333C*.*", SearchOption.AllDirectories);
                FileInfo fileInfo = null;
                string[] array = files;
                foreach (string fileName in array)
                {
                    fileInfo = new FileInfo(fileName);
                    arrayList.Add(fileInfo);
                }
            }
            return arrayList;
        }

        public static string MovRename(string filename)
        {
            string empty = string.Empty;
            string empty2 = string.Empty;
            empty = "FICH333_";
            return filename.Replace(empty, empty);
        }

        

        public static string CheckNull(string Text)
        {
            string empty = string.Empty;
            return (Text == null) ? string.Empty : Text;
        }

        public static int CheckNull(int Text)
        {
            int num = 0;
            return Text;
        }

        public static double CheckNull(double Text)
        {
            double num = 0.0;
            return Text;
        }

        public static bool CheckNull(bool Text)
        {
            bool flag = false;
            return Text;
        }

        public static string CheckDigitDocNumber(string szDocNumber)
        {
            string empty = string.Empty;
            int num = Convert.ToInt32(szDocNumber);
            return szDocNumber.ToString().PadLeft(7, '0');
        }

        public static string CheckDigitStoreID(string szStoreID)
        {
            string empty = string.Empty;
            int num = Convert.ToInt32(szStoreID);
            return szStoreID.ToString().PadLeft(4, '0');
        }

        public static string CheckDigit(string PLU)
        {
            bool flag = false;
            string text = PLU;
            if (PLU != string.Empty)
            {
                SpecialArticleData specialArticleData = new SpecialArticleData();
                for (int i = 0; i < SESSION.PLU_Alteration.Count; i++)
                {
                    specialArticleData = (SpecialArticleData)SESSION.PLU_Alteration[i];
                    if (PLU == specialArticleData.PLU)
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    text += CheckDigitZara(PLU);
                    if (text.Substring(0, 1) == "0")
                    {
                        text = "O" + text.Substring(1, text.Length - 1);
                    }
                }
            }
            return text;
        }

        private static string CheckDigitZara(string sItem)
        {
            string empty = string.Empty;
            string empty2 = string.Empty;
            double num = 0.0;
            empty = "3131313131313";
            for (int i = 0; i < 13; i++)
            {
                num += Convert.ToDouble(sItem.Substring(i, 1)) * Convert.ToDouble(empty.Substring(i, 1));
            }
            empty2 = (num % 10.0).ToString().Trim();
            return empty2.Substring(empty2.Length - 1);
        }

        public static string GiftCard(string szArticle)
        {
            string empty = string.Empty;
            if (szArticle.Substring(0, 2) == "57")
            {
                return "ZZZ5700000O";
            }
            return szArticle;
        }

        public static string CheckDigitMQCS(ushort usMQCS, int iType)
        {
            string result = string.Empty;
            double num = 0.0;
            int num2 = 0;
            string empty = string.Empty;
            empty = usMQCS.ToString();
            num2 = empty.Length;
            switch (iType)
            {
                case 1:
                    switch (num2)
                    {
                        case 1:
                            result = "000" + empty;
                            break;
                        case 2:
                            result = "00" + empty;
                            break;
                        case 3:
                            result = "0" + empty;
                            break;
                        case 4:
                            result = empty;
                            break;
                    }
                    break;
                case 2:
                    switch (num2)
                    {
                        case 1:
                            result = "00" + empty;
                            break;
                        case 2:
                            result = "0" + empty;
                            break;
                        case 3:
                            result = empty;
                            break;
                    }
                    break;
                case 3:
                    switch (num2)
                    {
                        case 1:
                            result = "00" + empty;
                            break;
                        case 2:
                            result = "0" + empty;
                            break;
                        case 3:
                            result = empty;
                            break;
                    }
                    break;
                case 4:
                    switch (num2)
                    {
                        case 1:
                            result = "0" + empty;
                            break;
                        case 2:
                            result = empty;
                            break;
                    }
                    break;
            }
            return result;
        }
    }
}