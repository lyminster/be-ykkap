using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ViewModel.ViewModels
{
    public class FilterTgl
    {
        public string tglDari { get; set; }
        public string tglSampai { get; set; }
        public string filter { get; set; }
        public List<string> filterIDVendor { get; set; }
        public List<string> ListDistrictDestination { get; set; }
        public List<string> ListDistrictPickup { get; set; }
        public List<string> ListVendor { get; set; }

        public List<string> filterIDDistrictDestination { get; set; }
        public List<string> filterIDDistrictPickup { get; set; }
        public List<string> filterIDIsland { get; set; }

        public string filterJenis { get; set; }

        public string FilterFromString { get; set; }
        public string FilterToString { get; set; }
        public string FilterString { get; set; }
    }
    public class IndexFileLogVM
    {
        public List<FileLogVM> listIndex { get; set; }
        public List<FileLogDetailVM> listDetail { get; set; }
        public IFormFile Upload { get; set; }
        public string FilterDONumber { get; set; }
        public string FilterCreatedTime { get; set; }
        public DateTime? FilterFrom { get; set; }
        private string _FilterFromString;
        public string FilterFromString
        {
            get
            {
                return _FilterFromString;
            }
            set
            {
                _FilterFromString = value;
                if (_FilterFromString != null)
                {
                    DateTime datetimedevice;
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    // It throws Argument null exception  
                    datetimedevice = DateTime.ParseExact(_FilterFromString, "dd-MMM-yyyy", provider);
                    FilterFrom = datetimedevice;
                }
            }
        }
        public String FilterJenis { get; set; }
        public List<FileLogVM> ListFilterJenis { get; set; }
        public DateTime? FilterTo { get; set; }
        private string _FilterToString;
        public string FilterToString
        {
            get
            {
                return _FilterToString;
            }
            set
            {
                _FilterToString = value;
                if (_FilterToString != null)
                {
                    DateTime datetimedevice;
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    // It throws Argument null exception  
                    datetimedevice = DateTime.ParseExact(_FilterToString, "dd-MMM-yyyy", provider);
                    FilterTo = datetimedevice;
                }
            }
        }
        public string UrlFileLog { get; set; }
    }
    public class FileLogVM
    {
        public string ID { get; set; }
        public string TableName { get; set; }
        public string FileName { get; set; }
        public bool? Status { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public List<FileLogDetailVM> ListDetail { get; set; }
    }

    public class FileLogDetailVM
    {
        public string ID { get; set; }
        public string IDFileLog { get; set; }
        public bool? Status { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public string CodeData { get; set; }
        public string SourceTxt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? OrderNo { get; set; }
    }
}
