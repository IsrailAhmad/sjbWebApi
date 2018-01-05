using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class PaymentGatewayModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string description { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public int LoginID { get; set; }
        public string OrderNumber { get; set; }
        public int StoreId { get; set; }
    }
}