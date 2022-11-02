using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Database.ConfigClass
{
    public class SystemConfig
    {


        [JsonPropertyName("StaticKey")]
        public string StaticKey { get; set; }

        [JsonPropertyName("URLAD")]
        public string URLAD { get; set; }
        [JsonPropertyName("IDClient")]
        public string IDClient { get; set; }
        [JsonPropertyName("IsUsingAD")]
        public string IsUsingAD { get; set; }

        [JsonPropertyName("IsTesting")]
        public string IsTesting { get; set; }
    }

    public class EmailConfig
    {






        [JsonPropertyName("Port")]
        public int Port { get; set; }


        [JsonPropertyName("SecureSocketOptions")]
        public SecureSocketOptions SecureSocketOptions { get; set; }
        [JsonPropertyName("Port2")]
        public int Port2 { get; set; }

        [JsonPropertyName("Host")]
        public string Host { get; set; }

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("emailtoJob")]
        public string emailtoJob { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }


        [JsonPropertyName("tryEmail")]
        public int tryEmail { get; set; }


        [JsonPropertyName("UrlElsa")]
        public string UrlElsa { get; set; }



        [JsonPropertyName("FolderDone")]
        public string FolderDone { get; set; }

        [JsonPropertyName("DefaultCC")]
        public string DefaultCC { get; set; }
        [JsonPropertyName("DefaultTo")]
        public string DefaultTo { get; set; }
        [JsonPropertyName("DefaultBCC")]
        public string DefaultBCC { get; set; }
    }
}

