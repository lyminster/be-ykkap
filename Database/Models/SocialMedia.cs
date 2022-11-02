using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public partial class SocialMedia : Entity<string>
    {

        public string urlFb { get; set; }
        public string urlIg { get; set; }
        public string urlYt { get; set; }
        public string urlWeb { get; set; }

    }
}
