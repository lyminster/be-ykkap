using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels.Transaksi
{
    public class IndexFICHSalesHeaderVM
    {
        public List<FICHSalesHeaderVM> listIndex { get; set; }

        public string UrlFileLog { get; set; }
    }
    public class FICHSalesHeaderVM
    {
        public string Warehouse { get; set; }
        public string RegType { get; set; }
        public string Till { get; set; }
        public string Operation { get; set; }
        public DateTime OpsDate { get; set; }
        public string OpsTime { get; set; }
        public string PrintedSummary { get; set; }
        public string Updated { get; set; }
        public string StaffCode { get; set; }
        public string ReturnType { get; set; }
        public string Status { get; set; }
        public string OnlineOrderID { get; set; }
        public string OpsType { get; set; }
        public string CustomerID { get; set; }
        public string FICHFileName { get; set; }
        public DateTime? ProcessDate { get; set; }
        public byte? Uploaded { get; set; }
        public string Remark { get; set; }
        public DateTime? UploadedOn { get; set; }
        public string UploadedBy { get; set; }
    }
}
