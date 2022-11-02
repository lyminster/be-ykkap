
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.ViewModels
{
    public class ViewPage3
    {
        public long Id { get; set; }
        public string FormRegisID { get; set; }
        public string vendorCodeSAP { get; set; }


        public string PaymentOption { get; set; }

        [Required(ErrorMessage = "Account Holder Name Need to fill!")]
        public string AccountHolderName { get; set; }
        [numberorEmail("number")]
        [Required(ErrorMessage = "Bank Account Number Need to fill!")]
        public string RekNumber { get; set; }

        public List<countrySelect> countryDetail { get; set; }
        [Required(ErrorMessage = "Bank country need to choose!")]
        public string BankCountry { get; set; }

        public List<bankCodeSelect> bankCodeDetails { get; set; }
        public string BankNumber { get; set; }
        public string Iban { get; set; }

        [Required(ErrorMessage = "Bank Name Need to fill!")]
        public string NameOfBank { get; set; }
        [Required(ErrorMessage = "Bank Address to fill!")]
        public string BankAddress { get; set; }
        [Required(ErrorMessage = "Bank city to fill!")]
        public string BankCity { get; set; }
        [Required(ErrorMessage = "Bank branch Need to fill!")]
        public string BankBranch { get; set; }
        public string SwiftOrBic { get; set; }
        public bool OtherBank { get; set; }

        //[PaymentOptionFile]
        public IFormFile OriginalConfirmationLetter { get; set; }
        public string OriginalConfirmationLetterFileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ClientId { get; set; }
        public bool? IsDeleted { get; set; }
    }


    public class bankCodeSelect
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public string bankswiftCode { get; set; }
    }

    public class payOpt
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class PaymentOption : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewpage3 = (ViewPage3)validationContext.ObjectInstance;

            var paymentopt = viewpage3.PaymentOption;

            if (paymentopt == "Bank Transfer")
            {
                if(value == null)
                {
                    return new ValidationResult("This field is required !");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class PaymentOptionFile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewpage3 = (ViewPage3)validationContext.ObjectInstance;

            var paymentopt = viewpage3.PaymentOption;
            var filename = viewpage3.OriginalConfirmationLetterFileName;

            if (paymentopt == "Bank Transfer")
            {
                if (value == null)
                {

                    if (filename != "" && filename != null)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Please enter Original Confirmation Letter !");
                    }

                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class otherbankVal : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewpage3 = (ViewPage3)validationContext.ObjectInstance;;
            var otherbank = viewpage3.OtherBank;

            if (otherbank)
            {
                if (value == null)
                {
                    return new ValidationResult("This field is required !");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}


