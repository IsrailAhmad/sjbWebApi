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
    public class ProductController : ApiController
    {
        static readonly IProductRepository _repository = new ProductRepository();

        [HttpPost]
        public HttpResponseMessage GetAllProductByCategory(ProductModel model)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (model.CategoryId >= 0 && model.StoreId > 0)
                {
                    var data = _repository.GetAllProductByCategory(model.CategoryId, model.StoreId);
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
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please Check Category Id !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllProductList(string StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllProductList(Convert.ToInt32(StoreId));
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
        public HttpResponseMessage AddNewProduct(string ProductId,string CategoryName, string ProductName, string UnitPrice, string GST, string Discount, string TaxType, bool Lock, string UOM, string ProductDetails,string DeliveryCharge)
        {
            ProductModel model = new ProductModel();
            model.ProductId = Convert.ToInt32(ProductId);
            model.CategoryId = Convert.ToInt32(CategoryName);
            model.ProductName = ProductName;
            model.UnitPrice = Convert.ToDecimal(UnitPrice);
            model.GST = Convert.ToDecimal(GST);
            model.Discount = Convert.ToDecimal(Discount);
            model.TaxType = TaxType;
            model.Lock = Lock;
            //if (Lock)
            //{
            //    model.Lock = "Y";
            //}
            //else
            //{
            //    model.Lock = "N";
            //}
            model.UOM = UOM;
            model.ProductDetails = ProductDetails;
            model.DeliveryCharge =Convert.ToDecimal(DeliveryCharge);
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.AddNewProduct(model);
                //if (data.ProductId > 0)
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
        public HttpResponseMessage RemoveProduct(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.RemoveProduct(id);
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
        public HttpResponseMessage GetProductById(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetProductById(id);
                if (data.ProductId > 0)
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
        public HttpResponseMessage ProductLockOn(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.ProductLockOn(id);
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
        public HttpResponseMessage ProductLockOff(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.ProductLockOff(id);
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
        public HttpResponseMessage GetAllOutOfStockProductList(string StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllOutOfStockProductList(Convert.ToInt32(StoreId));
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


    }
}
