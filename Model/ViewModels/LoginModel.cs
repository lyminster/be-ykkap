using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Database.ViewModels;
using ViewModel.ViewModels;

namespace MainProject.Models
{
    public class JsonAD
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class JsonToken
    {
        public Boolean Result { get; set; }
        public Object User { get; set; }

        public String Token { get; set; }
        public String RefreshToken { get; set; }

        public string ErrorMessage { get; set; }
    }
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
    public class UserViewModel
    {
        [Key]
        public string Id { get; set; }


        [numberorEmail("Email")]
        [DisplayName("Email")]
        [CekEmail("NONAD")]
        public string Email { get; set; }

        [DisplayName("Email")]
        [CekEmail("AD")]
        public string EmailApproval { get; set; }


        [DisplayName("Password")]
        [PasswordOption]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Choose User Type")]

        public string UserType { get; set; }
        public string UserTypeCode { get; set; }
        public string UserTypeName { get; set; }
        public string Jabatan { get; set; }


        public List<userTypeDetails> UserTypeDetails { get; set; }



        [CompanyOption]
        public List<string> companyList { get; set; }
        public List<string> storeUserList { get; set; }

        public List<companyDetails> CompanyDetails { get; set; }
        public bool isAD { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate { get; set; }
        [VendorOption]
        public string IDVendor { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Idclient { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class PasswordOption : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var uservm = (UserViewModel)validationContext.ObjectInstance;

            var isAD = uservm.isAD;
            var pass = uservm.Password;
            var id = uservm.Id;

            if (isAD != true)
            {
                if (String.IsNullOrEmpty(id))
                {
                    if (pass == null)
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
            else
            {
                return ValidationResult.Success;
            }

        }
    }
    public class VendorOption : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var uservm = (UserViewModel)validationContext.ObjectInstance;

            var usrtype = uservm.UserType;
            var idvndr = uservm.IDVendor;
            var comlist = uservm.companyList;

            if (usrtype == "3")
            {
                if (idvndr == null)
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

    public class CompanyOption : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var uservm = (UserViewModel)validationContext.ObjectInstance;

            var usrtype = uservm.UserType;
            var idvndr = uservm.IDVendor;
            var comlist = uservm.companyList;

            if (usrtype != "3")
            {
                if (comlist == null)
                {
                    return new ValidationResult("Please select at least 1 company!");
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

    public class CekEmail : ValidationAttribute
    {
        private readonly string tipeData;
        public CekEmail(string asd)
        {
            tipeData = asd;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewpage2 = (UserViewModel)validationContext.ObjectInstance;

            var ap1 = viewpage2.Email;
            var ap2 = viewpage2.EmailApproval;
            var isAD = viewpage2.isAD;


            if (isAD)
            {
                if (tipeData == "AD")
                {
                    if (value == null)
                    {
                        return new ValidationResult("Email is required!");
                    }
                }
            }
            else
            {
                if (tipeData == "NONAD")
                {
                    if (value == null)
                    {
                        return new ValidationResult("Email is required!");
                    }

                }
            }

            return ValidationResult.Success;
        }
    }


    public class LoginElsaJson
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginData
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public List<RegisID> FormrRegisID { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class userTypeDetails
    {
        public string id { get; set; }
        public string userTypeCode { get; set; }
        public string userTypeName { get; set; }
    }
    public class companyDetails
    {
        public string companyCode { get; set; }
        public string companyName { get; set; }
    }
    public class jsonLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class returnJson
    {
        public List<ResultArray> Result { get; set; }
        public List<DataArray> Data { get; set; }
    }

    public class ResultArray
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string IsActived { get; set; }
        public string ClaimPermission { get; set; }
    }

    public class DataArray
    {
        public string sAMAccountName { get; set; }
    }

    public class RegisID
    {
        public string RegisIDCode { get; set; }
    }
}