using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public partial class ProjectType : Entity<string>
    {
        public ProjectType()
        {
            ProjectReferences = new HashSet<ProjectReferences>();
        }

        public string name { get; set; }
        public virtual ICollection<ProjectReferences> ProjectReferences { get; set; }
    }
}
