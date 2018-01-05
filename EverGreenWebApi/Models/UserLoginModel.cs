using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class UserLoginModel
    {
        public int StoreId { get; set; }
        public bool StoreStatus { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }       
        public string UserName { get; set; }        
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}