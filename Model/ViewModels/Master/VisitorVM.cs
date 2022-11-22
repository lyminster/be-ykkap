using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class VisitorVM
    {
        public string Idclient { get; set; }
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }

        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AccessFrom { get; set; }
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
    }

    public class JsonVisitorVM : JsonModelBase
    {
        public string Idclient { get; set; }
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }

        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AccessFrom { get; set; }

    }

}
