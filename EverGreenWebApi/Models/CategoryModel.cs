using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryPictures { get; set; }
    }
}