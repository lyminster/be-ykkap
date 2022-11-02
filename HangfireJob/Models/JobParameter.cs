using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireJob.Models
{
    public class JobParameter
    {
        public string IDJob { get; set; }
        public string JsonObject { get; set; }
    }
}
