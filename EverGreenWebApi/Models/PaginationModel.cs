using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class PaginationModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}