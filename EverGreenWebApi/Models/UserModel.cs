using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class UserModel
    {
        public int LoginID { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public Int32 Otp { get; set; }
        //public DateTime CreatedOn { get; set; } 
        public string EmailID { get; set; }
        public int Role { get; set; }
        public string FbToken { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int FavouriteStoreId { get; set; }
        public string DeviceId { get; set; }
    }
}