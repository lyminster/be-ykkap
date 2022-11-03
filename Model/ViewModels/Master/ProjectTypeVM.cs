using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels.Master
{
    public class ProjectTypeVM
    {
        public string name { get; set; }
    }
    public class JsonProjectTypeVM : JsonModelBase
    {

        public string name { get; set; }

    }
}
