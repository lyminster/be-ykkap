using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class RolePriviledgeVM
    {
        public string ID { get; set; }
        public int? IDRole { get; set; }
        public string IDPriviledge { get; set; }
        public string IDClient { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; set; }
    }

    public class UserTypeVM
    {
        public int ID { get; set; }
        public string UserTypeCode { get; set; }
        public string UserTypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ClientId { get; set; }
        public bool? IsDeleted { get; set; }
        public string RolesCode { get; set; }
    }



    public class PriviledgeVM
    {
        public string UrlFileLog { get; set; }
    }
}
