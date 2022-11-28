using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class IndexProjectReferencesVM
    {
        public List<JsonProjectReferencesVM> listIndex { get; set; }
    }
    public class ProjectReferencesVM
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
        public string name { get; set; }
        public string detail { get; set; }
        public string building { get; set; }
        public string urlImage { get; set; }
        public string type { get; set; }
        public string projectYear { get; set; }
        public string location { get; set; }
        public string urlYoutube { get; set; }
        public string listProductUsed { get; set; }
        public IFormFile Upload { get; set; }
        public List<JsonProjectTypeVM> ListProjectType { get; set; }
        public string ProjectType { get; set; }
    }
    public class JsonProjectReferencesVM : JsonModelBase
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
        public string name { get; set; }
        public string detail { get; set; }
        public string building { get; set; }
        public string urlImage { get; set; }
        public string type { get; set; }
        public string projectYear { get; set; }
        public string location { get; set; }
        public string urlYoutube { get; set; }
        public string listProductUsed { get; set; }
        public IFormFile Upload { get; set; }
        public List<JsonProjectTypeVM> ListProjectType { get; set; }
        public string ProjectType { get; set; }

    }
}
