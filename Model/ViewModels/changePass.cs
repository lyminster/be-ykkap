using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace ViewModel.ViewModels
{
    public class ChangePass
    {
        [Required(ErrorMessage = "Please fill in new password!")]
        public string newPassword { get; set; }
        
        [Required(ErrorMessage = "Please fill in new confirm password")]
        [checkConfirm]
        public string newPasswordConfirm { get; set; }
    }


    
    public class checkConfirm : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ChangePass = (ChangePass)validationContext.ObjectInstance;

            var newPass = ChangePass.newPassword;

            if(newPass == null)
            {
                return ValidationResult.Success;
            }
            if ( value == null)
            {
                return ValidationResult.Success;
            }

            if (newPass != value.ToString())
            {
                return new ValidationResult("New Password and Confirm Password Not same !");
            }
                return ValidationResult.Success;
        }
    }

   
}


