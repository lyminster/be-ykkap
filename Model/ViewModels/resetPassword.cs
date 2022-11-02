using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.ViewModels
{
    public class ResetPass
    {
        [Required(ErrorMessage = "Please fill in new password!")]
        [numberorEmail("Email")]
        public string email { get; set; }
        
    }
   
}
