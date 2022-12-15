using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class IndexCompanyProfileVM
    {
        public List<JsonCompanyProfileVM> listIndex { get; set; }
    }
    public class CompanyProfileVM
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
        public string about { get; set; }
        public string visionMission { get; set; }
        public string imgUrlProjectReff { get; set; }
        public string imgUrlShowroom { get; set; }
        public string imgUrl { get; set; }
        public string pdfUrl { get; set; }
        public IFormFile Upload { get; set; }
        public IFormFile UploadShowroom { get; set; }
        public IFormFile UploadProjectReff { get; set; }
        public string youtubeId { get; set; }
    }
    public class JsonCompanyProfileVM : JsonModelBase
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

        public IFormFile Upload { get; set; }
        public string about { get; set; }
        public string visionMission { get; set; }
        public string imgUrl { get; set; }
        public string pdfUrl { get; set; }
 
        public string imgUrlProjectReff { get; set; }
        public string imgUrlShowroom { get; set; }
        public string youtubeId { get; set; }

    }
}
