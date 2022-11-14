using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class IndexSocialMediaVM
    {
        public List<JsonSocialMediaVM> listIndex { get; set; }
    }
    public class SocialMediaVM
    {
        public string Idclient { get; set; }
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }

        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string urlFb { get; set; }
        public string urlIg { get; set; }
        public string urlYt { get; set; }
        public string urlWeb { get; set; }
    }
    public class JsonSocialMediaVM : JsonModelBase
    {
        public string Idclient { get; set; }
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }

        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string urlFb { get; set; }
        public string urlIg { get; set; }
        public string urlYt { get; set; }
        public string urlWeb { get; set; }

    }
}
