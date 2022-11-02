using System;
using System.Collections.Generic;


namespace Database.ViewModels
{
    public class JsonApproval
    {
        public string ID { get; set; }
        public string IDClient { get; set; }
        public string Username { get; set; }
        public string DocNo { get; set; }
        public string DocTypeName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderPositionID { get; set; }
        public string SenderPositionName { get; set; }
        public string IDWorkflowUserGroup { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyID { get; set; }
        public string IDDocument { get; set; }
        public string JsonDocument { get; set; }
        public string Notes { get; set; }

        //1 = approve 2= reject 3= revised
        public int statusID { get; set; }



        public List<JsonApprovalDetail> ListApprovalDetail { get; set; }
    }


    public class JsonApprovalDetail
    {
        public string docNo { get; set; }
        public string docTypeName { get; set; }
        public string senderEmail { get; set; }
        public string senderID { get; set; }
        public string senderName { get; set; }
        public string senderPositionID { get; set; }
        public string senderPositionName { get; set; }



        public string approvalEmail { get; set; }
        public string approvalID { get; set; }
        public string approvalName { get; set; }
        public string approvalPositionID { get; set; }
        public string approvalPositionName { get; set; }

        public int levelApproval { get; set; }
    }

 


    public class DataAD
    {
        public List<AD> DataAd { get; set; }
    }
    public class returnAD
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class AD
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Description { get; set; }
        public string Idclient { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedTime { get; set; }
        public string TimeStatus { get; set; }
        public string RowStatus { get; set; }
    }

    public class UpdateWorkflow
    {
        public string ID { get; set; }
        public string IDClient { get; set; }
        public string Username { get; set; }
        public long Action { get; set; }
        public string IDDocumentWorkflow { get; set; }
        public string Notes { get; set; }
    }

    public class SendEmail
    {
        public string IDClient { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string VendorName { get; set; }
        public string IsRegistered { get; set; }
    }

    public class SendEmailBank
    {
        public string IDClient { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string BankAccountNumber { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string CompanyName { get; set; }
        public string NameOfBank { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }

    public class CreateUserJson
    {
        public string IDClient { get; set; }
        public string Email { get; set; }
        public string IDUser { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PositionID { get; set; }
        public string PositionName { get; set; }
        public string RolesCode { get; set; }
        public List<string> CompanyCode { get; set; }
        public List<string> IDStore { get; set; }
        public List<ListDataDetail> ListDetail { get; set; }
    }

    public class ListDataDetail
    {
        public string ReferenceID { get; set; }
        public string ReferenceCode { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceType { get; set; }
    }
}
