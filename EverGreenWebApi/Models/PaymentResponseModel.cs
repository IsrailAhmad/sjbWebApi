using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class PaymentResponseModel
    {
        public string Message { get; set; }
        public string PaymentURL { get; set; }        
    }
}