using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Repository
{
    public class StoreRepository : IStoreRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        //string path = "http://103.233.79.234/Data/SJB_Android/StorePictures/";

        //public IEnumerable<StoreModel> GetAllStoreByLocality(int localityid, int loginId)
        //{
        //    List<StoreModel> stores = new List<StoreModel>();
        //    StoreModel store = new StoreModel();
        //    using (sjb_androidEntities context = new sjb_androidEntities())
        //    {
        //        bool fav;
        //        var favouriteStore = (from s in context.storemasters
        //                              join u in context.registrationmasters on s.StoreId equals u.FavouriteStoreId into jointable
        //                              from z in jointable.DefaultIfEmpty()
        //                                  //join l in context.localitymasters on s.StoreId equals l.StoreId1 into j1
        //                                  //from j2 in j1.DefaultIfEmpty()
        //                                  //join l in context.localitymasters on s.StoreId equals l.StoreId2 into j3
        //                                  //from j4 in j1.DefaultIfEmpty()
        //                                  //join l in context.localitymasters on s.StoreId equals l.StoreId1 into j5
        //                                  //from j6 in j1.DefaultIfEmpty()
        //                              where s.LocalityId == localityid && z.LoginID == loginId
        //                              orderby s.StoreName
        //                              select z.FavouriteStoreId).SingleOrDefault();
        //        stores = (from s in context.storemasters
        //                  where s.LocalityId == localityid
        //                  select new StoreModel()
        //                  {
        //                      StoreId = s.StoreId,
        //                      StoreName = s.StoreName,
        //                      StoreEmailId = s.StoreEmailId,
        //                      StorePhoneNumber = s.StorePhoneNumber,
        //                      StoreAddress = s.StoreAddress,
        //                      StorePicturesUrl = path + s.StoreId + "StorePictures.jpg",
        //                      FavouriteStore = s.StoreId == favouriteStore ? true : false,
        //                  }).ToList();

        //    }
        //    return stores;
        //}

        public IEnumerable<StoreModel> GetAllStoreByLocality(int localityid, int loginId)
        {
            List<StoreModel> stores = new List<StoreModel>();
            StoreModel store = new StoreModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                bool fav;
                var favouriteStore = (from s in context.storemasters
                                      join u in context.registrationmasters on s.StoreId equals u.FavouriteStoreId into jointable
                                      from z in jointable.DefaultIfEmpty()
                                          //join l in context.localitymasters on s.StoreId equals l.StoreId1 into j1
                                          //from j2 in j1.DefaultIfEmpty()
                                          //join l in context.localitymasters on s.StoreId equals l.StoreId2 into j3
                                          //from j4 in j1.DefaultIfEmpty()
                                          //join l in context.localitymasters on s.StoreId equals l.StoreId1 into j5
                                          //from j6 in j1.DefaultIfEmpty()
                                      where z.LoginID == loginId && s.StoreStatus == "Y"
                                      orderby s.StoreName
                                      select z.FavouriteStoreId).SingleOrDefault();
                var result = context.localitymasters.Where(s => s.LocalityId == localityid).FirstOrDefault();
                if (result.StoreId1 > 0)
                {
                    store = (from s in context.storemasters
                             where s.StoreId == result.StoreId1
                             select new StoreModel()
                             {
                                 StoreId = s.StoreId,
                                 StoreName = s.StoreName,
                                 StoreEmailId = s.StoreEmailId,
                                 StorePhoneNumber = s.StorePhoneNumber,
                                 StoreAddress = s.StoreAddress,
                                 LocalityId = result.LocalityId,
                                 OpeningTime = (DateTime)s.OpeningTime,
                                 ClosingTime = (DateTime)s.ClosingTime,
                                 StoreStatus = s.StoreStatus == "Y" ? true : false,
                                 StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                                 //StorePicturesUrl = path + s.StoreId + "StorePictures.jpg",
                                 FavouriteStore = s.StoreId == favouriteStore ? true : false,
                             }).FirstOrDefault();
                    stores.Add(store);
                }
                if (result.StoreId2 > 0)
                {
                    store = (from s in context.storemasters
                             where s.StoreId == result.StoreId2
                             select new StoreModel()
                             {
                                 StoreId = s.StoreId,
                                 StoreName = s.StoreName,
                                 StoreEmailId = s.StoreEmailId,
                                 StorePhoneNumber = s.StorePhoneNumber,
                                 StoreAddress = s.StoreAddress,
                                 OpeningTime = (DateTime)s.OpeningTime,
                                 ClosingTime = (DateTime)s.ClosingTime,
                                 StoreStatus = s.StoreStatus == "Y" ? true : false,
                                 LocalityId = result.LocalityId,
                                 StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                                 FavouriteStore = s.StoreId == favouriteStore ? true : false,
                             }).FirstOrDefault();
                    stores.Add(store);
                }
                if (result.StoreId3 > 0)
                {
                    store = (from s in context.storemasters
                             where s.StoreId == result.StoreId3
                             select new StoreModel()
                             {
                                 StoreId = s.StoreId,
                                 StoreName = s.StoreName,
                                 StoreEmailId = s.StoreEmailId,
                                 StorePhoneNumber = s.StorePhoneNumber,
                                 StoreAddress = s.StoreAddress,
                                 OpeningTime = (DateTime)s.OpeningTime,
                                 ClosingTime = (DateTime)s.ClosingTime,
                                 StoreStatus = s.StoreStatus == "Y" ? true : false,
                                 LocalityId = result.LocalityId,
                                 StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                                 FavouriteStore = s.StoreId == favouriteStore ? true : false,
                             }).FirstOrDefault();
                    stores.Add(store);
                }
            }
            return stores;
        }

        public StoreModel GetStoreDetailsById(int storeid)
        {
            StoreModel store = new StoreModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                store = (from s in context.storemasters
                         where s.StoreId == storeid
                         select new StoreModel()
                         {
                             StoreId = s.StoreId,
                             StoreName = s.StoreName,
                             StoreEmailId = s.StoreEmailId,
                             StorePhoneNumber = s.StorePhoneNumber,
                             StoreAddress = s.StoreAddress,
                             StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                             //FavouriteStore = s.StoreId == favouriteStore ? true : false,
                         }).FirstOrDefault();
            }

            return store;
        }

        public StoreModel GetFavouriteStoreByUser(int loginid)
        {
            StoreModel store = new StoreModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                store = (from u in context.registrationmasters
                         join s in context.storemasters on u.FavouriteStoreId equals s.StoreId
                         where u.LoginID == (long)loginid
                         select new StoreModel()
                         {
                             StoreId = s.StoreId,
                             StoreName = s.StoreName,
                             StoreEmailId = s.StoreEmailId,
                             StorePhoneNumber = s.StorePhoneNumber,
                             StoreAddress = s.StoreAddress,
                             OpeningTime = (DateTime)s.OpeningTime,
                             ClosingTime = (DateTime)s.ClosingTime,
                             StoreStatus = s.StoreStatus == "Y" ? true : false,
                             StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                             //FavouriteStore = s.StoreId == favouriteStore ? true : false,
                         }).FirstOrDefault();
            }
            return store;
        }

        public StoreModel AddUpdateFavouriteStoreByUser(int loginid, int storeid)
        {
            StoreModel store = new StoreModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                if (loginid > 0 && storeid > 0)
                {
                    var data = context.registrationmasters.Where(w => w.LoginID.Equals(loginid));
                    foreach (var item in data)
                    {
                        item.FavouriteStoreId = storeid;
                    }
                    try
                    {
                        var resultdata = context.SaveChanges();
                        if (resultdata > 0)
                        {
                            var favouriteStore = (from s in context.storemasters
                                                  join u in context.registrationmasters on s.StoreId equals u.FavouriteStoreId into jointable
                                                  from z in jointable.DefaultIfEmpty()
                                                      //join l in context.localitymasters on s.StoreId equals l.StoreId1 into j1
                                                      //from j2 in j1.DefaultIfEmpty()
                                                      //join l in context.localitymasters on s.StoreId equals l.StoreId2 into j3
                                                      //from j4 in j1.DefaultIfEmpty()
                                                      //join l in context.localitymasters on s.StoreId equals l.StoreId1 into j5
                                                      //from j6 in j1.DefaultIfEmpty()
                                                  where z.LoginID == loginid && s.StoreStatus == "Y"
                                                  orderby s.StoreName
                                                  select z.FavouriteStoreId).SingleOrDefault();
                            var result = (from u in context.registrationmasters
                                          join s in context.storemasters on u.FavouriteStoreId equals s.StoreId
                                          where u.LoginID == (long)loginid && u.FavouriteStoreId == storeid
                                          select new StoreModel()
                                          {
                                              StoreId = s.StoreId,
                                              StoreName = s.StoreName,
                                              StoreStatus = s.StoreStatus == "Y" ? true : false,
                                              OpeningTime = (DateTime)s.OpeningTime,
                                              ClosingTime = (DateTime)s.ClosingTime,
                                              StoreEmailId = s.StoreEmailId,
                                              StorePhoneNumber = s.StorePhoneNumber,
                                              StoreAddress = s.StoreAddress,
                                              StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                                              FavouriteStore = s.StoreId == favouriteStore ? true : false,
                                          }).FirstOrDefault();
                            store = result;
                        }
                        else
                        {
                            var favouriteStore = (from s in context.storemasters
                                                  join u in context.registrationmasters on s.StoreId equals u.FavouriteStoreId into jointable
                                                  from z in jointable.DefaultIfEmpty()
                                                      //join l in context.localitymasters on s.StoreId equals l.StoreId1 into j1
                                                      //from j2 in j1.DefaultIfEmpty()
                                                      //join l in context.localitymasters on s.StoreId equals l.StoreId2 into j3
                                                      //from j4 in j1.DefaultIfEmpty()
                                                      //join l in context.localitymasters on s.StoreId equals l.StoreId1 into j5
                                                      //from j6 in j1.DefaultIfEmpty()
                                                  where z.LoginID == loginid && s.StoreStatus == "Y"
                                                  orderby s.StoreName
                                                  select z.FavouriteStoreId).SingleOrDefault();
                            var result = (from u in context.registrationmasters
                                          join s in context.storemasters on u.FavouriteStoreId equals s.StoreId
                                          where u.LoginID == (long)loginid && u.FavouriteStoreId == storeid
                                          select new StoreModel()
                                          {
                                              StoreId = s.StoreId,
                                              StoreName = s.StoreName,
                                              StoreStatus = s.StoreStatus == "Y" ? true : false,
                                              OpeningTime = (DateTime)s.OpeningTime,
                                              ClosingTime = (DateTime)s.ClosingTime,
                                              StoreEmailId = s.StoreEmailId,
                                              StorePhoneNumber = s.StorePhoneNumber,
                                              StoreAddress = s.StoreAddress,
                                              StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                                              FavouriteStore = s.StoreId == favouriteStore ? true : false,
                                          }).FirstOrDefault();
                            store = result;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Handle ex
                    }
                }
            }
            return store;
        }

        public ResponseStatus RemoveFavouriteStoreByUser(int loginid)
        {
            ResponseStatus response = new ResponseStatus();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                if (loginid > 0)
                {
                    var data = context.registrationmasters.FirstOrDefault(w => w.LoginID == loginid);
                    if (data != null)
                    {
                        data.FavouriteStoreId = 0;
                        context.SaveChanges();
                        response.isSuccess = true;
                        response.serverResponseTime = System.DateTime.Now;
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.serverResponseTime = System.DateTime.Now;
                    }
                }
            }
            return response;
        }

        public StoreModel AddNewStore(StoreModel model)
        {
            StoreModel data = new StoreModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var st = context.storemasters.Find(model.StoreId);
                if (st != null)
                {
                    st.LocalityId = model.LocalityId;
                    st.StoreName = model.StoreName;
                    st.StorePhoneNumber = model.StorePhoneNumber;
                    st.StoreEmailId = model.StoreEmailId;
                    st.StoreAddress = model.StoreAddress;
                    st.OpeningTime = model.OpeningTime;
                    st.ClosingTime = model.ClosingTime;
                    //s.StoreStatus = model.StoreStatus;
                    //if (model.StoreStatus)
                    //{
                    //st.StoreStatus = "Y";
                    //}
                    //else
                    //{
                    //    st.StoreStatus = "N";
                    //}
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.StoreId = st.StoreId;
                    }
                }
                else
                {
                    storemaster s = new storemaster();
                    s.LocalityId = model.LocalityId;
                    s.StoreName = model.StoreName;
                    s.StorePhoneNumber = model.StorePhoneNumber;
                    s.StoreEmailId = model.StoreEmailId;
                    s.StoreAddress = model.StoreAddress;
                    s.OpeningTime = model.OpeningTime;
                    s.ClosingTime = model.ClosingTime;
                    //s.StoreStatus = model.StoreStatus;
                    //if (model.StoreStatus)
                    //{
                    s.StoreStatus = "Y";
                    //}
                    //else
                    //{
                    //    s.StoreStatus = "N";
                    //}
                    context.storemasters.Add(s);
                    var result = context.SaveChanges();
                    if (result > 0)
                    {
                        data.StoreId = s.StoreId;
                    }
                }
            }
            return data;
        }

        public IEnumerable<StoreModel> GetAllStoreList()
        {
            IEnumerable<StoreModel> data;
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var stores = (from s in context.storemasters
                              orderby s.StoreName
                              select new StoreModel()
                              {
                                  StoreId = s.StoreId,
                                  StoreName = s.StoreName,
                                  StoreEmailId = s.StoreEmailId,
                                  StorePhoneNumber = s.StorePhoneNumber,
                                  StoreAddress = s.StoreAddress,
                                  OpeningTime = (DateTime)s.OpeningTime,
                                  ClosingTime = (DateTime)s.ClosingTime,
                                  StoreStatus = s.StoreStatus == "Y" ? true : false,
                                  StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                                  //StorePicturesUrl = "http://localhost/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                              }).ToList();
                data = stores;
            }
            return data;
        }

        public IEnumerable<StoreModel> RemoveStore(int id)
        {
            //ResponseStatus response = new ResponseStatus();
            List<StoreModel> data = new List<StoreModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                context.storemasters.Remove(context.storemasters.Where(d => d.StoreId == id).First());
                var result = context.SaveChanges();
                if (result > 0)
                {
                    data = (from s in context.storemasters
                            orderby s.StoreName
                            select new StoreModel()
                            {
                                StoreId = s.StoreId,
                                StoreName = s.StoreName,
                                StoreEmailId = s.StoreEmailId,
                                StorePhoneNumber = s.StorePhoneNumber,
                                StoreAddress = s.StoreAddress,
                                OpeningTime = (DateTime)s.OpeningTime,
                                ClosingTime = (DateTime)s.ClosingTime,
                                StoreStatus = s.StoreStatus == "Y" ? true : false,
                                StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                            }).ToList();
                }

            }
            return data;
        }

        public StoreModel GetStoreById(int id)
        {
            //ResponseStatus respponse = new ResponseStatus();
            StoreModel data = new StoreModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {

                data = (from s in context.storemasters
                        orderby s.StoreName
                        where s.StoreId == id
                        select new StoreModel()
                        {
                            StoreId = s.StoreId,
                            StoreName = s.StoreName,
                            StoreEmailId = s.StoreEmailId,
                            StorePhoneNumber = s.StorePhoneNumber,
                            StoreAddress = s.StoreAddress,
                            OpeningTime = (DateTime)s.OpeningTime,
                            ClosingTime = (DateTime)s.ClosingTime,
                            StoreStatus = s.StoreStatus == "Y" ? true : false,
                            StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                        }).FirstOrDefault();
            }
            return data;
        }

        public StoreModel StoreOpen(int id)
        {
            //ResponseStatus respponse = new ResponseStatus();
            StoreModel data = new StoreModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var result = context.storemasters.Where(w => w.StoreId == id);
                foreach (var item in result)
                {
                    item.StoreStatus = "Y";
                }
                context.SaveChanges();
                data = (from s in context.storemasters
                        orderby s.StoreName
                        where s.StoreId == id
                        select new StoreModel()
                        {
                            StoreId = s.StoreId,
                            StoreName = s.StoreName,
                            StoreEmailId = s.StoreEmailId,
                            StorePhoneNumber = s.StorePhoneNumber,
                            StoreAddress = s.StoreAddress,
                            OpeningTime = (DateTime)s.OpeningTime,
                            ClosingTime = (DateTime)s.ClosingTime,
                            StoreStatus = s.StoreStatus == "Y" ? true : false,
                            StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                        }).FirstOrDefault();
            }
            return data;
        }

        public StoreModel StoreClose(int id)
        {
            //ResponseStatus respponse = new ResponseStatus();
            StoreModel data = new StoreModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var result = context.storemasters.Where(w => w.StoreId == id);
                foreach (var item in result)
                {
                    item.StoreStatus = "N";
                }
                context.SaveChanges();
                data = (from s in context.storemasters
                        orderby s.StoreName
                        where s.StoreId == id
                        select new StoreModel()
                        {
                            StoreId = s.StoreId,
                            StoreName = s.StoreName,
                            StoreEmailId = s.StoreEmailId,
                            StorePhoneNumber = s.StorePhoneNumber,
                            StoreAddress = s.StoreAddress,
                            OpeningTime = (DateTime)s.OpeningTime,
                            ClosingTime = (DateTime)s.ClosingTime,
                            StoreStatus = s.StoreStatus == "Y" ? true : false,
                            StorePicturesUrl = "http://103.233.79.234/Data/SJB_Android/StorePictures/" + s.StoreId + ".jpg",
                        }).FirstOrDefault();
            }
            return data;
        }
    }
}