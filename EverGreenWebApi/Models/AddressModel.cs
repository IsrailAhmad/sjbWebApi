using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class AddressModel
    {
        public int AddressId { get; set; }
        public string CompleteAddress { get; set; }        
        public int CityId { get; set; }        
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public string LandMark { get; set; }
        public int LoginID{ get; set; }
        public string PhoneNumber { get; set; }
        public int LocalityId{ get; set; }
        public string LocalityName { get; set; }


    }
}