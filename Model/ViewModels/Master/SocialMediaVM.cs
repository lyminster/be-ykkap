using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class SocialMediaVM
    {
        public string urlFb { get; set; }
        public string urlIg { get; set; }
        public string urlYt { get; set; }
        public string urlWeb { get; set; }
    }
    public class JsonSocialMediaVM : JsonModelBase
    {

        public string urlFb { get; set; }
        public string urlIg { get; set; }
        public string urlYt { get; set; }
        public string urlWeb { get; set; }

    }
}
