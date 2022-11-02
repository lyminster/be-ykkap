using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels.Master
{
    public class CatalogVM
    {

    }

    public class JsonCatalogVM : JsonModelBase
    {
        public string name { get; set; }
        public string description { get; set; }
        public string imgUrl { get; set; }
        public List<JsonCatalogDetailVM> child { get; set; }
    }

    public class JsonCatalogDetailVM
    {
        public string name { get; set; }
        public string description { get; set; }
        public string imgUrl { get; set; }
        public string enPdfUrl { get; set; }
        public string idPdfUrl { get; set; }
    }
}
