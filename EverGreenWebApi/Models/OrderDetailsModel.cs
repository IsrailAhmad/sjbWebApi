using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class OrderDetailsModel
    {
        public int OrderDetailsId { get; set; }
        public int OrderNumber { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductPicturesUrl { get; set; }
        public string UOM { get; set; }
    }
}