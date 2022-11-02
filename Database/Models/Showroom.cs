using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public partial class Showroom : Entity<string>
    {
        public string name { get; set; }
        public string urlImage { get; set; }
        public string workingHourFrom { get; set; }
        public string workingHourTo { get; set; }
        public string address { get; set; }
        public string building { get; set; }
        public string province { get; set; }
        public string subProvince { get; set; }
        public string telephoneNumber { get; set; }
        public string faxNumber { get; set; }
    }
}
