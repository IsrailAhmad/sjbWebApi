using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Interfaces
{
    public interface ICategoryRepository : IDisposable
    {
        IEnumerable<CategoryModel> GetAllCategoryByMenuId(int menuid, int storeid);
        IEnumerable<CategoryModel> GetAllCategoryList(int StoreId);
        CategoryModel AddNewCategory(CategoryModel model);
        IEnumerable<CategoryModel> RemoveCategory(int id);
        CategoryModel GetCategoryById(int id);
    }
}