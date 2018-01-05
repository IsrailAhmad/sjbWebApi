using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Interfaces
{
    public interface IProductRepository : IDisposable
    {
        IEnumerable<ProductModel> GetAllProductByCategory(int categoryid, int storeid);
        IEnumerable<ProductModel> GetAllProductList(int StoreId);
        ProductModel AddNewProduct(ProductModel model);
        IEnumerable<ProductModel> RemoveProduct(int id);
        ProductModel GetProductById(int id);
        IEnumerable<ProductModel> ProductLockOn(int id);
        IEnumerable<ProductModel> ProductLockOff(int id);
        IEnumerable<ProductModel> GetAllOutOfStockProductList(int StoreId);
    }
}