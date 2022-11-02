using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class FileLogDetail : Entity<string>
    {
       
        public string IDFileLog { get; set; }
        public int? OrderNo { get; set; }
        public bool? Status { get; set; }
        public string Remarks { get; set; }
  
        public string SourceTxt { get; set; }
        public string CodeData { get; set; }
    }
}
