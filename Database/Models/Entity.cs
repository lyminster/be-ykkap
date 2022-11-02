using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Database.Models.HelperFunction;

namespace Database.Models
{
    public class EntityBase
    {
        [NotMapped]
        public ObjectState ModelState
        {
            get;
            set;
        }

        [NotMapped]
        public bool IsTrackedId { get; set; }
    }

    public abstract class Entity<T> : EntityBase, IEntity<string>
    {
        [Key]
        public string ID { get; set; }
        string IEntity.ID
        {
            get { return Guid.NewGuid().ToString("N").ToUpper(); }
        }
        [StringLength(300)]
        public virtual string Idclient { get; set; }

        [StringLength(300)]
        public virtual string CreatedByName { get; set; }
        [StringLength(300)]
        public virtual string CreatedBy { get; set; }
        [StringLength(300)]
        public virtual string LastModifiedByName { get; set; }
        [StringLength(300)]
        public virtual string CreatedByEmployeeID { get; set; }
        [StringLength(300)]
        public virtual string CreatedByEmployeeNIK { get; set; }
        [StringLength(300)]
        public virtual string LastModifiedByEmployeeID { get; set; }
        [StringLength(300)]
        public virtual string LastModifiedByEmployeeNIK { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        [StringLength(300)]
        public virtual string LastModifiedBy { get; set; }
        public virtual Nullable<DateTime> LastModifiedTime { get; set; }
        [Timestamp, ConcurrencyCheck]
        public virtual byte[] TimeStatus { get; set; }
        public virtual int RowStatus { get; private set; }
     
        public virtual void SetRowStatus(RowStatus value)
        {
            RowStatus = (int)value;
        }

    }

}
