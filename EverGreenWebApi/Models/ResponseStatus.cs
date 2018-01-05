using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class ResponseStatus
    {
        public bool isSuccess { get; set; }
        //public string Message { get; set; }
        public DateTime serverResponseTime { get; set; }

    }
}