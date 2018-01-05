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
    public class OrderController : ApiController
    {
        static readonly IOrderRepository _repository = new OrderRepository();

        [HttpPost]
        public HttpResponseMessage CreateOrder(OrderModel model)
        {
            ResponseStatus response = new ResponseStatus();
            //OrderModel data = new OrderModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _repository.CreateOrder(model);
                    if (data.OrderId > 0)
                    {
                        //data.OrderId = result.OrderId;
                        //data.OrderNumber = result.OrderNumber;
                        //data.StoreId = (int)result.StoreId;
                        //data.ProductId = (int)result.ProductId;
                        //data.AddressId = (int)result.AddressId;
                        //data.Quantity = (double)result.Quantity;
                        //data.LoginId = (int)result.LoginId;
                        //data.OrderTime = result.OrderTime;
                        //data.TotalPrice = (double)result.TotalPrice;
                        //data.OrderStatus = result.OrderStatus;
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
        public HttpResponseMessage GetOrderByOrderNumber(OrderModel model)
        {
            ResponseStatus response = new ResponseStatus();
            //OrderModel data = new OrderModel();
            try
            {
                if (model.OrderNumber != "")
                {
                    var data = _repository.GetOrderByOrderNumber(model.OrderNumber);
                    if (data.OrderId > 0)
                    {
                        //data.OrderId = result.OrderId;
                        //data.OrderNumber = result.OrderNumber;
                        //data.StoreId = (int)result.StoreId;
                        //data.ProductId = (int)result.ProductId;
                        //data.AddressId = (int)result.AddressId;
                        //data.Quantity = (double)result.Quantity;
                        //data.LoginId = (int)result.LoginId;
                        //data.OrderTime = result.OrderTime;
                        //data.TotalPrice = (double)result.TotalPrice;
                        //data.OrderStatus = result.OrderStatus;
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
        public HttpResponseMessage GetAllOrderByUser(OrderModel model)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (model.LoginId > 0)
                {
                    var data = _repository.GetAllOrderByUser(model.LoginId);
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
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }


        [HttpPost]
        public HttpResponseMessage TrackOrderStatus(OrderModel model)
        {
            ResponseStatus response = new ResponseStatus();
            //OrderModel data = new OrderModel();
            try
            {
                if (model.OrderNumber != "" && model.LoginId > 0)
                {
                    var data = _repository.TrackOrderStatus(model.LoginId, model.OrderNumber);
                    if (data.OrderId > 0)
                    {
                        //data.OrderId = result.OrderId;
                        //data.OrderNumber = result.OrderNumber;
                        //data.StoreId = (int)result.StoreId;
                        //data.ProductId = (int)result.ProductId;
                        //data.AddressId = (int)result.AddressId;
                        //data.Quantity = (double)result.Quantity;
                        //data.LoginId = (int)result.LoginId;
                        //data.OrderTime = result.OrderTime;
                        //data.TotalPrice = (double)result.TotalPrice;
                        //data.OrderStatus = result.OrderStatus;
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
        public HttpResponseMessage MyAllOrderList(OrderModel model)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (model.LoginId > 0)
                {
                    var data = _repository.MyAllOrderList(model.LoginId);
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
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }
    }
}
