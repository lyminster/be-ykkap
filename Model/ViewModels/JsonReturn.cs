using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.ViewModels
{
    public class JsonReturn
    {
        [Required]
        public bool result { get; set; }
        [Required]
        public string message { get; set; }
        public object ObjectValue { get; set; }

        public JsonReturn(Boolean? flag)
        {
            if (flag == null)
            {
                result = true;
            }
            else
            {
                result = Convert.ToBoolean(flag);
            }
        }

    }

    public class JsonResponseAPI
    {
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
    }

    public class JsonResponseAPI2
    {
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
        public object Data { get; set; }
    }
}
