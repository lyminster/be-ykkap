using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels.Master
{
    public class UserStoreVM
    {
        public long ID { get; set; }
        public long? UserID { get; set; }
        public string IDStore { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ClientId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
