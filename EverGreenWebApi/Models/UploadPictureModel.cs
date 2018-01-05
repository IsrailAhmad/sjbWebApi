using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class UploadPictureModel
    {
        public int LoginID { get; set; }
        public string ImageUrl { get; set; }
    }
}