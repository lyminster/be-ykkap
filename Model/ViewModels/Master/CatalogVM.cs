using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class IndexCatalogDetalVM
    {
        public List<JsonCatalogDetailVM> listIndex { get; set; }
    }
    public class CatalogDetailVM
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
        public string CatalogType { get; set; }
        public string description { get; set; }
        public string imgUrl { get; set; }
        public string enPdfUrl { get; set; }
        public string idPdfUrl { get; set; }
    }

    public class JsonCatalogVM : JsonModelBase
    {
        public string name { get; set; }
        public string description { get; set; }
        public string imgUrl { get; set; }
        public List<JsonCatalogDetailVM>? child { get; set; }
    }

    public class JsonCatalogDetailVM
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
        public string CatalogType { get; set; }
        public string description { get; set; }
        public string imgUrl { get; set; }
        public string enPdfUrl { get; set; }
        public string idPdfUrl { get; set; }
    }
}
