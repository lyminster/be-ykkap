using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class CompanyProfileVM
    {
        public string about { get; set; }
        public string visionMission { get; set; }
        public string imgUrl { get; set; }
        public string youtubeId { get; set; }
    }
    public class JsonCompanyProfileVM : JsonModelBase
    {

        public string about { get; set; }
        public string visionMission { get; set; }
        public string imgUrl { get; set; }
        public string youtubeId { get; set; }

    }
}
