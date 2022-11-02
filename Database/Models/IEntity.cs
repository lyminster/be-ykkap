using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Models.HelperFunction;

namespace Database.Models
{
    public interface IEntity
    {
        string ID { get; }
        string CreatedBy { get; set; }
        string CreatedByName { get; set; }
        string LastModifiedByName { get; set; }
        string CreatedByEmployeeID { get; set; }
        string CreatedByEmployeeNIK { get; set; }
        string LastModifiedByEmployeeID { get; set; }
        string LastModifiedByEmployeeNIK { get; set; }
        DateTime CreatedTime { get; set; }
        string LastModifiedBy { get; set; }
        Nullable<DateTime> LastModifiedTime { get; set; }

        byte[] TimeStatus { get; set; }
        int RowStatus { get; }
        ObjectState ModelState { get; set; }
        bool IsTrackedId { get; set; }

        public string Idclient { get; set; }



    }

    public interface IEntity<T> : IEntity
    {
        new T ID { get; set; }
    }
}
