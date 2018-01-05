using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Repository
{
    public class MenuMasterRepository : IMenuMasterRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MenuMasterModel> GetAllMenuList(int StoreId)
        {
            List<MenuMasterModel> data = new List<MenuMasterModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                if (StoreId == 0)
                {
                    data = (from m in context.menumasters
                            join s in context.storemasters on m.StoreId equals s.StoreId into j1
                            from j2 in j1.DefaultIfEmpty()
                            orderby m.CreatedOn descending
                            select new MenuMasterModel()
                            {
                                MenuId = m.MenuId,
                                MenuName = m.MenuName,
                                MenuPrice = (decimal)m.MenuPrice,
                                StoreName = j2.StoreName,
                                ImageUrl = "http://103.233.79.234/Data/SJB_Android/MenuPictures/" + m.MenuId + ".jpg",
                                //ImageUrl = "http://localhost/Data/SJB_Android/MenuPictures/" + m.MenuId + ".jpg",
                            }).ToList();
                }
                else
                {
                    data = (from m in context.menumasters
                            join s in context.storemasters on m.StoreId equals s.StoreId into j1
                            from j2 in j1.DefaultIfEmpty()
                            orderby m.CreatedOn descending
                            where m.StoreId == StoreId
                            select new MenuMasterModel()
                            {
                                MenuId = m.MenuId,
                                MenuName = m.MenuName,
                                MenuPrice = (decimal)m.MenuPrice,
                                StoreName = j2.StoreName,
                                ImageUrl = "http://103.233.79.234/Data/SJB_Android/MenuPictures/" + m.MenuId + ".jpg",
                                //ImageUrl = "http://localhost/Data/SJB_Android/MenuPictures/" + m.MenuId + ".jpg",
                            }).ToList();
                }
                return data;
            }
        }
        public IEnumerable<MenuMasterModel> GetAllMenuListByStoreId(int storeid)
        {
            //string path = "http://103.233.79.234/Data/SJB_Android/MenuPictures/";
            List<MenuMasterModel> datalist = new List<MenuMasterModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var data = (from m in context.menumasters
                            where m.StoreId == storeid
                            select new MenuMasterModel()
                            {
                                MenuId = m.MenuId,
                                MenuName = m.MenuName,
                                ImageUrl = "http://103.233.79.234/Data/SJB_Android/MenuPictures/" + m.MenuId + ".jpg",
                            }).ToList();
                datalist = data;
            }
            return datalist;
        }

        public IEnumerable<MenuMasterModel> GetAllMenuListByStoreIdForWeb(int storeid)
        {
            //string path = "http://103.233.79.234/Data/SJB_Android/MenuPictures/";
            List<MenuMasterModel> datalist = new List<MenuMasterModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var data = (from m in context.menumasters
                            where m.StoreId == storeid
                            select new MenuMasterModel()
                            {
                                MenuId = m.MenuId,
                                MenuName = m.MenuName,
                                //ImageUrl = "http://103.233.79.234/Data/SJB_Android/MenuPictures/" + m.MenuId + ".jpg",
                            }).ToList();
                datalist = data;
            }
            return datalist;
        }

        public MenuMasterModel AddNewMenu(MenuMasterModel model)
        {
            //ResponseStatus respponse = new ResponseStatus();
            MenuMasterModel data = new MenuMasterModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                var me = context.menumasters.Find(model.MenuId);
                if (me != null)
                {
                    me.MenuName = model.MenuName;
                    me.MenuPrice = model.MenuPrice;
                    me.StoreId = model.StoreId;
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.MenuId = me.MenuId;
                    }
                }
                else
                {
                    menumaster m = new menumaster();
                    m.MenuName = model.MenuName;
                    m.MenuPrice = model.MenuPrice;
                    m.StoreId = model.StoreId;
                    context.menumasters.Add(m);
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.MenuId = m.MenuId;
                    }
                }
            }
            return data;
        }

        public IEnumerable<MenuMasterModel> RemoveMenu(int id)
        {
            //ResponseStatus response = new ResponseStatus();
            List<MenuMasterModel> data = new List<MenuMasterModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                context.menumasters.Remove(context.menumasters.Where(d => d.MenuId == id).First());
                var result = context.SaveChanges();
                if (result > 0)
                {
                    data = (from m in context.menumasters
                            join s in context.storemasters on m.StoreId equals s.StoreId into j1
                            from j2 in j1.DefaultIfEmpty()
                            orderby m.CreatedOn descending
                            select new MenuMasterModel()
                            {
                                MenuId = m.MenuId,
                                MenuName = m.MenuName,
                                MenuPrice = (decimal)m.MenuPrice,
                                StoreName = j2.StoreName
                            }).ToList();
                }

            }
            return data;
        }

        public MenuMasterModel GetMenuById(int id)
        {
            //ResponseStatus respponse = new ResponseStatus();
            MenuMasterModel data = new MenuMasterModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                data = (from m in context.menumasters
                        join s in context.storemasters on m.StoreId equals s.StoreId into j1
                        from j2 in j1.DefaultIfEmpty()
                        orderby m.CreatedOn descending
                        where m.MenuId == id
                        select new MenuMasterModel()
                        {
                            MenuId = m.MenuId,
                            MenuName = m.MenuName,
                            MenuPrice = (decimal)m.MenuPrice,
                            StoreName = j2.StoreName
                        }).FirstOrDefault();
            }
            return data;
        }


    }
}