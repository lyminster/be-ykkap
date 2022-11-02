using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class FileLog : Entity<string>
    {
     
        public string TableName { get; set; }
        public string FileName { get; set; }
        public bool? Status { get; set; }
        public string Remarks { get; set; }
 
    }
}
