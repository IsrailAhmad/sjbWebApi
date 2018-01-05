using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class PromoCodeModel
    {
        public int PromoCodeId { get; set; }
        public string PromoCode { get; set; }
        public decimal Discount { get; set; }       
        public int LoginId { get; set; }
    }
}