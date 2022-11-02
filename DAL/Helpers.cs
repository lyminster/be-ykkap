using Database.ConfigClass;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace DAL
{
    public class Helpers
    {
        public static List<FileInfo> GetFICHFiles(string SourcePath)
        {
            List<FileInfo> arrayList = new List<FileInfo>();
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
        public class FileData
        {
            public string FileName { get; set; }
            public byte[] FileByte { get; set; }
        }
        public static bool SentMail(String Subject, string Body, string To, String Cc, string Bcc, EmailConfig _EmailConfig, List<FileData> files)
        {

            Boolean result = true;

            try
            {

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.Subject = Subject;

                message.From = new MailAddress(_EmailConfig.UserName);
                string to = To;
                string cc = Cc;
                string bcc = Bcc;

                foreach (var xx in to.Split(';').ToList())
                {
                    var email = xx;
                    if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                    {

                        message.To.Add(new MailAddress(email));
                    }
                    else
                    {
                        message.To.Add(new MailAddress(_EmailConfig.DefaultTo));

                    }
                }
                if (!String.IsNullOrWhiteSpace(bcc))
                {
                    foreach (var xx in bcc.Split(';').ToList())
                    {

                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.Bcc.Add(new MailAddress(email));

                        }
                        else
                        {
                            message.Bcc.Add(new MailAddress(_EmailConfig.DefaultBCC));

                        }
                    }


                }
                message.Bcc.Add(((_EmailConfig.DefaultBCC)));
                if (!String.IsNullOrWhiteSpace(cc))
                {
                    foreach (var xx in cc.Split(';').ToList())
                    {
                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.CC.Add(new MailAddress(email));
                        }
                        else
                        {
                            message.CC.Add(new MailAddress(_EmailConfig.DefaultCC));

                        }
                    }
                }


                message.Subject = Subject;




                foreach (var item in files.ToList())
                {
                    Attachment att = new Attachment(new MemoryStream(item.FileByte), item.FileName);
                    message.Attachments.Add(att);
                }






                message.IsBodyHtml = true; //to make message body as html  
                message.Body = Body;
                smtp.Port = _EmailConfig.Port;
                smtp.Host = _EmailConfig.Host; //for gmail host  
                if (_EmailConfig.SecureSocketOptions == MailKit.Security.SecureSocketOptions.SslOnConnect)
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;

                }
                smtp.Credentials = new NetworkCredential(_EmailConfig.UserName, _EmailConfig.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                //var smtp = new SmtpClient();
                //smtp.Connect(_EmailConfig.Host, _EmailConfig.Port, _EmailConfig.SecureSocketOptions);
                //try
                //{
                //    if (_EmailConfig.Password != null)
                //    {
                //        smtp.Authenticate(_EmailConfig.UserName, _EmailConfig.Password);
                //    }
                //}
                //catch (Exception)
                //{

                //}
                //smtp.Send(message);
                //smtp.Disconnect(true);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }









        public static bool SentMailWithImage(String Subject, string Body, string To, String Cc, string Bcc, EmailConfig _EmailConfig, List<FileData> files, Stream stream1, Stream stream2)
        {

            Boolean result = true;

            try
            {

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.Subject = Subject;

                message.From = new MailAddress(_EmailConfig.UserName);
                string to = To;
                string cc = Cc;
                string bcc = Bcc;



                if (stream1 != null)
                {
                    AlternateView view = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);
                    LinkedResource resource = new LinkedResource(stream1, "image/jpeg");
                    resource.ContentId = "image1";
                    view.LinkedResources.Add(resource);
                    message.AlternateViews.Add(view);
                }


                if (stream2 != null)
                {
                    AlternateView view = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);
                    LinkedResource resource = new LinkedResource(stream2, "image/jpeg");
                    resource.ContentId = "image2";
                    view.LinkedResources.Add(resource);
                    message.AlternateViews.Add(view);
                }


                foreach (var xx in to.Split(';').ToList())
                {
                    var email = xx;
                    if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                    {

                        message.To.Add(new MailAddress(email));
                    }
                    else
                    {
                        message.To.Add(new MailAddress(_EmailConfig.DefaultTo));

                    }
                }
                if (!String.IsNullOrWhiteSpace(bcc))
                {
                    foreach (var xx in bcc.Split(';').ToList())
                    {

                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.Bcc.Add(new MailAddress(email));

                        }
                        else
                        {
                            message.Bcc.Add(new MailAddress(_EmailConfig.DefaultBCC));

                        }
                    }


                }
                message.Bcc.Add(((_EmailConfig.DefaultBCC)));
                if (!String.IsNullOrWhiteSpace(cc))
                {
                    foreach (var xx in cc.Split(';').ToList())
                    {
                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.CC.Add(new MailAddress(email));
                        }
                        else
                        {
                            message.CC.Add(new MailAddress(_EmailConfig.DefaultCC));

                        }
                    }
                }


                message.Subject = Subject;




                foreach (var item in files.ToList())
                {
                    Attachment att = new Attachment(new MemoryStream(item.FileByte), item.FileName);
                    message.Attachments.Add(att);
                }






                message.IsBodyHtml = true; //to make message body as html  
                message.Body = Body;
                smtp.Port = _EmailConfig.Port;
                smtp.Host = _EmailConfig.Host; //for gmail host  
                if (_EmailConfig.SecureSocketOptions == MailKit.Security.SecureSocketOptions.SslOnConnect)
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;

                }
                smtp.Credentials = new NetworkCredential(_EmailConfig.UserName, _EmailConfig.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                //var smtp = new SmtpClient();
                //smtp.Connect(_EmailConfig.Host, _EmailConfig.Port, _EmailConfig.SecureSocketOptions);
                //try
                //{
                //    if (_EmailConfig.Password != null)
                //    {
                //        smtp.Authenticate(_EmailConfig.UserName, _EmailConfig.Password);
                //    }
                //}
                //catch (Exception)
                //{

                //}
                //smtp.Send(message);
                //smtp.Disconnect(true);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        public static System.Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }
    }
}
