using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class HelperTable : Entity<string>
    {
        
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
   
        public byte[] TimeStamp { get; set; }
    }
}
