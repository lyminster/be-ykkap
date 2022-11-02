using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public partial class CatalogType : Entity<string>
    {
        public string name { get; set; }
        public string description { get; set; }
        public string imgUrl { get; set; }
    }
}
