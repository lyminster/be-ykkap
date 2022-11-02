using System;
using System.Collections.Generic;

namespace Database.ViewModels
{
    public class ViewModelApproval
    {
        public ViewPage1 modelPage1 { get; set; }
        public ViewPage2 modelPage2 { get; set; }
        public ViewPage3 modelPage3 { get; set; }
        
        public List<ApprovalHistory> approvalHistory { get; set; }
    }
}
