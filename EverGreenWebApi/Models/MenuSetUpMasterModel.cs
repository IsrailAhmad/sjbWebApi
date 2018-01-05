using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class MenuSetUpMasterModel
    {
        public int id { get; set; }       
        public string MenuName { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}