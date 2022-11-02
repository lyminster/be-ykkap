using Database.ConfigClass;
using Database.Models;
using Hangfire;
using HtmlAgilityPack;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HangfireJob.Services
{
    public class EmailService
    {
        private readonly EmailConfig _EmailConfig;
        //private readonly SystemConfig _SystemConfig;
        BusinessModelContext _context;
        public EmailService(EmailConfig emailConfig, BusinessModelContext helperContext, SystemConfig systemConfig)
        {
            _EmailConfig = emailConfig;
            _context = helperContext;

        }

       
        public bool SentMail(String Subject, string Body, string To, String Cc, string Bcc)
        {

            Boolean result = true;

            try
            {
                MimeMessage message = new MimeMessage();

                message.From.Add(MailboxAddress.Parse(_EmailConfig.UserName));
                string to = To;
                string cc = Cc;
                string bcc = Bcc;

                foreach (var xx in to.Split(';').ToList())
                {
                    var email = xx;
                    if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                    {

                        message.To.Add((MailboxAddress.Parse(email)));
                    }
                    else
                    {
                        message.To.Add(MailboxAddress.Parse((_EmailConfig.DefaultTo)));

                    }
                }
                if (!String.IsNullOrWhiteSpace(bcc))
                {
                    foreach (var xx in bcc.Split(';').ToList())
                    {

                        var email = xx;
                        if (!String.IsNullOrWhiteSpace(email) && !email.Contains(".test"))
                        {

                            message.Bcc.Add(MailboxAddress.Parse((email)));
                        }
                        else
                        {
                            message.Bcc.Add(MailboxAddress.Parse((_EmailConfig.DefaultBCC)));

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

                            message.Cc.Add(MailboxAddress.Parse((email)));
                        }
                        else
                        {
                            message.Cc.Add(MailboxAddress.Parse((_EmailConfig.DefaultCC)));

                        }
                    }
                }

                message.Subject = Subject;
                message.Body = new TextPart(TextFormat.Html) { Text = Body };

                var smtp = new SmtpClient();
                smtp.Connect(_EmailConfig.Host, _EmailConfig.Port, SecureSocketOptions.Auto);
                if (_EmailConfig.Password != null)
                {
                    smtp.Authenticate(_EmailConfig.UserName, _EmailConfig.Password);
                }
                smtp.Send(message);
                smtp.Disconnect(true);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
      


        public HttpStatusCode SentRequest(String JsonData, String URL, Boolean isAuthorize, String Token, out string ResultJson, string method)
        {
            ResultJson = "";


            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(method), URL))
                {
                    try
                    {
                        if (isAuthorize)
                        {
                            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + Token);

                        }



                        var response = httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        string result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        ResultJson = result;
                        ResultJson = HttpUtility.HtmlDecode(result);
                        return response.StatusCode;


                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }



            }






        }


        

        public void RefreshJob(String Url)
        {
            try
            {
                var webRequest = WebRequest.Create(Url);

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    var strContent = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {


            }
        }



         


    }
}
