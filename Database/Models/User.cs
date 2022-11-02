using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class User : Entity<String>
    {
        
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
     
        public string IsAD { get; set; }
        public string Jabatan { get; set; }
        public string IDVendor { get; set; }
    }
}
