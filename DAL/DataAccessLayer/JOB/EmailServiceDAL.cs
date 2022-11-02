using Database.ConfigClass;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DAL.DataAccessLayer.JOB
{
    public class EmailService
    {
        private readonly EmailConfig _EmailConfig;
        public EmailService(EmailConfig emailConfig)
        {
            _EmailConfig = emailConfig;

        }


        public bool SentMail(String Subject, string Body, string To, String Cc, string Bcc)
        {

            Boolean result = true;

            try
            {
                MailMessage message = new MailMessage();

                message.From = new MailAddress(_EmailConfig.UserName);
                string to = To;
                string cc = Cc;
                string bcc = Bcc;

                foreach (var xx in to.Split(';').ToList())
                {
                    var email = xx;
                    if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                    {

                        message.To.Add(((email)));
                    }
                    else
                    {
                        message.To.Add(((_EmailConfig.DefaultTo)));

                    }
                }
                if (!String.IsNullOrWhiteSpace(bcc))
                {
                    foreach (var xx in bcc.Split(';').ToList())
                    {

                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.Bcc.Add(((email)));
                        }
                        else
                        {
                            message.Bcc.Add(((_EmailConfig.DefaultBCC)));

                        }
                    }


                }
                if (!String.IsNullOrWhiteSpace(cc))
                {
                    foreach (var xx in cc.Split(';').ToList())
                    {
                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.CC.Add(((email)));
                        }
                        else
                        {
                            message.CC.Add(((_EmailConfig.DefaultCC)));

                        }
                    }
                }

                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;

                var smtp = new SmtpClient();
                smtp.Port = _EmailConfig.Port;
                smtp.Host = _EmailConfig.Host;
                smtp.Credentials = new NetworkCredential(_EmailConfig.UserName, _EmailConfig.Password);
                if (_EmailConfig.SecureSocketOptions == MailKit.Security.SecureSocketOptions.None)
                {
                    smtp.EnableSsl = false;
                }
                else
                {
                    smtp.EnableSsl = true;
                }

                smtp.Send(message);

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SentMailToDefault(String Subject, string Body)
        {

            Boolean result = true;

            try
            {
                MailMessage message = new MailMessage();

                message.From = new MailAddress(_EmailConfig.UserName);
                string to = _EmailConfig.DefaultTo;
                string cc = _EmailConfig.DefaultCC;
                string bcc = _EmailConfig.DefaultBCC;

                foreach (var xx in to.Split(';').ToList())
                {
                    var email = xx;
                    if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                    {

                        message.To.Add(((email)));
                    }
                    else
                    {
                        message.To.Add(((_EmailConfig.DefaultTo)));

                    }
                }
                if (!String.IsNullOrWhiteSpace(bcc))
                {
                    foreach (var xx in bcc.Split(';').ToList())
                    {

                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.Bcc.Add(((email)));
                        }
                        else
                        {
                            message.Bcc.Add(((_EmailConfig.DefaultBCC)));

                        }
                    }


                }
                if (!String.IsNullOrWhiteSpace(cc))
                {
                    foreach (var xx in cc.Split(';').ToList())
                    {
                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.CC.Add(((email)));
                        }
                        else
                        {
                            message.CC.Add(((_EmailConfig.DefaultCC)));

                        }
                    }
                }

                message.Subject = Subject;
                message.Body =   Body;
                message.IsBodyHtml = true;

                var smtp = new SmtpClient();
                smtp.Port = _EmailConfig.Port;
                smtp.Host = _EmailConfig.Host;
                smtp.Credentials = new NetworkCredential(_EmailConfig.UserName, _EmailConfig.Password);
                if (_EmailConfig.SecureSocketOptions == MailKit.Security.SecureSocketOptions.None)
                {
                    smtp.EnableSsl = false;
                }
                else
                {
                    smtp.EnableSsl = true;
                }

                smtp.Send(message);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
