using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using EverGreenWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EverGreenWebApi.Controllers
{
    public class UserLoginController : ApiController
    {
        static readonly IUserLoginRepository _repository = new UserLoginRepository();

        [HttpGet]
        public HttpResponseMessage WebsiteLogin(string username, string password, string storeid)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (username != "" && password != "")
                {
                    var data = _repository.WebsiteLogin(username, password, Convert.ToInt32(storeid));
                    if (data.UserID > 0)
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
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllOrders(string storeid)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllOrders(Convert.ToInt32(storeid));
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage MyAllOrderListByStatusId(int StatusId, int StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.MyAllOrderListByStatusId(StatusId, StoreId);
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrderByOrderId(string orderid, string storeid)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetOrderByOrderId(Convert.ToInt32(orderid),Convert.ToInt32(storeid));
                if (data.OrderId > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { data });
                }
                else
                {
                    response.isSuccess = false;
                    response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage AcceptOrder(int orderid, int storeid)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                response = _repository.AcceptOrder(orderid, storeid);
                if (response.isSuccess == true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { response });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage DeclineOrder(int orderid, int storeid)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                response = _repository.DeclineOrder(orderid, storeid);
                if (response.isSuccess == true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { response });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage DispatchOrder(string orderid, int storeid)
        {
            //ResponseStatus response = new ResponseStatus();
            int id = Convert.ToInt32(orderid);
            try
            {
                var data = _repository.DispatchOrder(id, storeid);
                if (data.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { data });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLatestOrderDetails(int StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetLatestOrderDetails(StoreId);
                if (data.OrderId > 0)
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllOrdersByStoreList(int StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllOrdersByStoreList(StoreId);
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

    }
}
