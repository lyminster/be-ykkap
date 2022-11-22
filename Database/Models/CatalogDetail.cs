using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class CatalogDetail : Entity<string>
    {
        public string name { get; set; }
        public string CatalogType { get; set; }
        public string description { get; set; }
        public string imgUrl { get; set; }
        public string enPdfUrl { get; set; }
        public string idPdfUrl { get; set; }
        public int OrderNo { get; set; }
        public virtual CatalogType catalogTypeNavigation { get; set; }
    }
}
