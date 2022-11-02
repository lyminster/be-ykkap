using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels.Master
{
    public class StoreViewModel
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string NPWP { get; set; }
        public string NPWPCompany { get; set; }
        public string NPWPAddress1 { get; set; }
        public string NPWPAddress2 { get; set; }
        public DateTime? CloseDate1 { get; set; }
        public DateTime? CloseDate2 { get; set; }
        public string IDClient { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; set; }
    }
}
