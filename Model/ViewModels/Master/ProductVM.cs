using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels.Master
{
    public class IndexProductVM
    {
        public List<ProductVM> listIndex { get; set; }
     
        public string UrlFileLog { get; set; }
    }
    public partial class ProductVM
    {
        public string article_ { get; set; }
        public string plu_ { get; set; }
        public string uom { get; set; }
        public decimal uomFactor { get; set; }
        public decimal normalPrice { get; set; }
        public decimal promoPrice { get; set; }
        public decimal originalPrice { get; set; }
        public bool deleted { get; set; }
        public DateTime? updatedOn { get; set; }
        public string updatedBy { get; set; }
    }
}
