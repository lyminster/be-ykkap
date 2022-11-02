using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public partial class CompanyProfile : Entity<string>
    {
        public string about { get; set; }
        public string visionMission { get; set; }
        public string imgUrl { get; set; }
        public string youtubeId { get; set; }
    }
}
