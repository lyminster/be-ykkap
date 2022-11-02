using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public partial class ProjectReferences : Entity<string>
    {
        public string name { get; set; }
        public string detail { get; set; }
        public string building { get; set; }
        public string type { get; set; }
        public string startTimeStamp { get; set; }
        public string finishTimestamp { get; set; }
        public string location { get; set; }
        public string urlYoutube { get; set; }
        public string listProductUsed { get; set; }
    }
}
