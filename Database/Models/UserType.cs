using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class UserType : Entity<string>
    {
       
        public string Code { get; set; }
        public string Name { get; set; }
  
        public string RolesCode { get; set; }
    }
}
