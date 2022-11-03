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
        public string workingHour { get; set; }
        public string telephone { get; set; }
        public string address { get; set; }
    }
}
