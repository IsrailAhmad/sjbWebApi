using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class MenuSetupModel
    {
        public int MenuId { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
    }
}