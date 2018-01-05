using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EverGreenWebApi.Models;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Repository;
using System.IO;
using System.Web;

namespace EverGreenWebApi.Controllers
{
    public class LocalitiesController : ApiController
    {
        static readonly ILocalitiesRepository _repository = new LocalitiesRepository();

        [HttpPost]
        public HttpResponseMessage GetAllLocalities(PaginationModel paging)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllLocalities(paging.PageSize, paging.PageNumber);
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

        //[HttpPost]
        //public HttpResponseMessage GetAllLocalitiesByCity(LocalitiesModel model)
        //{
        //    ResponseStatus response = new ResponseStatus();
        //    try
        //    {
        //        if (model.CityId >= 0)
        //        {
        //            var data = _repository.GetAllLocalitiesByCity(model.CityId);
        //            if (data.Count() > 0)
        //            {
        //                response.isSuccess = true;
        //                response.serverResponseTime = System.DateTime.Now;
        //                return Request.CreateResponse(HttpStatusCode.OK, new { data, response });
        //            }
        //            else
        //            {
        //                response.isSuccess = false;
        //                response.serverResponseTime = System.DateTime.Now;
        //                return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please Check CityId !");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage AddNewLocalityName(string LocalityId, string localityname, string storeid, string storeid1, string storeid2)
        {

            ResponseStatus response = new ResponseStatus();
            try
            {
                LocalitiesModel model = new LocalitiesModel();
                model.LocalityId = Convert.ToInt32(LocalityId);
                model.LocalityName = localityname;
                model.StoreId = Convert.ToInt32(storeid);
                model.StoreId1 = Convert.ToInt32(storeid1);
                model.StoreId2 = Convert.ToInt32(storeid2);
                var data = _repository.AddNewLocalityName(model);
                //if (data.LocalityId > 0)
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
        public HttpResponseMessage GetAllLocalitiesWithOutPagging()
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetAllLocalitiesWithOutPagging();
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
        public HttpResponseMessage RemoveLocality(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.RemoveLocality(id);
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
        public HttpResponseMessage GetLocalityById(int id)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                var data = _repository.GetLocalityById(id);
                if (data.LocalityId > 0)
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
