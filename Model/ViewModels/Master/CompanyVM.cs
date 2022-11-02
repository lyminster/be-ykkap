using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class IndexCompanyVM
    {
        public List<JsonCompanyVM> listIndex { get; set; }
        public IFormFile Upload { get; set; }
        public string UrlFileLog { get; set; }
    }
    public class CompanyVM
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
        [Required(ErrorMessage = "Mandatory")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string Name { get; set; }

        public string Code_Name { get; set; }
    }

    public class JsonCompanyVM : JsonModelBase
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
        public string Code { get; set; }
        public string Name { get; set; }

        public string Code_Name { get; set; }

    }
}
