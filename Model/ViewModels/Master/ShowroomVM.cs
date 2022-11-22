using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class IndexShowroomVM
    {
        public List<JsonShowroomVM> listIndex { get; set; }
    }
    public class ShowroomVM
    {
        public string Idclient { get; set; }
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }

        public byte[] TimeStatus { get; set; }
        public IFormFile Upload { get; set; }
        public int RowStatus { get; }
        public int Take { get; set; }
        public int Skip { get; set; }

        [Required(ErrorMessage = "Mandatory")]
        public string name { get; set; }
        
        public string urlImage { get; set; }
         
        public string workingHour { get; set; }
         
        public string address { get; set; }
        
        public string telephone { get; set; }
    }
    public class JsonShowroomVM : JsonModelBase
    {
        public string Idclient { get; set; }
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }
        public IFormFile Upload { get; set; }
        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; }
        public int Take { get; set; }
        public int Skip { get; set; }
    
        public string name { get; set; }
        
        public string urlImage { get; set; }
 
        public string workingHour { get; set; }
    
        public string address { get; set; }
        
        public string telephone { get; set; }

    }
}
