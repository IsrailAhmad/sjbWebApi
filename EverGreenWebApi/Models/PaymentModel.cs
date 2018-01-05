using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int StoreId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentId { get; set; }
        public string PaymentOrderId { get; set; }
        public string PaymentMode { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal TotalAmount { get; set; }
    }
}