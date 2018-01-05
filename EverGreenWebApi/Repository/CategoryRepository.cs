using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CategoryModel> GetAllCategoryList(int StoreId)
        {
            List<CategoryModel> data = new List<CategoryModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (StoreId == 0)
                {
                    data = (from c in context.categorymasters
                            join s in context.storemasters on c.StoreId equals s.StoreId into j1
                            from j2 in j1.DefaultIfEmpty()
                            join m in context.menumasters on c.MenuId equals m.MenuId into j3
                            from j4 in j3.DefaultIfEmpty()
                            orderby c.CreatedOn descending
                            select new CategoryModel()
                            {
                                CategoryId = c.CategoryId,
                                CategoryName = c.CategoryName,
                                CategoryDescription = c.CategoryDescription,
                                CategoryPictures = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/" + c.CategoryId + ".jpg",
                                StoreName = j2.StoreName,
                                MenuName = j4.MenuName
                            }).ToList();

                }
                else
                {
                    data = (from c in context.categorymasters
                            join s in context.storemasters on c.StoreId equals s.StoreId into j1
                            from j2 in j1.DefaultIfEmpty()
                            join m in context.menumasters on c.MenuId equals m.MenuId into j3
                            from j4 in j3.DefaultIfEmpty()
                            where c.StoreId == StoreId
                            orderby c.CreatedOn descending
                            select new CategoryModel()
                            {
                                CategoryId = c.CategoryId,
                                CategoryName = c.CategoryName,
                                CategoryDescription = c.CategoryDescription,
                                CategoryPictures = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/" + c.CategoryId + ".jpg",
                                StoreName = j2.StoreName,
                                MenuName = j4.MenuName
                            }).ToList();

                }

                return data;
            }
        }

        public IEnumerable<CategoryModel> GetAllCategoryByMenuId(int menuid, int storeid)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                //string path = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/";

                var result = context.categorymasters.Where(c => c.MenuId == menuid && c.StoreId == storeid).OrderBy(c => c.CategoryName);
                var data = result.Select(c => new CategoryModel()
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    StoreId = (int)c.StoreId,
                    CategoryPictures = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/" + c.CategoryId + ".jpg",
                }).ToList();
                return data;
            }
        }

        public IEnumerable<CategoryModel> GetAllCategoryByStoreId(int storeid)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                //string path = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/";

                var result = context.categorymasters.Where(c => c.StoreId == storeid).OrderBy(c => c.CategoryName);
                var data = result.Select(c => new CategoryModel()
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    StoreId = (int)c.StoreId,
                    CategoryPictures = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/" + c.CategoryId + ".jpg",
                }).ToList();
                return data;
            }
        }

        public CategoryModel AddNewCategory(CategoryModel model)
        {
            //ResponseStatus respponse = new ResponseStatus();
            CategoryModel data = new CategoryModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var ca = context.categorymasters.Find(model.CategoryId);
                if (ca != null)
                {
                    ca.CategoryName = model.CategoryName;
                    ca.MenuId = model.MenuId;
                    ca.StoreId = model.StoreId;
                    ca.CategoryDescription = model.CategoryDescription;
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.CategoryId = ca.CategoryId;                       
                    }                   
                }
                else
                {

                    categorymaster c = new categorymaster();
                    c.CategoryName = model.CategoryName;
                    c.MenuId = model.MenuId;
                    c.StoreId = model.StoreId;
                    c.CategoryDescription = model.CategoryDescription;
                    context.categorymasters.Add(c);
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.CategoryId = c.CategoryId;
                    }                   
                }        
            }
            return data;
        }

        public IEnumerable<CategoryModel> RemoveCategory(int id)
        {
            ResponseStatus response = new ResponseStatus();
            List<CategoryModel> data = new List<CategoryModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                context.categorymasters.Remove(context.categorymasters.Where(d => d.CategoryId == id).First());
                var result = context.SaveChanges();
                if (result > 0)
                {
                     data = (from c in context.categorymasters
                                select new CategoryModel()
                                {
                                    CategoryId = c.CategoryId,
                                    CategoryName = c.CategoryName,
                                    CategoryDescription = c.CategoryDescription,
                                    StoreId = (int)c.StoreId,
                                    CategoryPictures = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/" + c.CategoryId + ".jpg",
                                }).ToList();                    
                }
            }
            return data;
        }

        public CategoryModel GetCategoryById(int id)
        {
            //ResponseStatus respponse = new ResponseStatus();
            CategoryModel data = new CategoryModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                data = (from c in context.categorymasters
                        where c.CategoryId ==id
                        select new CategoryModel()
                        {
                            CategoryId = c.CategoryId,
                            CategoryName = c.CategoryName,
                            CategoryDescription = c.CategoryDescription,
                            StoreId = (int)c.StoreId,
                            CategoryPictures = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/" + c.CategoryId + ".jpg",
                        }).FirstOrDefault();

            }
            return data;
        }
    }
}