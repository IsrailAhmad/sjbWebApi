using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using EverGreenWebApi.Repository;

namespace EverGreenWebApi.Controllers
{
    public class StoreController : ApiController
    {
        static readonly IStoreRepository _repository = new StoreRepository();

        [HttpPost]
        public HttpResponseMessage GetAllStoreByLocality(StoreModel model)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (model.LocalityId >= 0)
                {
                    var data = _repository.GetAllStoreByLocality(model.LocalityId, model.LoginId);
                    if (data.Count() > 0)
                    {
                        response.isSuccess = true;
                        response.serverResponseTime = System.DateTime.Now;
                        return Request.CreateResponse(HttpStatusCode.OK, new { data, response });
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.serverResponseTime = System.DateTime.Now;
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please Check Locality Id !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetStoreDetailsById(StoreModel store)
        {
            //var data = new StoreModel();
            var response = new ResponseStatus();
            try
            {
                if (store.StoreId > 0)
                {
                    var data = _repository.GetStoreDetailsById(store.StoreId);
                    if (data != null)
                    {
                        //data.StoreId = result.StoreId;
                        //data.StoreName = result.StoreName;
                        //data.StorePhoneNumber = result.StorePhoneNumber;
                        //data.StoreEmailId = result.StoreEmailId;
                        //data.StoreAddress = result.StoreAddress;
                        //data.LocalityId = result.LocalityId;
                        //data.StorePicturesUrl = result.StorePicturesUrl;
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new { data, response });
                    }
                    else
                    {
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = false;
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetFavouriteStoreByUser(UserModel user)
        {
            var data = new StoreModel();
            var response = new ResponseStatus();
            try
            {
                if (user.LoginID > 0)
                {
                    var result = _repository.GetFavouriteStoreByUser(user.LoginID);
                    if (result != null)
                    {
                        data.StoreId = result.StoreId;
                        data.StoreName = result.StoreName;
                        data.StorePhoneNumber = result.StorePhoneNumber;
                        data.StoreEmailId = result.StoreEmailId;
                        data.StoreAddress = result.StoreAddress;
                        data.OpeningTime = result.OpeningTime;
                        data.ClosingTime = result.ClosingTime;
                        data.StoreStatus = result.StoreStatus;
                        data.LocalityId = result.LocalityId;
                        data.FavouriteStore = result.FavouriteStore;
                        data.StorePicturesUrl = result.StorePicturesUrl;
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new { data, response });
                    }
                    else
                    {
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = false;
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateFavouriteStoreByUser(UserModel user)
        {
            //var data = new StoreModel();
            var response = new ResponseStatus();
            try
            {
                if (user.LoginID > 0 && user.FavouriteStoreId > 0)
                {
                    var data = _repository.AddUpdateFavouriteStoreByUser(user.LoginID, user.FavouriteStoreId);
                    if (data.StoreId > 0)
                    {
                        //data.StoreId = result.StoreId;
                        //data.StoreName = result.StoreName;
                        //data.StorePhoneNumber = result.StorePhoneNumber;
                        //data.StoreEmailId = result.StoreEmailId;
                        //data.StoreAddress = result.StoreAddress;
                        //data.LocalityId = result.LocalityId;
                        //data.FavouriteStore = result.FavouriteStore;
                        //data.StorePicturesUrl = result.StorePicturesUrl;
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new { data, response });
                    }
                    else
                    {
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = false;
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage RemoveFavouriteStoreByUser(UserModel user)
        {

            var response = new ResponseStatus();
            try
            {
                if (user.LoginID > 0)
                {
                    var result = _repository.RemoveFavouriteStoreByUser(user.LoginID);
                    if (result != null)
                    {
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new { response });
                    }
                    else
                    {
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = false;
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage AddNewStore(string storeid, string storename, string phonenumber, string email, string address, string opeingtime, string closingtime, string localityid)
        {
            StoreModel model = new StoreModel();
            model.StoreId = Convert.ToInt32(storeid);
            model.LocalityId = Convert.ToInt32(localityid);
            model.StoreName = storename;
            model.StorePhoneNumber = phonenumber;
            model.StoreEmailId = email;
            model.StoreAddress = address;
            model.OpeningTime = Convert.ToDateTime(opeingtime);
            model.ClosingTime = Convert.ToDateTime(closingtime);
            //model.StoreStatus = storestatus;
            //if (storestatus)
            //{
            //    model.StoreStatus  = "Y";
            //}
            //else
            //{
            //    model.StoreStatus = "N";
            //}

            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.AddNewStore(model);
                //if (data.StoreId > 0)
                //{
                //    response.isSuccess = true;
                //    response.serverResponseTime = DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.OK, new { data });
                //}
                //else
                //{
                //    response.isSuccess = false;
                //    response.serverResponseTime = DateTime.Now;
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                //}
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllStoreList()
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllStoreList();
                if (data.Count() > 0)
                {
                    response.isSuccess = true;
                    response.serverResponseTime = DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.OK, new { data });
                }
                else
                {
                    response.isSuccess = false;
                    response.serverResponseTime = DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage RemoveStore(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.RemoveStore(id);
                if (data.Count() > 0)
                {
                    //response.isSuccess = true;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.OK, new { data });
                }
                else
                {
                    //response.isSuccess = false;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetStoreById(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetStoreById(id);
                if (data.StoreId > 0)
                {
                    //response.isSuccess = true;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.OK, new { data });
                }
                else
                {
                    //response.isSuccess = false;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage StoreOpen(int StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.StoreOpen(StoreId);
                if (data.StoreId > 0)
                {
                    //response.isSuccess = true;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.OK, new { data });
                }
                else
                {
                    //response.isSuccess = false;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage StoreClose(int StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.StoreClose(StoreId);
                if (data.StoreId > 0)
                {
                    //response.isSuccess = true;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.OK, new { data });
                }
                else
                {
                    //response.isSuccess = false;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }
    }
}
