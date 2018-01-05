using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class LocalitiesModel
    {
        public int LocalityId { get; set; }
        public string LocalityName { get; set; }       
        public string ImageUrl { get; set; }    
        public int StoreId { get; set; }
        public int StoreId1 { get; set; }
        public int StoreId2 { get; set; }
        public string StoreName { get; set; }
        public string StoreName1 { get; set; }
        public string StoreName2 { get; set; }
    }
}