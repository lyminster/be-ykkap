using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class JobsLog : Entity<string>
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public string TableKey { get; set; }
        public string Description { get; set; }
        public DateTime? JobRunning { get; set; }
      
    }
}
