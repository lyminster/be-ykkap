using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public partial class Company : Entity<string>
    {
       
        public string Code { get; set; }
        public string Name { get; set; }
 
  
     
    }
}
