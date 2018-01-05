using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using System.Data.Entity;

namespace EverGreenWebApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ProductModel> GetAllProductByCategory(int categoryid, int storeid)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                //string path = "http://103.233.79.234/Data/SJB_Android/ProductPictures/";

                var result = (from p in context.productmasters
                              join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                              from j2 in j1.DefaultIfEmpty()
                              join s in context.storemasters on j2.StoreId equals s.StoreId into j3
                              from j4 in j3.DefaultIfEmpty()
                              where p.CategoryId == categoryid && j4.StoreId == storeid
                              orderby p.ProductName
                              select new ProductModel()
                              {
                                  ProductId = p.ProductId,
                                  ProductName = p.ProductName,
                                  CategoryId = (int)p.CategoryId,
                                  UnitPrice = (decimal)p.UnitPrice,
                                  GST = (decimal)p.GST,
                                  Discount = (decimal)p.Discount,
                                  TaxType = p.TaxType,
                                  UOM = p.UOM,
                                  HSN = p.HSN,
                                  SweetsReset = p.SweetsReset,
                                  ProductDetails = p.ProductDetails,
                                  Lock = p.Lock == "Y" ? true : false,
                                  StoreId = j4.StoreId,
                                  ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                              }).ToList();

                //    var data = result.Select(p => new ProductModel()
                //    {
                //        ProductId = p.ProductId,
                //        ProductName = p.ProductName,
                //        CategoryId = (int)p.CategoryId,
                //        UnitPrice = (decimal)p.UnitPrice,
                //        GST = (decimal)p.GST,
                //        Discount = (decimal)p.Discount,
                //        TaxType = p.TaxType,
                //        UOM = p.UOM,
                //        HSN = p.HSN,                    
                //        SweetsReset = p.SweetsReset,
                //        ProductDetails = p.ProductDetails,
                //        Lock = p.Lock,

                //        //ProductPicturesUrl = path + p.ProductId + "ProductPictures.jpg",
                //}).ToList();
                return result;
            }
        }

        public IEnumerable<ProductModel> GetAllProductList(int StoreId)
        {
            List<ProductModel> data = new List<ProductModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (StoreId == 0)
                {
                    data = (from p in context.productmasters
                            join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                            from j2 in j1.DefaultIfEmpty()
                            where p.Lock == "N"
                            orderby p.CreatedOn
                            select new ProductModel()
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                CategoryId = (int)p.CategoryId,
                                CategoryName = j2.CategoryName,
                                UnitPrice = (decimal)p.UnitPrice,
                                GST = (decimal)p.GST,
                                Discount = (decimal)p.Discount,
                                TaxType = p.TaxType,
                                UOM = p.UOM,
                                HSN = p.HSN,
                                SweetsReset = p.SweetsReset,
                                ProductDetails = p.ProductDetails,
                                Lock = p.Lock == "Y" ? true : false,
                                ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                            }).ToList();
                }
                else
                {
                    data = (from p in context.productmasters
                            join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                            from j2 in j1.DefaultIfEmpty()
                            where j2.StoreId == StoreId && p.Lock == "N"
                            orderby p.CreatedOn
                            select new ProductModel()
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                CategoryId = (int)p.CategoryId,
                                CategoryName = j2.CategoryName,
                                UnitPrice = (decimal)p.UnitPrice,
                                GST = (decimal)p.GST,
                                Discount = (decimal)p.Discount,
                                TaxType = p.TaxType,
                                UOM = p.UOM,
                                HSN = p.HSN,
                                SweetsReset = p.SweetsReset,
                                ProductDetails = p.ProductDetails,
                                Lock = p.Lock == "Y" ? true : false,
                                ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                            }).ToList();
                }

                return data;
            }
        }

        public ProductModel AddNewProduct(ProductModel model)
        {
            ProductModel data = new ProductModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var pr = context.productmasters.Find(model.ProductId);
                if (pr != null)
                {
                    pr.CategoryId = model.CategoryId;
                    pr.ProductName = model.ProductName;
                    pr.UnitPrice = model.UnitPrice;
                    pr.GST = model.GST;
                    pr.Discount = model.Discount;
                    pr.TaxType = model.TaxType;
                    if (model.Lock)
                    {
                        pr.Lock = "Y";
                    }
                    else
                    {
                        pr.Lock = "N";
                    }

                    pr.UOM = model.UOM;
                    pr.ProductDetails = model.ProductDetails;
                    pr.DeliveryCharge = model.DeliveryCharge;
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.ProductId = pr.ProductId;
                    }
                }
                else
                {

                    productmaster p = new productmaster();
                    p.CategoryId = model.CategoryId;
                    p.ProductName = model.ProductName;
                    p.UnitPrice = model.UnitPrice;
                    p.GST = model.GST;
                    p.Discount = model.Discount;
                    p.TaxType = model.TaxType;
                    if (model.Lock)
                    {
                        p.Lock = "Y";
                    }
                    else
                    {
                        p.Lock = "N";
                    }

                    p.UOM = model.UOM;
                    p.ProductDetails = model.ProductDetails;
                    p.DeliveryCharge = model.DeliveryCharge;
                    context.productmasters.Add(p);
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.ProductId = p.ProductId;
                    }
                }
            }
            return data;
        }

        public IEnumerable<ProductModel> RemoveProduct(int id)
        {
            ResponseStatus response = new ResponseStatus();
            List<ProductModel> resultdata = new List<ProductModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                context.productmasters.Remove(context.productmasters.Where(d => d.ProductId == id).First());
                var result = context.SaveChanges();
                if (result > 0)
                {
                    var data = (from p in context.productmasters
                                join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                                from j2 in j1.DefaultIfEmpty()
                                orderby p.CreatedOn
                                select new ProductModel()
                                {
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    CategoryId = (int)p.CategoryId,
                                    CategoryName = j2.CategoryName,
                                    UnitPrice = (decimal)p.UnitPrice,
                                    GST = (decimal)p.GST,
                                    Discount = (decimal)p.Discount,
                                    TaxType = p.TaxType,
                                    UOM = p.UOM,
                                    HSN = p.HSN,
                                    SweetsReset = p.SweetsReset,
                                    ProductDetails = p.ProductDetails,
                                    Lock = p.Lock == "Y" ? true : false,
                                    ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                                }).ToList();
                    resultdata = data;
                }

            }
            return resultdata;
        }

        public ProductModel GetProductById(int id)
        {
            //ResponseStatus respponse = new ResponseStatus();
            ProductModel data = new ProductModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                data = (from p in context.productmasters
                        join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                        from j2 in j1.DefaultIfEmpty()
                        orderby p.CreatedOn
                        where p.ProductId == id
                        select new ProductModel()
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName,
                            CategoryId = (int)p.CategoryId,
                            CategoryName = j2.CategoryName,
                            UnitPrice = (decimal)p.UnitPrice,
                            GST = (decimal)p.GST,
                            Discount = (decimal)p.Discount,
                            TaxType = p.TaxType,
                            UOM = p.UOM,
                            HSN = p.HSN,
                            SweetsReset = p.SweetsReset,
                            ProductDetails = p.ProductDetails,
                            DeliveryCharge = (decimal)p.DeliveryCharge,
                            Lock = p.Lock == "Y" ? true : false,
                            ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                        }).FirstOrDefault();

            }
            return data;
        }

        public IEnumerable<ProductModel> ProductLockOn(int id)
        {
            ResponseStatus response = new ResponseStatus();
            List<ProductModel> resultdata = new List<ProductModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var result = context.productmasters.Where(w => w.ProductId == id);
                foreach (var item in result)
                {
                    item.Lock = "N";
                }
                var saved = context.SaveChanges();

                if (saved > 0)
                {
                    var data = (from p in context.productmasters
                                join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                                from j2 in j1.DefaultIfEmpty()
                                orderby p.CreatedOn
                                select new ProductModel()
                                {
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    CategoryId = (int)p.CategoryId,
                                    CategoryName = j2.CategoryName,
                                    UnitPrice = (decimal)p.UnitPrice,
                                    GST = (decimal)p.GST,
                                    Discount = (decimal)p.Discount,
                                    TaxType = p.TaxType,
                                    UOM = p.UOM,
                                    HSN = p.HSN,
                                    SweetsReset = p.SweetsReset,
                                    ProductDetails = p.ProductDetails,
                                    Lock = p.Lock == "Y" ? true : false,
                                    ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                                }).ToList();
                    resultdata = data;
                }

            }
            return resultdata;
        }

        public IEnumerable<ProductModel> ProductLockOff(int id)
        {
            ResponseStatus response = new ResponseStatus();
            List<ProductModel> resultdata = new List<ProductModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var result = context.productmasters.Where(w => w.ProductId == id);
                foreach (var item in result)
                {
                    item.Lock = "Y";
                }
                var saved = context.SaveChanges();

                if (saved > 0)
                {
                    var data = (from p in context.productmasters
                                join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                                from j2 in j1.DefaultIfEmpty()
                                orderby p.CreatedOn
                                select new ProductModel()
                                {
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    CategoryId = (int)p.CategoryId,
                                    CategoryName = j2.CategoryName,
                                    UnitPrice = (decimal)p.UnitPrice,
                                    GST = (decimal)p.GST,
                                    Discount = (decimal)p.Discount,
                                    TaxType = p.TaxType,
                                    UOM = p.UOM,
                                    HSN = p.HSN,
                                    SweetsReset = p.SweetsReset,
                                    ProductDetails = p.ProductDetails,
                                    Lock = p.Lock == "Y" ? true : false,
                                    ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                                }).ToList();
                    resultdata = data;
                }

            }
            return resultdata;
        }

        public IEnumerable<ProductModel> GetAllOutOfStockProductList(int StoreId)
        {
            List<ProductModel> data = new List<ProductModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (StoreId == 0)
                {
                    data = (from p in context.productmasters
                            join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                            from j2 in j1.DefaultIfEmpty()
                            where p.Lock == "Y"
                            orderby p.CreatedOn
                            select new ProductModel()
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                CategoryId = (int)p.CategoryId,
                                CategoryName = j2.CategoryName,
                                UnitPrice = (decimal)p.UnitPrice,
                                GST = (decimal)p.GST,
                                Discount = (decimal)p.Discount,
                                TaxType = p.TaxType,
                                UOM = p.UOM,
                                HSN = p.HSN,
                                SweetsReset = p.SweetsReset,
                                ProductDetails = p.ProductDetails,
                                Lock = p.Lock == "Y" ? true : false,
                                ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                            }).ToList();
                }
                else
                {
                    data = (from p in context.productmasters
                            join c in context.categorymasters on p.CategoryId equals c.CategoryId into j1
                            from j2 in j1.DefaultIfEmpty()
                            where j2.StoreId == StoreId && p.Lock == "Y"
                            orderby p.CreatedOn
                            select new ProductModel()
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                CategoryId = (int)p.CategoryId,
                                CategoryName = j2.CategoryName,
                                UnitPrice = (decimal)p.UnitPrice,
                                GST = (decimal)p.GST,
                                Discount = (decimal)p.Discount,
                                TaxType = p.TaxType,
                                UOM = p.UOM,
                                HSN = p.HSN,
                                SweetsReset = p.SweetsReset,
                                ProductDetails = p.ProductDetails,
                                Lock = p.Lock == "Y" ? true : false,
                                ProductPicturesUrl = "http://103.233.79.234/Data/SJB_Android/ProductPictures/" + p.ProductId + ".jpg",
                            }).ToList();
                }

                return data;
            }
        }
    }
}