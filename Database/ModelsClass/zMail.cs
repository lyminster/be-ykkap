using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ModelsClass
{
    public class zMail
    {
        public string sSubject;

        public string HeaderMessage;

        public string FooterMessage;

        public string sBodyMessage;

        public string[] sReceiversTo;

        public string[] sReceiversCC;

        public string[] sReceiversBCC;

        public string[] sAttachment;
    }
}