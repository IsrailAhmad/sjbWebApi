using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using EverGreenWebApi.Repository;
using EverGreenWebApi.DBHelper;

namespace EverGreenWebApi.Controllers
{
    public class MenuMasterController : ApiController
    {
        // GET: Menu
        static readonly IMenuMasterRepository _repository = new MenuMasterRepository();

        [HttpGet]
        public HttpResponseMessage GetAllMenuList(string StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllMenuList(Convert.ToInt32(StoreId));
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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllMenuListByStoreId(StoreModel model)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllMenuListByStoreId(model.StoreId);
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
        public HttpResponseMessage AddNewMenu(string menuid, string menuname, decimal menuprice, string storeid)
        {
            MenuMasterModel model = new MenuMasterModel();
            model.MenuId = Convert.ToInt32(menuid);
            model.MenuName = menuname;
            model.MenuPrice = menuprice;
            model.StoreId = Convert.ToInt32(storeid);
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.AddNewMenu(model);
                //if (data.MenuId > 0)
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

        [HttpGet]
        public HttpResponseMessage GetAllCategoryByStoreIdForWeb(string StoreId)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                int StoreID = Convert.ToInt32(StoreId);
                var data = _repository.GetAllMenuListByStoreIdForWeb(StoreID);
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
        public HttpResponseMessage RemoveMenu(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.RemoveMenu(id);
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
        public HttpResponseMessage GetMenuById(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetMenuById(id);
                if (data.MenuId > 0)
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