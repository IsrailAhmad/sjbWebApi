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
    public class CategoryController : ApiController
    {
        static readonly ICategoryRepository _repository = new CategoryRepository();

        [HttpPost]
        public HttpResponseMessage GetAllCategoryByMenuId(CategoryModel model)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (model.MenuId > 0 && model.StoreId > 0)
                {
                    var data = _repository.GetAllCategoryByMenuId(model.MenuId, model.StoreId);
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

        [HttpGet]
        public HttpResponseMessage GetAllCategoryList(string StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {

                var data = _repository.GetAllCategoryList(Convert.ToInt32(StoreId));
                if (data.Count() > 0)
                {
                    //response.isSuccess = true;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.OK, new { data, response });
                }
                else
                {
                    //response.isSuccess = false;
                    //response.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage AddNewCategory(string CategoryId, string categoryname, string menuid, string storeid, string CategoryDescription)
        {
            CategoryModel model = new CategoryModel();
            model.CategoryId = Convert.ToInt32(CategoryId);
            model.CategoryName = categoryname;
            model.StoreId = Convert.ToInt32(storeid);
            model.MenuId = Convert.ToInt32(menuid);
            model.CategoryDescription = CategoryDescription;
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.AddNewCategory(model);
                //if (data.CategoryId > 0)
                //{                   
                return Request.CreateResponse(HttpStatusCode.OK, new { data });
                //}
                //else
                //{                   
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                //}
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage RemoveCategory(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.RemoveCategory(id);
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
        public HttpResponseMessage GetCategoryById(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetCategoryById(id);
                if (data.CategoryId > 0)
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
