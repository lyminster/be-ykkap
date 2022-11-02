
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Database.ViewModels
{
    public class ViewPage1
    {
        public long Id { get; set; }

        public List<companySelect> detailcomp { get; set; }
        [Required(ErrorMessage = "Please Choose Company!")]
        public string Company { get; set; }
        public string vendorCodeSAP { get; set; }

        public List<vendorgroupSelect> detailVendorGroup { get; set; }
        [Required(ErrorMessage = "Please Choose Vendor Account Group!")]
        public string VendorGroup { get; set; }

        public List<vendorSelect> detailVendor { get; set; }
        [Required(ErrorMessage = "Please Choose Vendor Type!")]
        public string VendorType { get; set; }

        [Required(ErrorMessage = "Please enter Vendor Entity Name")]
        public string VendorEntityName { get; set; }

        [Required(ErrorMessage = "Please enter Brand !")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Please Choose Registration Complete By !")]
        public string RegistrationCompletedBy { get; set; }

        [Required(ErrorMessage = "Contact Person Need To Be Fill!")]
        public string VendorContactPerson { get; set; }
        [Required(ErrorMessage = "Email Need To Be Fill!")]
        public string VendorEmail { get; set; }
        [Required(ErrorMessage = "Contact Designation Need To Be Fill!")]
        public string VendorContactDesignation { get; set; }

        [Required(ErrorMessage = "Approval 1 is required !")]
        [CekEmailSama("1")]
        public string Approval1 { get; set; }
        [CekEmailSama("2")]
        public string Approval2 { get; set; }
        [CekEmailSama("3")]
        public string Approval3 { get; set; }

        public string StatusResubmit { get; set; }


        public string isSearch { get; set; }
        public string afterSearch { get; set; }

        public string CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public string ClientId { get; set; }
            public bool? IsDeleted { get; set; }
    }

    public class vendorSelect
    {
        public string kodeVendor { get; set; }
        public string namaVendor { get; set; }
    }

    public class vendorgroupSelect
    {
        public string kodeVendorGroup { get; set; }
        public string namaVendorGroup { get; set; }
    }

    public class companySelect
    {
        public string namaCompany { get; set; }
        public string kodeCompany { get; set; }
    }
    public class returnSearchVendorName
    {
        public string displayVendorName { get; set; }
        public string vendorName { get; set; }
        public string idFromA { get; set; }
    }

    public class returnSearchBankKey
    {
        public string ID { get; set; }
        public string bankdesc { get; set; }
        public string BankKey { get; set; }
    }
    public class UrlViewModel
    {
        public string approvalEmail { get; set; }
        public string approvalName { get; set; }
        public string statuscode { get; set; }
        public string Notes { get; set; }
        public string descriptionApproval { get; set; }
        public string tanggal { get; set; }
    }

    public class ApprovalHistory
    {
        public string approvalEmail { get; set; }
        public string approvalName { get; set; }
        public string statuscode { get; set; }
        public string Notes { get; set; }
        public string descriptionApproval { get; set; }
        public string tanggal { get; set; }
    }

    public class vendorGroupView
    {
        public string vendorCode { get; set; }
        public string vendorName { get; set; }
        public bool isSelected { get; set; }
    }


    public class RegisCompTerisi : ValidationAttribute
    {
        private readonly string tipeData;
        public RegisCompTerisi(string asd){
            tipeData = asd;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewpage1 = (ViewPage1)validationContext.ObjectInstance;

            var regiscom = viewpage1.RegistrationCompletedBy;

            if (regiscom == "Vendor")
            {
                if (value == null || value == "")
                {
                    if(tipeData == "Email")
                    {
                        return new ValidationResult("Vendor Email need to be fill");
                    }
                    else if(tipeData == "conDes")
                    {
                        return new ValidationResult("Vendor Contact Designation need to choose");
                    }
                    else
                    {
                        return new ValidationResult("Vendor Contact Person need to be fill");
                    }
                    
                }
                else
                {
                    if(tipeData == "Email")
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
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class CekEmailSama : ValidationAttribute
    {
        private readonly string tipeData;
        public CekEmailSama(string asd)
        {
            tipeData = asd;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewpage1 = (ViewPage1)validationContext.ObjectInstance;

            var ap1 = viewpage1.Approval1;
            var ap2 = viewpage1.Approval2;
            var ap3 = viewpage1.Approval3;

            if(value == null)
            {
                return ValidationResult.Success;
            }
            if(tipeData == "1")
            {
                if (value.ToString() == ap2)
                {
                    return new ValidationResult("Approval cannot be the same!");
                }
                else if (value.ToString() == ap3)
                {
                    return new ValidationResult("Approval cannot be the same!");
                }
            }
            if (tipeData == "2")
            {
                if (value.ToString() == ap1)
                {
                    return new ValidationResult("Approval cannot be the same!");
                }
                else if (value.ToString() == ap3)
                {
                    return new ValidationResult("Approval cannot be the same!");
                }
            }
            if (tipeData == "3")
            {
                if (value.ToString() == ap1)
                {
                    return new ValidationResult("Approval cannot be the same!");
                }
                else if (value.ToString() == ap2)
                {
                    return new ValidationResult("Approval cannot be the same!");
                }
            }
            return ValidationResult.Success;
        }
    }
}


