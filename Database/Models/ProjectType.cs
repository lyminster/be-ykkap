﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public partial class ProjectType : Entity<string>
    {
        public string name { get; set; }
    }
}
