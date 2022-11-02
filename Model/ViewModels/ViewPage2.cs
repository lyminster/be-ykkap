
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Database.ViewModels
{

    public class showpage2
    {
        public ViewPage2 page2 { get; set; }
        public Boolean IsDraft { get; set; }
        //untuk history
        public List<ApprovalHistory> approvalHistory { get; set; }
    }


    public class ViewPage2
    {
        public long Id { get; set; }
        public string FormRegisID { get; set; }

        public string vendorCodeSAP { get; set; }
        public string RegisBy { get; set; }

        [Required(ErrorMessage = "Please enter Supplier Name !")]
        public string SupplierName { get; set; }

        //[Required(ErrorMessage = "Please enter NPWP !")]
        [numberorEmail("npwp")]
        public string Npwp { get; set; }

        public List<pkpSelect> pkpDetail { get; set; }
        [Required(ErrorMessage = "Please select PKP !")]
        public string Pkp { get; set; }

        [Required(ErrorMessage = "Please enter Street Name !")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Please enter Building Number !")]
        public string BuildingNumber { get; set; }

        [Required(ErrorMessage = "Please enter Post Code!")]
        [numberorEmail("number")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Please enter City !")]
        public string City { get; set; }
        public List<countrySelect> countryDetail { get; set; }
        public string Country { get; set; }
        public List<regionSelect> regionDetail { get; set; }
        public string Region { get; set; }

        [SalahSatu("tele")]
        [numberorEmail("number")]
        //[Required(ErrorMessage = "Please enter Telephone Number !")]
        public string TelephoneNumber { get; set; }
        [numberorEmail("number")]
        public string Extention { get; set; }
        [SalahSatu("mobile")]
        [numberorEmail("number")]
        public string MobilePhone { get; set; }
        [numberorEmail("number")]
        public string FaxNumber { get; set; }

        [Required(ErrorMessage = "Please enter Name !")]
        public string ContactPerson1 { get; set; }
        [Required(ErrorMessage = "Please enter Tittle !")]
        public string Title1 { get; set; }
        [numberorEmail("Email")]
        [Required(ErrorMessage = "Please enter Email !")]
        [CekEmailSama2("1")]
        public string EmailContactPerson1 { get; set; }

        [Required(ErrorMessage = "Please enter Name !")]
        public string ContactPerson2 { get; set; }
        [Required(ErrorMessage = "Please enter Tittle !")]
        public string Title2 { get; set; }
        [Required(ErrorMessage = "Please enter Email !")]
        [numberorEmail("Email")]
        [CekEmailSama2("2")]
        public string EmailContactPerson2 { get; set; }


        public string ContactPerson3 { get; set; }
        public string Title3 { get; set; }
        [numberorEmail("Email")]
        [CekEmailSama2("3")]
        public string EmailContactPerson3 { get; set; }


        //[filenameTerisi]
        //public IFormFile StatementLetter { get; set; }
        //public string StatementLetterFileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ClientId { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class pkpSelect
    {
        public string pkpCode { get; set; }
        public string pkpName { get; set; }
    }
    public class regionSelect
    {
        public string regionCode { get; set; }
        public string regionName { get; set; }
    }
    public class countrySelect
    {
        public string countryCode { get; set; }
        public string countryName { get; set; }
    }


    public class numberorEmail : ValidationAttribute
    {
        private readonly string tipeData;
        public numberorEmail(string asd)
        {
            tipeData = asd;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var viewpage2 = (ViewPage2)validationContext.ObjectInstance;
            if(value == null)
            {
                return ValidationResult.Success;
            }

            if (tipeData == "Email")
            {
                string email = value.ToString();
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                {
                    return ValidationResult.Success;
                }

                else
                {
                    return new ValidationResult("Not A Valid Email !");
                }

            }
            else if (tipeData == "npwp")
            {
                string no = value.ToString();
                Regex regex = new Regex(@"^[0-9][0-9][.]([\d]{3})[.]([\d]{3})[.][\d][-]([\d]{3})[.]([\d]{3})$");
                Match match = regex.Match(no);
                if (match.Success)
                {
                    return ValidationResult.Success;
                }

                else
                {
                    return new ValidationResult("Not A Valid NPWP !");
                }
            }
            else
            {
                string no = value.ToString();
                Regex regex = new Regex(@"^[0-9]*$");
                Match match = regex.Match(no);
                if (match.Success)
                {
                    return ValidationResult.Success;
                }

                else
                {
                    return new ValidationResult("Not A Valid Number !");
                }
            }
        }
    }

    public class CekEmailSama2 : ValidationAttribute
    {
        private readonly string tipeData;
        public CekEmailSama2(string asd)
        {
            tipeData = asd;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewpage2 = (ViewPage2)validationContext.ObjectInstance;

            var ap1 = viewpage2.EmailContactPerson1;
            var ap2 = viewpage2.EmailContactPerson2;
            var ap3 = viewpage2.EmailContactPerson3;

            if (value == null)
            {
                return ValidationResult.Success;
            }
            if (tipeData == "1")
            {
                if (value.ToString() == ap2)
                {
                    return new ValidationResult("Email cannot be the same!");
                }
                else if (value.ToString() == ap3)
                {
                    return new ValidationResult("Email cannot be the same!");
                }
            }
            if (tipeData == "2")
            {
                if (value.ToString() == ap1)
                {
                    return new ValidationResult("Email cannot be the same!");
                }
                else if (value.ToString() == ap3)
                {
                    return new ValidationResult("Email cannot be the same!");
                }
            }
            if (tipeData == "3")
            {
                if (value.ToString() == ap1)
                {
                    return new ValidationResult("Email cannot be the same!");
                }
                else if (value.ToString() == ap2)
                {
                    return new ValidationResult("Email cannot be the same!");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class SalahSatu : ValidationAttribute
    {
        private readonly string tipeData;
        public SalahSatu(string asd)
        {
            tipeData = asd;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewpage2 = (ViewPage2)validationContext.ObjectInstance;

            var mobile = viewpage2.MobilePhone;
            var tele = viewpage2.TelephoneNumber;

            if(tipeData == "mobile")
            {
                if(tele == null && value == null)
                {
                    return new ValidationResult("Mobile Number Or Telephone Number must be fill!");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else if (tipeData == "tele")
            {
                if (mobile == null && value == null)
                {
                    return new ValidationResult("Mobile Number Or Telephone Number must be fill!");
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


