using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using System.IO;

namespace EverGreenWebApi.Repository
{
    public class LocalitiesRepository : ILocalitiesRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LocalitiesModel> GetAllLocalities(int PageSize, int PageNumber)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                PageNumber = PageNumber > 0 ? PageNumber - 1 : 0;
                PageSize = PageSize > 0 ? PageSize : 0;

                var result = context.localitymasters.OrderBy(l => l.LocalityId).Skip(PageNumber * PageSize).Take(PageSize);
                var data = result.Select(l => new LocalitiesModel()
                {
                    LocalityId = l.LocalityId,
                    LocalityName = l.LocalityName,
                    ImageUrl = "http://103.233.79.234/Data/SJB_Android/LocalityPictures/" + l.LocalityId + ".jpg",
                }).ToList();

                return data;
            }
        }

        public IEnumerable<LocalitiesModel> GetAllLocalitiesByCity(int cityid)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var result = context.localitymasters.OrderBy(l => l.LocalityName);
                var data = result.Select(l => new LocalitiesModel()
                {
                    LocalityId = l.LocalityId,
                    LocalityName = l.LocalityName,
                    ImageUrl = "http://103.233.79.234/Data/SJB_Android/LocalityPictures/" + l.LocalityId + ".jpg",
                }).ToList();

                return data;
            }
        }

        public LocalitiesModel AddNewLocalityName(LocalitiesModel model)
        {
            //ResponseStatus respponse = new ResponseStatus();
            LocalitiesModel data = new LocalitiesModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {


                var lc = context.localitymasters.Find(model.LocalityId);
                if (lc != null)
                {
                    lc.LocalityName = model.LocalityName;
                    lc.StoreId1 = model.StoreId;
                    lc.StoreId2 = model.StoreId1;
                    lc.StoreId3 = model.StoreId2;
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.LocalityId = lc.LocalityId;
                        data.LocalityName = lc.LocalityName;
                    }
                }
                else
                {
                    localitymaster l = new localitymaster();
                    l.LocalityName = model.LocalityName;
                    l.StoreId1 = model.StoreId;
                    l.StoreId2 = model.StoreId1;
                    l.StoreId3 = model.StoreId2;
                    context.localitymasters.Add(l);
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.LocalityId = l.LocalityId;
                        data.LocalityName = l.LocalityName;                        
                    }
                }
            }
            return data;
        }

        public IEnumerable<LocalitiesModel> GetAllLocalitiesWithOutPagging()
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var data = (from lo in context.localitymasters
                            join st in context.storemasters on lo.StoreId1 equals st.StoreId into j1
                            from j2 in j1.DefaultIfEmpty()
                            join st1 in context.storemasters on lo.StoreId2 equals st1.StoreId into j3
                            from j4 in j3.DefaultIfEmpty()
                            join st2 in context.storemasters on lo.StoreId3 equals st2.StoreId into j5
                            from j6 in j5.DefaultIfEmpty()
                            orderby lo.CreatedOn descending
                            //where lo.LocalityId == l.LocalityId
                            select new LocalitiesModel()
                            {
                                LocalityId = lo.LocalityId,
                                LocalityName = lo.LocalityName,
                                StoreName = j2.StoreName,
                                StoreName1 = j4.StoreName,
                                StoreName2 = j6.StoreName,
                                ImageUrl = "http://103.233.79.234/Data/SJB_Android/LocalityPictures/" + lo.LocalityId + ".jpg",
                            }).ToList();

                return data;
            }
        }

        public IEnumerable<LocalitiesModel> RemoveLocality(int id)
        {
            ResponseStatus response = new ResponseStatus();
            List<LocalitiesModel> data = new List<LocalitiesModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                context.localitymasters.Remove(context.localitymasters.Where(d => d.LocalityId == id).First());
                var result = context.SaveChanges();
                if (result > 0)
                {
                     data = (from lo in context.localitymasters
                                join st in context.storemasters on lo.StoreId1 equals st.StoreId into j1
                                from j2 in j1.DefaultIfEmpty()
                                join st1 in context.storemasters on lo.StoreId2 equals st1.StoreId into j3
                                from j4 in j3.DefaultIfEmpty()
                                join st2 in context.storemasters on lo.StoreId3 equals st2.StoreId into j5
                                from j6 in j5.DefaultIfEmpty()
                                orderby lo.CreatedOn descending
                                //where lo.LocalityId == l.LocalityId
                                select new LocalitiesModel()
                                {
                                    LocalityId = lo.LocalityId,
                                    LocalityName = lo.LocalityName,
                                    StoreName = j2.StoreName,
                                    StoreName1 = j4.StoreName,
                                    StoreName2 = j6.StoreName
                                }).ToList();                   
                }

            }
            return data;
        }

        public LocalitiesModel GetLocalityById(int id)
        {
            //ResponseStatus respponse = new ResponseStatus();
            LocalitiesModel data = new LocalitiesModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                data = (from lo in context.localitymasters
                        join st in context.storemasters on lo.StoreId1 equals st.StoreId into j1
                        from j2 in j1.DefaultIfEmpty()
                        join st1 in context.storemasters on lo.StoreId2 equals st1.StoreId into j3
                        from j4 in j3.DefaultIfEmpty()
                        join st2 in context.storemasters on lo.StoreId3 equals st2.StoreId into j5
                        from j6 in j5.DefaultIfEmpty()
                        orderby lo.CreatedOn descending
                        where lo.LocalityId == id
                        select new LocalitiesModel()
                        {
                            LocalityId = lo.LocalityId,
                            LocalityName = lo.LocalityName,
                            StoreName = j2.StoreName,
                            StoreName1 = j4.StoreName,
                            StoreName2 = j6.StoreName
                        }).FirstOrDefault();
            }
            return data;
        }
    }
}