using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class IndexCatalogTypeVM
    {
        public List<JsonCatalogTypeVM> listIndex { get; set; }
    }
    public class CatalogTypeVM
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
        public string description { get; set; }
        public string imgUrl { get; set; }
        public IFormFile Upload { get; set; }
    }
    public class JsonCatalogTypeVM : JsonModelBase
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
        public string description { get; set; }
        public string imgUrl { get; set; }
        public IFormFile Upload { get; set; }

    }
}
